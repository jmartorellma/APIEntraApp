using System.Threading.Tasks;
using System.Collections.Generic;
using APIEntraApp.Data;
using APIEntraApp.Services.PaymentStatuses.Models.DTOs;
using APIEntraApp.Services.PaymentStatuses.Models.Request;

namespace APIEntraApp.Services.PaymentStatuses.Core
{
    public interface IPaymentStatusService
    {
        Task<List<PaymentStatusDTO>> GetAllAsync(ApiDbContext apiDbContext);
        Task<PaymentStatusDTO> GetByIdAsync(int id, ApiDbContext apiDbContext);
        Task<PaymentStatusDTO> CreateAsync(PaymentStatusPostRequest model, ApiDbContext apiDbContext);
        Task<PaymentStatusDTO> UpdateAsync(PaymentStatusPutRequest model, ApiDbContext apiDbContext);
        Task<int> DeleteAsync(int id, ApiDbContext apiDbContext);
    }
}
