using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using APIEntraApp.Data;
using APIEntraApp.Services.Purchases.Models.DTOs;
using APIEntraApp.Services.Purchases.Models.Request;

namespace APIEntraApp.Services.Purchases.Core
{
    public interface IPurchaseService
    {
        Task<PurchaseDTO> GetByIdAsync(int id, ApiDbContext apiDbContext);
        Task<List<PurchaseDTO>> GetByUserIdAsync(int userId, ApiDbContext apiDbContext);
        Task<List<PurchaseDTO>> GetByShopIdAsync(int shopId, ApiDbContext apiDbContext);
        Task<PurchaseDTO> CreateAsync(PurchasePostRequest model, ApiDbContext apiDbContext, IConfiguration configuration);
        Task<PurchaseDTO> UpdateAsync(PurchasePutRequest model, ApiDbContext apiDbContext, IConfiguration configuration);
        Task<PurchaseDTO> CompleteAsync(PurchasePutRequest model, ApiDbContext apiDbContext, IConfiguration configuration);
        Task<int> DeleteAsync(int id, ApiDbContext apiDbContext, IConfiguration configuration);
    }
}
