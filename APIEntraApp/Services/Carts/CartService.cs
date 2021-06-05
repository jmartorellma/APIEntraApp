using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using APIEntraApp.Data;
using APIEntraApp.Data.Models;
using APIEntraApp.Services.Carts.Core;
using APIEntraApp.Services.Carts.Models.DTOs;
using APIEntraApp.Services.Carts.Models.Request;

namespace APIEntraApp.Services.Carts
{
    public class CartService : ICartService
    {
        public async Task<CartDTO> GetByUserIdAsync(int userId, ApiDbContext apiDbContext)
        {
            try
            {
                return await Task.Run(() => {

                    List<User_Product_Cart> userCartList = apiDbContext.Users_Products_Cart.Where(pc => !pc.IsCompleted && pc.UserId == userId).ToList();
                    if (userCartList == null)
                    {
                        throw new Exception($"Error cargadndo el carro del usuario con id {userId}");
                    }

                    if (!userCartList.Any())
                    {
                        return new CartDTO
                        {
                            ProductCartList = new List<ProductCartDTO>(),
                            CartTotal = 0
                        };
                    }

                    return ModelToDTO(userCartList);
                });                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CartDTO> AddAsync(ProductCartPostRequest model, ApiDbContext apiDbContext)
        {
            try
            {
                List<User_Product_Cart> userCartList = apiDbContext.Users_Products_Cart.Where(pc => !pc.IsCompleted && pc.UserId == model.UserId).ToList();
                if (userCartList == null)
                {
                    throw new Exception($"Error cargadndo el carro del usuario con id {model.UserId}");
                }

                apiDbContext.Users_Products_Cart.Add(new User_Product_Cart
                {
                    IsCompleted = false,
                    ProductId = model.ProductId,
                    UserId = model.UserId,
                    Quantity = model.Quantity
                });

                await apiDbContext.SaveChangesAsync();

                return ModelToDTO(apiDbContext.Users_Products_Cart.Where(pc => !pc.IsCompleted && pc.UserId == model.UserId).ToList());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CartDTO> UpdateAsync(ProductCartPutRequest model, ApiDbContext apiDbContext)
        {
            try
            {
                List<User_Product_Cart> userCartList = apiDbContext.Users_Products_Cart.Where(pc => !pc.IsCompleted && pc.UserId == model.UserId).ToList();
                if (userCartList == null)
                {
                    throw new Exception($"Error cargadndo el carro del usuario con id {model.UserId}");
                }

                User_Product_Cart cartLine = await apiDbContext.Users_Products_Cart.FindAsync(model.Id);
                if (cartLine == null)
                {
                    throw new Exception($"Error cargadndo la línea del producto con id {model.ProductId} del carro del usuario con id {model.UserId}");
                }

                cartLine.Quantity = model.Quantity;

                if (cartLine.Quantity == 0) 
                {
                    apiDbContext.Users_Products_Cart.Remove(cartLine);
                }

                await apiDbContext.SaveChangesAsync();

                return ModelToDTO(apiDbContext.Users_Products_Cart.Where(pc => !pc.IsCompleted && pc.UserId == model.UserId).ToList());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<int> RemoveAsync(int id, ApiDbContext apiDbContext)
        {
            try
            {
                User_Product_Cart cartLine = await apiDbContext.Users_Products_Cart.FindAsync(id);
                if (cartLine == null)
                {
                    throw new Exception($"Error cargadndo la línea con id {id}");
                }

                apiDbContext.Users_Products_Cart.Remove(cartLine);

                await apiDbContext.SaveChangesAsync();

                return id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #region Support Methods

        private CartDTO ModelToDTO(List<User_Product_Cart> userCartList)
        {
            CartDTO result = new CartDTO
            {
                ProductCartList = new List<ProductCartDTO>(),
                CartTotal = 0
            };

            userCartList.ForEach(cartLine => 
            {
                ProductCartDTO productLine = new ProductCartDTO
                {
                    Id = cartLine.Id,
                    ProductName = cartLine.Product.Name,
                    ShopName = cartLine.Product.Shop.Name,
                    ShopId = cartLine.Product.Shop.Id,
                    ProductPicture = cartLine.Product.Picture,
                    Quantity = cartLine.Quantity,
                    ProductPvp = cartLine.Product.Pvp * cartLine.Quantity
                };

                result.ProductCartList.Add(productLine);
                result.CartTotal += cartLine.Product.Pvp;
            });

            return result;
        }

        #endregion Support Methods
    }
}
