using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using APIEntraApp.Data;
using APIEntraApp.Data.Models;
using APIEntraApp.Services.PaymentMethods.Core;
using APIEntraApp.Services.PaymentMethods.Models.DTOs;
using APIEntraApp.Services.PaymentMethods.Models.Request;

namespace APIEntraApp.Services.PaymentMethods
{
    public class PaymentMethodService : IPaymentMethodService
    {
        public async Task<List<PaymentMethodDTO>> GetAllAsync(ApiDbContext apiDbContext)
        {
            try
            {
                return await Task.Run(() => apiDbContext.PaymentMethods.Select(s => ModelToDTO(s)).ToList());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PaymentMethodDTO> GetByIdAsync(int id, ApiDbContext apiDbContext)
        {
            try
            {
                PaymentMethod paymentMethod = await apiDbContext.PaymentMethods.FindAsync(id);
                if (paymentMethod == null)
                {
                    throw new Exception($"Método de pago con id {id} no encontrado");
                }

                return ModelToDTO(paymentMethod);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PaymentMethodDTO> CreateAsync(PaymentMethodPostRequest model, ApiDbContext apiDbContext)
        {
            try
            {
                var productFound = apiDbContext.PaymentMethods.FirstOrDefault(p => p.Code.ToUpper().Trim().Equals(model.Code.ToUpper().Trim()));
                if (productFound != null)
                {
                    throw new Exception($"Ya existe un método de pago con el código {model.Code}");
                }

                PaymentMethod newPaymentMethod = new PaymentMethod
                {
                    Code = model.Code,
                    Name = model.Name,
                    Value = model.Value,
                    CreationDate = DateTime.Now
                };

                await apiDbContext.PaymentMethods.AddAsync(newPaymentMethod);

                await apiDbContext.SaveChangesAsync();

                PaymentMethod createdPaymentMethod = apiDbContext.PaymentMethods.First(p => p.Code.ToUpper().Trim().Equals(model.Code.ToUpper().Trim()));

                return ModelToDTO(createdPaymentMethod);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PaymentMethodDTO> UpdateAsync(PaymentMethodPutRequest model, ApiDbContext apiDbContext)
        {
            try
            {
                var paymentMethod = await apiDbContext.PaymentMethods.FindAsync(model.Id);
                if (paymentMethod == null)
                {
                    throw new Exception($"No existe el método de pago {model.Name} con id {model.Id}");
                }

                var paymentMethodFound = apiDbContext.PaymentMethods.FirstOrDefault(p => p.Id != model.Id && p.Code.ToUpper().Trim().Equals(model.Code.ToUpper().Trim()));
                if (paymentMethodFound != null)
                {
                    throw new Exception($"Ya existe un método de pago con el código {model.Code}");
                }

                paymentMethod.Code = model.Code;
                paymentMethod.Name = model.Name;
                paymentMethod.Value = model.Value;

                await apiDbContext.SaveChangesAsync();

                return ModelToDTO(paymentMethod);
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
                var paymentMethod = await apiDbContext.PaymentMethods.FindAsync(id);
                if (paymentMethod == null)
                {
                    throw new Exception($"Método de pago con id {id} no encontrado");
                }

                apiDbContext.PaymentMethods.Remove(paymentMethod);
                await apiDbContext.SaveChangesAsync();

                return paymentMethod.Id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #region Support Methods

        private PaymentMethodDTO ModelToDTO(PaymentMethod paymentMethod)
        {
            return new PaymentMethodDTO
            {
                Id = paymentMethod.Id,
                Code = paymentMethod.Code,
                Name = paymentMethod.Name,
                Value = paymentMethod.Value,
                CreationDate = paymentMethod.CreationDate
            };
        }

        #endregion Support Methods
    }
}
