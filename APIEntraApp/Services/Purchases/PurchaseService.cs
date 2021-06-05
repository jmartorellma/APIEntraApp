using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using APIEntraApp.Data;
using APIEntraApp.Data.Models;
using APIEntraApp.Services.Purchases.Core;
using APIEntraApp.Services.Purchases.Models.DTOs;
using APIEntraApp.Services.Purchases.Models.Request;
using Microsoft.Extensions.Configuration;

namespace APIEntraApp.Services.Purchases
{
    public class PurchaseService : IPurchaseService
    {
        public async Task<PurchaseDTO> GetByIdAsync(int id, ApiDbContext apiDbContext)
        {
            try
            {
                Purchase purchase = await apiDbContext.Purchases.FindAsync(id);
                if (purchase == null)
                {
                    throw new Exception($"Error cargadndo la compra con id {id}");
                }

                return ModelToDTO(purchase);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<PurchaseDTO>> GetByUserIdAsync(int userId, ApiDbContext apiDbContext)
        {
            try
            {
                return await Task.Run(() => {

                    List<Purchase> userPurchaseList = apiDbContext.Purchases.Where(p => p.Purchase_Carts.FirstOrDefault(pc => pc.UserProductCart.IsCompleted && 
                                                                                                                              pc.UserProductCart.UserId == userId) != null).ToList();
                    if (userPurchaseList == null)
                    {
                        throw new Exception($"Error cargadndo el listado ce compras del usuario con id {userId}");
                    }

                    return userPurchaseList.Select(s => ModelToDTO(s)).ToList(); ;
                });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<PurchaseDTO>> GetByShopIdAsync(int shopId, ApiDbContext apiDbContext)
        {
            try
            {
                return await Task.Run(() => {

                    List<Purchase> userPurchaseList = apiDbContext.Purchases.Where(p => p.Purchase_Carts.FirstOrDefault(pc => pc.UserProductCart.IsCompleted && 
                                                                                                                              pc.UserProductCart.Product.ShopId == shopId) != null).ToList();
                    if (userPurchaseList == null)
                    {
                        throw new Exception($"Error cargadndo el listado ce compras de la tienda con id {shopId}");
                    }

                    return userPurchaseList.Select(s => ModelToDTO(s)).ToList(); ;
                });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PurchaseDTO> CreateAsync(PurchasePostRequest model, ApiDbContext apiDbContext, IConfiguration configuration)
        {
            try
            {
                Purchase newPurchase = new Purchase
                {
                    StatusDate = DateTime.Now,
                    CreationDate = DateTime.Now,
                    PaymentMethodId = model.PaymentMethoId,
                    PurchaseTypeId = model.PurchaseTypeId,
                    PaymentStatusId = model.PaymentStatusId
                };

                decimal amount = 0;

                // Todos los productos deben pertenecer a la misma tienda - Se crea un purchase por tienda en el carro
                model.ProductCartIdList.ForEach(id =>
                {
                    decimal am = apiDbContext.Users_Products_Cart.Where(pc => pc.Id == id).First().Product.Pvp;
                    amount += am;

                    newPurchase.Purchase_Carts.Add(new Purchase_Cart
                    {
                        Purchase = newPurchase,
                        UserProductCartId = id
                    });
                });

                newPurchase.Amount = amount;

                await apiDbContext.Purchases.AddAsync(newPurchase);

                await apiDbContext.SaveChangesAsync();

                PurchaseType type = await apiDbContext.PurchaseTypes.FindAsync(newPurchase.PurchaseTypeId);

                if (type != null && type.Code.ToUpper().Trim().Equals(configuration["OnlinePurchasTypeCode"].ToString().ToUpper().Trim()) && model.DeliveryData != null) 
                {
                    Shop shop = apiDbContext.Users_Products_Cart.Where(pc => model.ProductCartIdList.Contains(pc.Id)).ToList().First().Product.Shop;

                    Delivery newDelivery = new Delivery
                    {
                        PurchaseId = newPurchase.PurchaseTypeId,
                        DeliveryDate = DateTime.Now.AddDays(5),
                        DeliveryTaxes = amount < shop.MinAmountTaxes ? shop.Taxes : 0,
                        Adderess = model.DeliveryData.Address,
                        Number = model.DeliveryData.Number,
                        City = model.DeliveryData.City,
                        PostCode = model.DeliveryData.PostCode,
                        Region = model.DeliveryData.Region,
                        CreationDate = DateTime.Now
                    };

                    await apiDbContext.SaveChangesAsync();
                }

                return ModelToDTO(newPurchase);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PurchaseDTO> UpdateAsync(PurchasePutRequest model, ApiDbContext apiDbContext, IConfiguration configuration)
        {
            try
            {
                Purchase purchase = await apiDbContext.Purchases.FindAsync(model.Id);
                if (purchase == null)
                {
                    throw new Exception($"No existe la compra con id {model.Id}");
                }

                purchase.Purchase_Carts.RemoveAll(pc => !model.ProductCartIdList.Contains(pc.UserProductCartId));
                model.ProductCartIdList.Where(mp => !purchase.Purchase_Carts.Select(s => s.PurchaseId).ToList().Contains(mp)).ToList().ForEach(pId =>
                {
                    purchase.Purchase_Carts.Add(new Purchase_Cart
                    {
                        PurchaseId = model.Id,
                        UserProductCartId = pId
                    });
                });


                Purchase newPurchase = new Purchase
                {
                    StatusDate = DateTime.Now,
                    CreationDate = DateTime.Now,
                    PaymentMethodId = model.PaymentMethoId,
                    PurchaseTypeId = model.PurchaseTypeId,
                    PaymentStatusId = model.PaymentStatusId
                };

                decimal amount = 0;

                // Todos los productos deben pertenecer a la misma tienda - Se crea un purchase por tienda en el carro
                model.ProductCartIdList.ForEach(id =>
                {
                    decimal am = apiDbContext.Users_Products_Cart.Where(pc => pc.Id == id).First().Product.Pvp;
                    amount += am;

                    newPurchase.Purchase_Carts.Add(new Purchase_Cart
                    {
                        Purchase = newPurchase,
                        UserProductCartId = id
                    });
                });

                newPurchase.Amount = amount;

                await apiDbContext.Purchases.AddAsync(newPurchase);

                await apiDbContext.SaveChangesAsync();

                PurchaseType type = await apiDbContext.PurchaseTypes.FindAsync(newPurchase.PurchaseTypeId);

                if (type != null && type.Code.ToUpper().Trim().Equals(configuration["OnlinePurchasTypeCode"].ToString().ToUpper().Trim()) && model.DeliveryData != null)
                {
                    Shop shop = apiDbContext.Users_Products_Cart.Where(pc => model.ProductCartIdList.Contains(pc.Id)).ToList().First().Product.Shop;

                    Delivery newDelivery = new Delivery
                    {
                        PurchaseId = newPurchase.PurchaseTypeId,
                        DeliveryDate = DateTime.Now.AddDays(5),
                        DeliveryTaxes = amount < shop.MinAmountTaxes ? shop.Taxes : 0,
                        Adderess = model.DeliveryData.Address,
                        Number = model.DeliveryData.Number,
                        City = model.DeliveryData.City,
                        PostCode = model.DeliveryData.PostCode,
                        Region = model.DeliveryData.Region,
                        CreationDate = DateTime.Now
                    };

                    await apiDbContext.SaveChangesAsync();
                }

                return ModelToDTO(newPurchase);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #region Support Methods

        private PurchaseDTO ModelToDTO(Purchase purchase)
        {

            List<PurchaseProductDTO> purchaseProducts = new List<PurchaseProductDTO>();

            purchase.Purchase_Carts.Select(s => s.UserProductCart.Product).ToList().ForEach(product => 
            {
                purchaseProducts.Add(new PurchaseProductDTO 
                {
                    Id = product.Id,
                    Code = product.Code,
                    Name = product.Name,
                    Price = product.Price,
                    Tax = product.Tax,
                    Pvp = product.Pvp,
                    Picture = product.Picture,
                    Shop = product.Shop.Name
                });
            });

            string userName = purchase.Purchase_Carts.Select(s => s.UserProductCart.User.UserName).First();

            return new PurchaseDTO 
            {
               ProductList = purchaseProducts,
               Amount = purchase.Amount,
               UserName = userName,
               StatusDate = purchase.StatusDate,
               CreationDate = purchase.StatusDate,
               PaymentMethod = purchase.PaymentMethod.Name,
               PurchaseType = purchase.PurchaseType.Name,
               PaymentStatus = purchase.PaymentStatus.Name
            };
        }

        #endregion Support Methods
    }
}
