using System.Threading.Tasks;
using System.Collections.Generic;
using APIEntraApp.Data;
using APIEntraApp.Services.Purchases.Models.DTOs;

namespace APIEntraApp.Services.Purchases.Core
{
    public interface IPurchaseService
    {
        Task<PurchaseDTO> GetByIdAsync(int id, ApiDbContext apiDbContext);
        Task<List<PurchaseDTO>> GetByUserIdAsync(int userId, ApiDbContext apiDbContext);
        Task<List<PurchaseDTO>> GetByShopIdAsync(int shopId, ApiDbContext apiDbContext);
    }
}
