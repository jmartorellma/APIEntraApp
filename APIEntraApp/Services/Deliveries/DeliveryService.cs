using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using APIEntraApp.Data;
using APIEntraApp.Data.Models;
using APIEntraApp.Services.Deliveries.Core;
using APIEntraApp.Services.Deliveries.Models.DTOs;
using APIEntraApp.Services.Deliveries.Models.Request;

namespace APIEntraApp.Services.Deliveries
{
    public class DeliveryService : IDeliveryService
    {
        public async Task<DeliveryDTO> GetByIdAsync(int id, ApiDbContext apiDbContext)
        {
            try
            {
                Delivery delivery = await apiDbContext.Deliveries.FindAsync(id);
                if (delivery == null)
                {
                    throw new Exception($"Entrega con id {id} no encontrada");
                }

                return ModelToDTO(delivery);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<DeliveryDTO>> GetByUserIdAsync(int id, ApiDbContext apiDbContext)
        {
            try
            {
                return await Task.Run(() => apiDbContext.Deliveries.Where(d => d.Purchase.Purchase_Carts.FirstOrDefault(pc => pc.UserProductCart.UserId == id) != null).ToList()
                                                                   .Select(s => ModelToDTO(s)).ToList());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<DeliveryDTO>> GetByShopIdAsync(int id, ApiDbContext apiDbContext)
        {
            try
            {
                return await Task.Run(() => apiDbContext.Deliveries.Where(d => d.Purchase.Purchase_Carts.FirstOrDefault(pc => pc.UserProductCart.Product.ShopId == id) != null).ToList()
                                                                   .Select(s => ModelToDTO(s)).ToList());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        public async Task<DeliveryDTO> UpdateAsync(DeliveryPutRequest model, ApiDbContext apiDbContext)
        {
            try
            {
                var delivery = await apiDbContext.Deliveries.FindAsync(model.Id);
                if (delivery == null)
                {
                    throw new Exception($"No existe la entrega con id {model.Id}");
                }

                if (delivery.IsCompleted) 
                {
                    throw new Exception($"No se puede modificar la entrega. Ya ha sido completada.");
                }

                delivery.Address = model.Address;
                delivery.Number = model.Number;
                delivery.City = model.City;
                delivery.PostCode = model.PostCode;
                delivery.Region = model.Region;

                await apiDbContext.SaveChangesAsync();

                return ModelToDTO(delivery);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<DeliveryDTO> CompleteAsync(DeliveryPutCompleteRequest model, ApiDbContext apiDbContext)
        {
            try
            {
                var delivery = await apiDbContext.Deliveries.FindAsync(model.Id);
                if (delivery == null)
                {
                    throw new Exception($"No existe la entrega con id {model.Id}");
                }

                if (delivery.IsCompleted)
                {
                    throw new Exception($"No se puede modificar la entrega. Ya ha sido completada.");
                }

                delivery.IsCompleted = true;
                
                await apiDbContext.SaveChangesAsync();

                return ModelToDTO(delivery);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #region Support Methods

        private DeliveryDTO ModelToDTO(Delivery delivery)
        {
            return new DeliveryDTO
            {
                Id = delivery.Id,
                DeliveryDate = delivery.DeliveryDate,
                DeliveryTaxes = delivery.DeliveryTaxes.HasValue ? delivery.DeliveryTaxes.Value : 0,
                Address = delivery.Number,
                Number = delivery.Number,
                City = delivery.City,
                PostCode = delivery.PostCode,
                Region = delivery.Region,
                CreationDate = delivery.CreationDate
            };
        }

        #endregion Support Methods
    }
}
