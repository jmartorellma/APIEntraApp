using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using APIEntraApp.Data;
using APIEntraApp.Data.Models;
using APIEntraApp.Data.Identity;
using APIEntraApp.Services.Shops.Core;
using APIEntraApp.Services.Shops.Models.DTOs;
using APIEntraApp.Services.Shops.Models.Request;

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
            catch(Exception e) 
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
                    Address = model.Address,
                    City = model.City,
                    Picture = model.Picture,
                    Web = model.Web,
                    CreationDate = DateTime.Now,
                    OwnerId = model.OwnerId
                };

                await apiDbContext.Shops.AddAsync(shop);
                await apiDbContext.SaveChangesAsync();

                Shop createdShop = apiDbContext.Shops.First(s => s.Code.ToUpper().Trim().Equals(model.Code.ToUpper().Trim()));

                return await ModelToDTOAsync(createdShop, apiDbContext);
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
                shop.Address = model.Address;
                shop.City = model.City;
                shop.Picture = model.Picture;
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
                Address = shop.Address,
                City = shop.City,
                Picture = shop.Picture,
                Web = shop.Web,
                Owner = user.UserName,
                CreationDate = shop.CreationDate
            };
        }

        #endregion Support Methods
    }
}
