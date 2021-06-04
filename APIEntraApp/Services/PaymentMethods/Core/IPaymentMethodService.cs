
using System.Threading.Tasks;
using System.Collections.Generic;
using APIEntraApp.Data;
using APIEntraApp.Services.PaymentMethods.Models.DTOs;
using APIEntraApp.Services.PaymentMethods.Models.Request;

namespace APIEntraApp.Services.PaymentMethods.Core
{
    public interface IPaymentMethodService
    {
        Task<List<PaymentMethodDTO>> GetAllAsync(ApiDbContext apiDbContext);
        Task<PaymentMethodDTO> GetByIdAsync(int id, ApiDbContext apiDbContext);
        Task<PaymentMethodDTO> CreateAsync(PaymentMethodPostRequest model, ApiDbContext apiDbContext);
        Task<PaymentMethodDTO> UpdateAsync(PaymentMethodPutRequest model, ApiDbContext apiDbContext);
        Task<int> DeleteAsync(int id, ApiDbContext apiDbContext);
    }
}
