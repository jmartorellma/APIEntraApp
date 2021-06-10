using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using APIEntraApp.Data;
using APIEntraApp.Data.Models;
using APIEntraApp.Data.Identity;
using APIEntraApp.Services.Shops.Core;
using APIEntraApp.Services.Shops.Models.DTOs;
using APIEntraApp.Services.Shops.Models.Request;
using APIEntraApp.Services.Users.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace APIEntraApp.Services.Shops
{
    public class ShopService : IShopService
    {
        public async Task<List<ShopDTO>> GetAllAsync(ApiDbContext apiDbContext)
        {
            try
            {
                List<ShopDTO> shops = new List<ShopDTO>();

                foreach (Shop appShop in apiDbContext.Shops.ToList())
                {
                    ShopDTO shop = await ModelToDTOAsync(appShop, apiDbContext);
                    if (shop != null)
                    {
                        shops.Add(shop);
                    }
                }

                return shops;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ShopDTO> GetByIdAsync(int id, ApiDbContext apiDbContext)
        {
            try
            {
                Shop shop = await apiDbContext.Shops.FindAsync(id);
                if (shop == null)
                {
                    throw new Exception($"Tienda con id {id} no encontrada");
                }

                return await ModelToDTOAsync(shop, apiDbContext);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ShopDTO> GetByOwnerIdAsync(int ownerId, ApiDbContext apiDbContext)
        {
            try
            {
                Shop shop = apiDbContext.Shops.FirstOrDefault(s => s.OwnerId == ownerId);
                if (shop == null)
                {
                    throw new Exception($"Tienda del usuario con id {ownerId} no encontrada");
                }

                return await ModelToDTOAsync(shop, apiDbContext);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<UserDTO>> GetLockedAsync(int shopId, ApiDbContext apiDbContext, UserManager<ApplicationUser> userManager)
        {
            try
            {
                Shop shop = await apiDbContext.Shops.FindAsync(shopId);
                if (shop == null)
                {
                    throw new Exception($"Tienda con id {shopId} no encontrada");
                }

                List<UserDTO> result = new List<UserDTO>();

                foreach (User_Shop_Locked item in shop.User_Shop_Locked) 
                {
                    result.Add(await UserModelToDTOAsync(item.User, userManager));
                }                

                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ShopDTO> CreateAsync(ShopPostRequest model, ApiDbContext apiDbContext)
        {
            try
            {
                var shopFound = apiDbContext.Shops.FirstOrDefault(s => s.Name.ToUpper().Trim().Equals(model.Name.ToUpper().Trim()));
                if (shopFound != null)
                {
                    throw new Exception($"Ya existe una tienda con el nombre {model.Email}");
                }

                shopFound = apiDbContext.Shops.FirstOrDefault(s => s.Code.ToUpper().Trim().Equals(model.Code.ToUpper().Trim()));
                if (shopFound != null)
                {
                    throw new Exception($"Ya existe una tienda con el código {model.Code}");
                }

                Shop shop = new Shop
                {
                    Nif = model.Nif,
                    IsActive = model.IsActive,
                    Code = model.Code,
                    Name = model.Name,
                    Phone = model.Phone,
                    Email = model.Email,
                    Taxes = model.Taxes,
                    MinAmountTaxes = model.MinAmountTaxes,
                    Address = model.Address,
                    City = model.City,
                    Picture = model.Picture,
                    Web = model.Web,
                    CreationDate = DateTime.Now,
                    OwnerId = model.OwnerId
                };

                await apiDbContext.Shops.AddAsync(shop);
                await apiDbContext.SaveChangesAsync();

                return await ModelToDTOAsync(shop, apiDbContext);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PictureDTO> UpdatePictureAsync(IFormFile file, int shopId, IConfiguration configuration, ApiDbContext apiDbContext)
        {
            try
            {
                Shop shop = await apiDbContext.Shops.FindAsync(shopId);
                if (shop == null)
                {
                    throw new Exception($"No se ha encontrado el proveedor con id {shopId}");
                }

                string folderName = Path.Combine(configuration["ResourcesFolder"].ToString(),
                                                 configuration["ImagesFolder"].ToString(),
                                                 configuration["ShopsFolder"].ToString());

                string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                //string extension = file.ContentType.Split("/")[1];
                //string fileName = $"{shop.Code}.{extension}";

                string fullPath = Path.Combine(pathToSave, file.FileName);
                string dbPath = Path.Combine(folderName, file.FileName);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                FileStream stream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(stream);
                await stream.DisposeAsync();

                shop.Picture = dbPath;
                await apiDbContext.SaveChangesAsync();

                return new PictureDTO 
                { 
                    FilePath = dbPath,
                    ShopId= shopId,
                    OwnerId = shop.OwnerId
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        public async Task<string> AddPaymentethodAsync(ShopPaymentMethodPostRequest model, ApiDbContext apiDbContext) 
        {
            try
            {
                Shop shop = await apiDbContext.Shops.FindAsync(model.ShopId);
                if (shop == null)
                {
                    throw new Exception($"Tienda con id {model.ShopId} no encontrada");
                }

                PaymentMethod paymentMethod = await apiDbContext.PaymentMethods.FindAsync(model.PaymentMethodId);
                if (paymentMethod == null)
                {
                    throw new Exception($"Método de pago con id {model.PaymentMethodId} no encontrado");
                }

                shop.AllowedPaymentMethods.Add(paymentMethod);

                await apiDbContext.SaveChangesAsync();

                return paymentMethod.Code;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<string> AddAllowedPurchaseTypeAsync(ShopPurchaseTypePostRequest model, ApiDbContext apiDbContext)
        {
            try
            {
                Shop shop = await apiDbContext.Shops.FindAsync(model.ShopId);
                if (shop == null)
                {
                    throw new Exception($"Tienda con id {model.ShopId} no encontrada");
                }

                PurchaseType purchaseType = await apiDbContext.PurchaseTypes.FindAsync(model.PurchaseTypeId);
                if (purchaseType == null)
                {
                    throw new Exception($"Tipo de pedido con id {model.PurchaseTypeId} no encontrado");
                }

                shop.Shop_PurchaseTypes.Add(new Shop_PurchaseType 
                { 
                    ShopId = model.ShopId,
                    PurchaseTypeId = model.PurchaseTypeId
                });

                await apiDbContext.SaveChangesAsync();

                return purchaseType.Code;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<int> AddUserLockedAsync(int shopId, int userId, ApiDbContext apiDbContext) 
        {
            try
            {
                User_Shop_Locked userShopLocked = await apiDbContext.Users_Shops_Locked.FindAsync(new { userId, shopId });
                if (userShopLocked != null) 
                {
                    throw new Exception($"El usuario con id {userId} ya se encuenta bloqueado para la tienda con id {shopId}");
                }

                await apiDbContext.Users_Shops_Locked.AddAsync(new User_Shop_Locked 
                { 
                    UserId = userId,
                    ShopId = shopId
                });

                await apiDbContext.SaveChangesAsync();

                return userId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ShopDTO> UpdateAsync(ShopPutRequest model, ApiDbContext apiDbContext)
        {
            try
            {
                var shop = await apiDbContext.Shops.FindAsync(model.Id);
                if (shop == null)
                {
                    throw new Exception($"No existe la tienda {model.Name} con id {model.Id}");
                }

                var shopFoundEmail = apiDbContext.Shops.FirstOrDefault(u => u.Id != model.Id && u.Email.ToUpper().Trim().Equals(model.Email.ToUpper().Trim()));
                if (shopFoundEmail != null)
                {
                    throw new Exception($"Ya existe otra tienda con el Email {model.Email}");
                }

                var shopFoundName = apiDbContext.Shops.FirstOrDefault(u => u.Id != model.Id && u.Name.ToUpper().Trim().Equals(model.Name.ToUpper().Trim()));
                if (shopFoundName != null)
                {
                    throw new Exception($"Ya existe otra tienda {model.Name}");
                }

                var shopFoundCode = apiDbContext.Shops.FirstOrDefault(u => u.Id != model.Id && u.Name.ToUpper().Trim().Equals(model.Code.ToUpper().Trim()));
                if (shopFoundCode != null)
                {
                    throw new Exception($"Ya existe otra tienda con el código {model.Code}");
                }

                shop.IsActive = model.IsActive;
                shop.Code = model.Code;
                shop.Name = model.Name;
                shop.Phone = model.Phone;
                shop.Email = model.Email;
                shop.Taxes = model.Taxes;
                shop.MinAmountTaxes = model.MinAmountTaxes;
                shop.Address = model.Address;
                shop.City = model.City;
                shop.Web = model.Web;
                shop.OwnerId = model.OwnerId;

                await apiDbContext.SaveChangesAsync();

                return await ModelToDTOAsync(shop, apiDbContext);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<int> DeleteAsync(int id, ApiDbContext apiDbContext)
        {
            try
            {
                var shop = await apiDbContext.Shops.FindAsync(id);
                if (shop == null)
                {
                    throw new Exception($"Tienda con id {id} no encontrada");
                }

                apiDbContext.Shops.Remove(shop);
                await apiDbContext.SaveChangesAsync();

                return shop.Id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<string> RemovePaymentMethodAsync(ShopPaymentMethodDeleteRequest model, ApiDbContext apiDbContext)
        {
            try
            {
                Shop shop = await apiDbContext.Shops.FindAsync(model.ShopId);
                if (shop == null)
                {
                    throw new Exception($"Tienda con id {model.ShopId} no encontrada");
                }

                PaymentMethod paymentMethod = await apiDbContext.PaymentMethods.FindAsync(model.PaymentMethodId);
                if (paymentMethod == null)
                {
                    throw new Exception($"Método de pago con id {model.PaymentMethodId} no encontrado");
                }

                shop.AllowedPaymentMethods.Remove(paymentMethod);

                await apiDbContext.SaveChangesAsync();

                return paymentMethod.Code;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<string> RemovePurchaseTypeAsync(ShopPurchaseTypeDeleteRequest model, ApiDbContext apiDbContext)
        {
            try
            {
                Shop shop = await apiDbContext.Shops.FindAsync(model.ShopId);
                if (shop == null)
                {
                    throw new Exception($"Tienda con id {model.ShopId} no encontrada");
                }

                PurchaseType purchaseType = await apiDbContext.PurchaseTypes.FindAsync(model.PurchaseTypeId);
                if (purchaseType == null)
                {
                    throw new Exception($"Tipo de pedido con id {model.PurchaseTypeId} no encontrado");
                }

                Shop_PurchaseType rel = shop.Shop_PurchaseTypes.FirstOrDefault(sp => sp.PurchaseTypeId == model.PurchaseTypeId && sp.ShopId == model.ShopId);
                if (rel == null) 
                {
                    throw new Exception($"Relación entre {shop.Name} y {purchaseType.Name} no encontrada");
                }

                shop.Shop_PurchaseTypes.Remove(rel);

                await apiDbContext.SaveChangesAsync();

                return purchaseType.Code;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<int> RemoveUserLockedAsync(int shopId, int userId, ApiDbContext apiDbContext)
        {
            try
            {
                User_Shop_Locked userShopLocked = await apiDbContext.Users_Shops_Locked.FindAsync(new { userId, shopId });
                if (userShopLocked == null)
                {
                    throw new Exception($"El usuario con id {userId} no se encuenta bloqueado para la tienda con id {shopId}");
                }

                apiDbContext.Users_Shops_Locked.Remove(userShopLocked);

                await apiDbContext.SaveChangesAsync();

                return userId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #region Support Methods

        private async Task<ShopDTO> ModelToDTOAsync(Shop shop, ApiDbContext apiDbContext) 
        {
            ApplicationUser user = await apiDbContext.Users.FindAsync(shop.OwnerId);
            if (user == null)
            {
                throw new Exception($"El usuario propietario {user.UserName} no encontrado");
            }

            return new ShopDTO
            {
                Id = shop.Id,
                Nif = shop.Nif,
                IsActive = shop.IsActive,
                Code = shop.Code,
                Name = shop.Name,
                Phone = shop.Phone,
                Email = shop.Email,
                Taxes = shop.Taxes,
                MinAmountTaxes = shop.MinAmountTaxes,
                Address = shop.Address,
                City = shop.City,
                Picture = shop.Picture,
                Web = shop.Web,
                Owner = user.UserName,
                OwnerId = user.Id,
                CreationDate = shop.CreationDate
            };
        }

        private async Task<UserDTO> UserModelToDTOAsync(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            IList<string> roleList = await userManager.GetRolesAsync(user);
            if (roleList == null || !roleList.Any())
            {
                throw new Exception($"Rol del usuario {user.UserName} no encontrado");
            }

            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                IsActive = user.IsActive,
                UserName = user.UserName,
                Email = user.Email,
                Role = roleList.First(), // Los usuarios solo tendran un rol
                PhoneNumber = user.PhoneNumber,
                CreationDate = user.CreationDate
            };
        }

        #endregion Support Methods
    }
}
