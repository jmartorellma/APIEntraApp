using System.Threading.Tasks;
using System.Collections.Generic;
using APIEntraApp.Data;
using APIEntraApp.Services.Deliveries.Models.DTOs;
using APIEntraApp.Services.Deliveries.Models.Request;

namespace APIEntraApp.Services.Deliveries.Core
{
    public interface IDeliveryService
    {
        Task<DeliveryDTO> GetByIdAsync(int id, ApiDbContext apiDbContext);
        Task<List<DeliveryDTO>> GetByUserIdAsync(int id, ApiDbContext apiDbContext);
        Task<List<DeliveryDTO>> GetByShopIdAsync(int id, ApiDbContext apiDbContext);
        Task<DeliveryDTO> UpdateAsync(DeliveryPutRequest model, ApiDbContext apiDbContext);
        Task<DeliveryDTO> CompleteAsync(DeliveryPutCompleteRequest model, ApiDbContext apiDbContext);
    }
}
