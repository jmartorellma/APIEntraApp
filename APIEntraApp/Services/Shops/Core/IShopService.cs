using System.Threading.Tasks;
using System.Collections.Generic;
using APIEntraApp.Data;
using APIEntraApp.Services.Shops.Models.DTOs;
using APIEntraApp.Services.Shops.Models.Request;

namespace APIEntraApp.Services.Shops.Core
{
    public interface IShopService
    {
        Task<List<ShopDTO>> GetAllAsync(ApiDbContext apiDbContext);
        Task<ShopDTO> GetByIdAsync(int id, ApiDbContext apiDbContext);
        Task<ShopDTO> CreateAsync(ShopPostRequest model, ApiDbContext apiDbContext);
        Task<ShopDTO> UpdateAsync(ShopPutRequest model, ApiDbContext apiDbContext);
        Task<int> DeleteAsync(int id, ApiDbContext apiDbContext);
    }
}
