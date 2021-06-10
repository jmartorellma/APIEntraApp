using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using APIEntraApp.Data;
using APIEntraApp.Services.Shops.Models.DTOs;
using APIEntraApp.Services.Shops.Models.Request;
using Microsoft.AspNetCore.Identity;
using APIEntraApp.Data.Identity;
using APIEntraApp.Services.Users.Models.DTOs;

namespace APIEntraApp.Services.Shops.Core
{
    public interface IShopService
    {
        Task<List<ShopDTO>> GetAllAsync(ApiDbContext apiDbContext);
        Task<ShopDTO> GetByIdAsync(int id, ApiDbContext apiDbContext);
        Task<ShopDTO> GetByOwnerIdAsync(int ownerId, ApiDbContext apiDbContext);
        Task<List<UserDTO>> GetLockedAsync(int shopId, ApiDbContext apiDbContext, UserManager<ApplicationUser> userManager);
        Task<ShopDTO> CreateAsync(ShopPostRequest model, ApiDbContext apiDbContext);
        Task<PictureDTO> UpdatePictureAsync(IFormFile file, int shopId, IConfiguration configuration, ApiDbContext apiDbContext);
        Task<string> AddPaymentethodAsync(ShopPaymentMethodPostRequest model, ApiDbContext apiDbContext);
        Task<string> AddAllowedPurchaseTypeAsync(ShopPurchaseTypePostRequest model, ApiDbContext apiDbContext);
        Task<int> AddUserLockedAsync(int shopId, int userId, ApiDbContext apiDbContext);
        Task<ShopDTO> UpdateAsync(ShopPutRequest model, ApiDbContext apiDbContext);
        Task<int> DeleteAsync(int id, ApiDbContext apiDbContext);
        Task<string> RemovePaymentMethodAsync(ShopPaymentMethodDeleteRequest model, ApiDbContext apiDbContext);
        Task<string> RemovePurchaseTypeAsync(ShopPurchaseTypeDeleteRequest model, ApiDbContext apiDbContext);
        Task<int> RemoveUserLockedAsync(int shopId, int userId, ApiDbContext apiDbContext);
    }
}
