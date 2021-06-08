using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using APIEntraApp.Data;
using APIEntraApp.Data.Models;
using APIEntraApp.Services.PaymentStatuses.Core;
using APIEntraApp.Services.PaymentStatuses.Models.DTOs;
using APIEntraApp.Services.PaymentStatuses.Models.Request;

namespace APIEntraApp.Services.PaymentStatuses
{
    public class PaymentStatusService : IPaymentStatusService
    {
        public async Task<List<PaymentStatusDTO>> GetAllAsync(ApiDbContext apiDbContext)
        {
            try
            {
                return await Task.Run(() => 
                {
                    List<PaymentStatusDTO> result = new List<PaymentStatusDTO>();

                    apiDbContext.PaymentStatus.ToList().ForEach(p => 
                    {
                        result.Add(ModelToDTO(p));
                    });

                    return result;
                });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PaymentStatusDTO> GetByIdAsync(int id, ApiDbContext apiDbContext)
        {
            try
            {
                PaymentStatus paymentStatus = await apiDbContext.PaymentStatus.FindAsync(id);
                if (paymentStatus == null)
                {
                    throw new Exception($"Estado de pago con id {id} no encontrado");
                }

                return ModelToDTO(paymentStatus);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PaymentStatusDTO> CreateAsync(PaymentStatusPostRequest model, ApiDbContext apiDbContext)
        {
            try
            {
                var paymentStatusFound = apiDbContext.PaymentStatus.FirstOrDefault(p => p.Code.ToUpper().Trim().Equals(model.Code.ToUpper().Trim()));
                if (paymentStatusFound != null)
                {
                    throw new Exception($"Ya existe un estado de pago con el código {model.Code}");
                }

                PaymentStatus newPaymentStatus = new PaymentStatus
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreationDate = DateTime.Now
                };

                await apiDbContext.PaymentStatus.AddAsync(newPaymentStatus);

                await apiDbContext.SaveChangesAsync();

                return ModelToDTO(newPaymentStatus);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PaymentStatusDTO> UpdateAsync(PaymentStatusPutRequest model, ApiDbContext apiDbContext)
        {
            try
            {
                var paymentStatus = await apiDbContext.PaymentStatus.FindAsync(model.Id);
                if (paymentStatus == null)
                {
                    throw new Exception($"No existe el estado de pago {model.Name} con id {model.Id}");
                }

                var paymentStatusFound = apiDbContext.PaymentStatus.FirstOrDefault(p => p.Id != model.Id && p.Code.ToUpper().Trim().Equals(model.Code.ToUpper().Trim()));
                if (paymentStatusFound != null)
                {
                    throw new Exception($"Ya existe un estado de pago con el código {model.Code}");
                }

                paymentStatus.Code = model.Code;
                paymentStatus.Name = model.Name;

                await apiDbContext.SaveChangesAsync();

                return ModelToDTO(paymentStatus);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<int> DeleteAsync(int id, ApiDbContext apiDbContext)
        {
            try
            {
                var paymentStatus = await apiDbContext.PaymentStatus.FindAsync(id);
                if (paymentStatus == null)
                {
                    throw new Exception($"Estado de pago con id {id} no encontrado");
                }

                apiDbContext.PaymentStatus.Remove(paymentStatus);
                await apiDbContext.SaveChangesAsync();

                return paymentStatus.Id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #region Support Methods

        private PaymentStatusDTO ModelToDTO(PaymentStatus paymentStatus)
        {
            return new PaymentStatusDTO
            {
                Id = paymentStatus.Id,
                Code = paymentStatus.Code,
                Name = paymentStatus.Name,
                CreationDate = paymentStatus.CreationDate
            };
        }

        #endregion Support Methods

    }
}
