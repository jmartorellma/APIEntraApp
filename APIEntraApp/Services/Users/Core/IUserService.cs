using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using APIEntraApp.Data;
using APIEntraApp.Data.Identity;
using APIEntraApp.Services.Users.Models.DTOs;
using APIEntraApp.Services.Users.Models.Request;

namespace APIEntraApp.Services.Users.Core
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllAsync(UserManager<ApplicationUser> userManager);
        Task<UserDTO> GetByIdAsync(int id, UserManager<ApplicationUser> userManager);
        Task<UserDTO> CreateAsync(UserPostRequest model, UserManager<ApplicationUser> userManager);
        Task<string> UpdatePictureAsync(IFormFile file, int userId, IConfiguration configuration, UserManager<ApplicationUser> userManager);
        Task<int> AddShopFavoritesAsync(int userId, int shopId, UserManager<ApplicationUser> userManager, ApiDbContext apiDbContext);
        Task<int> RateShopsAsync(UserShopRatePostRequestcs model, UserManager<ApplicationUser> userManager, ApiDbContext apiDbContext);
        Task<int> AddProductFavoritesAsync(int userId, int productId, UserManager<ApplicationUser> userManager, ApiDbContext apiDbContext);
        Task<int> RateProductAsync(UserProductRatePostRequestcs model, UserManager<ApplicationUser> userManager, ApiDbContext apiDbContext);
        Task<UserDTO> UpdateAsync(UserPutRequest model, UserManager<ApplicationUser> userManager);
        Task<int> DeleteAsync(int id, UserManager<ApplicationUser> userManager);
        Task<int> RemoveShopFavoritesAsync(int userId, int shopId, UserManager<ApplicationUser> userManager, ApiDbContext apiDbContext);
    }
}
