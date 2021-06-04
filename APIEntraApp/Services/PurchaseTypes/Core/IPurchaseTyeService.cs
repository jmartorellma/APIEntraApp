using System.Threading.Tasks;
using System.Collections.Generic;
using APIEntraApp.Data;
using APIEntraApp.Services.PurchaseTypes.Models.DTOs;
using APIEntraApp.Services.PurchaseTypes.Models.Request;

namespace APIEntraApp.Services.PurchaseTypes.Core
{
    public interface IPurchaseTyeService
    {
        Task<List<PurchaseTypeDTO>> GetAllAsync(ApiDbContext apiDbContext);
        Task<PurchaseTypeDTO> GetByIdAsync(int id, ApiDbContext apiDbContext);
        Task<PurchaseTypeDTO> CreateAsync(PurchaseTypePostRequest model, ApiDbContext apiDbContext);
        Task<PurchaseTypeDTO> UpdateAsync(PurchaseTypePutRequest model, ApiDbContext apiDbContext);
        Task<int> DeleteAsync(int id, ApiDbContext apiDbContext);
    }
}
