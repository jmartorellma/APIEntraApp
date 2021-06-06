using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using APIEntraApp.Data;
using APIEntraApp.Data.Identity;
using APIEntraApp.Services.Users.Core;
using APIEntraApp.Services.Users.Models.DTOs;
using APIEntraApp.Services.Users.Models.Request;
using APIEntraApp.Data.Models;
using System.Security.Claims;

namespace APIEntraApp.Services.Users
{
    public class UserService : IUserService
    {
        public async Task<List<UserDTO>> GetAllAsync(UserManager<ApplicationUser> userManager)
        {
            try
            {
                List<UserDTO> users = new List<UserDTO>();

                foreach (ApplicationUser appUser in userManager.Users.ToList()) 
                {
                    UserDTO user = await ModelToDTOAsync(appUser, userManager);
                    if (user != null) 
                    {
                        users.Add(user);
                    }
                }

                return users;
            }
            catch(Exception e) 
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<string>> GetRolesAsync(ApiDbContext apiDbContext, UserManager<ApplicationUser> userManager, ClaimsPrincipal currentUser)
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(currentUser);

                if (await userManager.IsInRoleAsync(user, "SuperUser")) 
                {
                    return await Task.Run(() => apiDbContext.Roles.Select(r => r.Name).ToList());
                }

                return await Task.Run(() => apiDbContext.Roles.Where(r => !r.NormalizedName.Equals("SUPERUSER") && !r.NormalizedName.Equals("ADMIN")).Select(r => r.Name).ToList());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserDTO> GetByIdAsync(int id, UserManager<ApplicationUser> userManager)
        {
            try
            {
                ApplicationUser user = await userManager.FindByIdAsync(id.ToString());
                if (user == null) 
                {
                    throw new Exception($"Usuario con id {id} no encontrado");
                }

                return await ModelToDTOAsync(user, userManager);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserDTO> CreateAsync(UserPostRequest model, UserManager<ApplicationUser> userManager)
        {
            try
            {
                var userFound = await userManager.FindByEmailAsync(model.Email);
                if (userFound != null)
                {
                    throw new Exception($"Ya existe un usuario en el sisitema con el Email {model.Email}");
                }

                userFound = await userManager.FindByNameAsync(model.Username);
                if (userFound != null)
                {
                    throw new Exception($"Ya existe un usuario en el sisitema con el Usuario {model.Username}");
                }

                ApplicationUser applicationUser = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Name = model.Name,
                    Surname = model.Surname,
                    CreationDate = DateTime.Now,
                    IsActive = true
                };

                var createResult = await userManager.CreateAsync(applicationUser, model.Password);

                if (!createResult.Succeeded)
                {
                    throw new Exception($"ERROR dando de alta el usuario - {createResult.Errors}");
                }

                var user = await userManager.FindByEmailAsync(applicationUser.Email);
                var roleresult = await userManager.AddToRoleAsync(user, model.Role);

                if (!roleresult.Succeeded)
                {
                    await userManager.DeleteAsync(user);
                    throw new Exception($"ERROR asignando el rol de usuario {model.Role} - {roleresult.Errors}");
                }

                return new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    IsActive = user.IsActive,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = model.Role,
                    PhoneNumber = user.PhoneNumber,
                    CreationDate = user.CreationDate
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<string> UpdatePictureAsync(IFormFile file, int userId, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            try
            {
                ApplicationUser user = await userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    throw new Exception($"Usuario con id {userId} no encontrado");
                }

                string folderName = Path.Combine(configuration["ResourcesFolder"].ToString(),
                                                 configuration["ImagesFolder"].ToString(),
                                                 configuration["UsersFolder"].ToString());

                string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                string fileName = user.UserName;

                string fullPath = Path.Combine(pathToSave, fileName);
                string dbPath = Path.Combine(folderName, fileName);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                FileStream stream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(stream);
                await stream.DisposeAsync();

                user.Picture = dbPath;

                var updateResult = await userManager.UpdateAsync(user);
                if (!updateResult.Succeeded) 
                {
                    throw new Exception("Error añadiendo la imagen al usuario");
                }

                return dbPath;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<int> AddShopFavoritesAsync(int userId, int shopId, UserManager<ApplicationUser> userManager, ApiDbContext apiDbContext) 
        { 
            try
            {
                ApplicationUser user = await userManager.FindByIdAsync(userId.ToString());
                if (user == null) 
                {
                    throw new Exception($"Usuario con id {userId} no encontrado");
                }

                Shop shop = await apiDbContext.Shops.FindAsync(shopId);
                if (shop == null)
                {
                    throw new Exception($"Tienda con id {shopId} no encontrada");
                }

                if (apiDbContext.Users_Shops_Favorites.FirstOrDefault(f => f.UserId == userId && f.ShopId == shopId) != null) 
                {
                    throw new Exception($"La tienda {shop.Name} ya se encuentra en el listado de favoritos de {user.UserName}");
                }

                await apiDbContext.Users_Shops_Favorites.AddAsync(new User_Shop_Favorite 
                { 
                    UserId = userId,
                    ShopId =  shopId
                });

                await apiDbContext.SaveChangesAsync();

                return shopId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<int> RateShopsAsync(UserShopRatePostRequestcs model, UserManager<ApplicationUser> userManager, ApiDbContext apiDbContext) 
        {
            try
            {
                ApplicationUser user = await userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                {
                    throw new Exception($"Usuario con id {model.UserId} no encontrado");
                }

                Shop shop = await apiDbContext.Shops.FindAsync(model.ShopId);
                if (shop == null)
                {
                    throw new Exception($"Tienda con id {model.ShopId} no encontrada");
                }

                User_Shop_Rating userRate = await apiDbContext.Users_Shops_Ratings.FindAsync(new { model.UserId, model.ShopId });

                if (userRate == null)
                {
                    await apiDbContext.Users_Shops_Ratings.AddAsync(new User_Shop_Rating
                    {
                        Rate = model.Rate,
                        Date = DateTime.Now,
                        Comment = model.Comment,
                        UserId = model.UserId,
                        ShopId = model.ShopId
                    });
                }
                else 
                {
                    userRate.Rate = model.Rate;
                    userRate.Date = DateTime.Now;
                    userRate.Comment = model.Comment;
                }

                await apiDbContext.SaveChangesAsync();

                return model.ShopId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<int> AddProductFavoritesAsync(int userId, int productId, UserManager<ApplicationUser> userManager, ApiDbContext apiDbContext)
        {
            try
            {
                ApplicationUser user = await userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    throw new Exception($"Usuario con id {userId} no encontrado");
                }

                Product product = await apiDbContext.Products.FindAsync(productId);
                if (product == null)
                {
                    throw new Exception($"Producto con id {productId} no encontrado");
                }

                if (apiDbContext.Users_Products_Favorites.FirstOrDefault(f => f.UserId == userId && f.ProductId == productId) != null)
                {
                    throw new Exception($"El producto {product.Name} ya se encuentra en el listado de favoritos de {user.UserName}");
                }

                await apiDbContext.Users_Products_Favorites.AddAsync(new User_Product_Favorite
                {
                    UserId = userId,
                    ProductId = productId
                });

                await apiDbContext.SaveChangesAsync();

                return productId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<int> RateProductAsync(UserProductRatePostRequestcs model, UserManager<ApplicationUser> userManager, ApiDbContext apiDbContext)
        {
            try
            {
                ApplicationUser user = await userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                {
                    throw new Exception($"Usuario con id {model.UserId} no encontrado");
                }

                Product product = await apiDbContext.Products.FindAsync(model.Productd);
                if (product == null)
                {
                    throw new Exception($"Producto con id {model.Productd} no encontrado");
                }

                User_Product_Rating userRate = await apiDbContext.Users_Products_Ratings.FindAsync(new { model.UserId, model.Productd });

                if (userRate == null)
                {
                    await apiDbContext.Users_Products_Ratings.AddAsync(new User_Product_Rating
                    {
                        Rate = model.Rate,
                        Date = DateTime.Now,
                        Comment = model.Comment,
                        UserId = model.UserId,
                        ProductId = model.Productd
                    });
                }
                else
                {
                    userRate.Rate = model.Rate;
                    userRate.Date = DateTime.Now;
                    userRate.Comment = model.Comment;
                }

                await apiDbContext.SaveChangesAsync();

                return model.Productd;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserDTO> UpdateAsync(UserPutRequest model, UserManager<ApplicationUser> userManager, ClaimsPrincipal currentUser)
        {
            try
            {
                var appUser = await userManager.FindByIdAsync(model.Id.ToString());
                if (appUser == null)
                {
                    throw new Exception($"No existe el usuario con id {model.Id}");
                }

                ApplicationUser user = await userManager.GetUserAsync(currentUser);
                if ((!await userManager.IsInRoleAsync(user, "SuperUser") && await userManager.IsInRoleAsync(appUser, "SuperUser")) ||
                    (!model.IsProfile && !await userManager.IsInRoleAsync(user, "SuperUser") && await userManager.IsInRoleAsync(appUser, "Admin")))
                {
                    throw new Exception($"No tienes permisos para editar el usuario {appUser.UserName}");
                }

                var userFoundEmail = userManager.Users.FirstOrDefault(u => u.Id != model.Id && u.Email.ToUpper().Trim().Equals(model.Email.ToUpper().Trim()));
                if (userFoundEmail != null)
                {
                    throw new Exception($"Ya existe otro usuario con el Email {model.Email}");
                }

                var userFoundName = userManager.Users.FirstOrDefault(u => u.Id != model.Id && u.UserName.ToUpper().Trim().Equals(model.Username.ToUpper().Trim()));
                if (userFoundName != null)
                {
                    throw new Exception($"Ya existe otro usuario {model.Username}");
                }

                if (!string.IsNullOrWhiteSpace(model.OldPassword)) 
                {
                    var passwordResult = await userManager.ChangePasswordAsync(appUser, model.OldPassword, model.Password);
                    if (!passwordResult.Succeeded)
                    {
                        throw new Exception($"ERROR actualizando la contraseña - {passwordResult.Errors}");
                    }
                }
                
                appUser.UserName = model.Username;
                appUser.Name = model.Name;
                appUser.Surname = model.Surname;
                appUser.PhoneNumber = model.PhoneNumber;
                appUser.Email = model.Email;
                appUser.IsActive = model.IsActive;

                var upadateResult = await userManager.UpdateAsync(appUser);

                if (!upadateResult.Succeeded)
                {
                    throw new Exception($"ERROR actualizando los datos de usuario - {upadateResult.Errors}");
                }

                return await ModelToDTOAsync(appUser, userManager);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<int> DeleteAsync(int id, UserManager<ApplicationUser> userManager)
        {
            try
            {
                ApplicationUser user = await userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    throw new Exception($"Usuario con id {id} no encontrado");
                }

                var deletResult = await userManager.DeleteAsync(user);
                if (!deletResult.Succeeded)
                {
                    throw new Exception($"ERROR eliminando el ususario {user.UserName}");
                }

                return user.Id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<int> RemoveShopFavoritesAsync(int userId, int shopId, UserManager<ApplicationUser> userManager, ApiDbContext apiDbContext)
        {
            try
            {
                ApplicationUser user = await userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    throw new Exception($"Usuario con id {userId} no encontrado");
                }

                Shop shop = await apiDbContext.Shops.FindAsync(shopId);
                if (shop == null)
                {
                    throw new Exception($"Tienda con id {shopId} no encontrada");
                }

                User_Shop_Favorite userSopFavorite = apiDbContext.Users_Shops_Favorites.FirstOrDefault(f => f.UserId == userId && f.ShopId == shopId);
                if (userSopFavorite == null)
                {
                    throw new Exception($"La tienda {shop.Name} no se encuentra en el listado de favoritos de {user.UserName}");
                }

                apiDbContext.Users_Shops_Favorites.Remove(userSopFavorite);

                await apiDbContext.SaveChangesAsync();

                return shopId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #region Support Methods

        private async Task<UserDTO> ModelToDTOAsync(ApplicationUser user, UserManager<ApplicationUser> userManager) 
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
