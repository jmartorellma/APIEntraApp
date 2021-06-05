using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using APIEntraApp.Data;
using APIEntraApp.Data.Models;
using APIEntraApp.Services.Purchases.Core;
using APIEntraApp.Services.Purchases.Models.DTOs;

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
