using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using APIEntraApp.Data;
using APIEntraApp.Data.Models;
using APIEntraApp.Services.PurchaseTypes.Core;
using APIEntraApp.Services.PurchaseTypes.Models.DTOs;
using APIEntraApp.Services.PurchaseTypes.Models.Request;

namespace APIEntraApp.Services.PurchaseTypes
{
    public class PurchaseTypeService : IPurchaseTyeService
    {
        public async Task<List<PurchaseTypeDTO>> GetAllAsync(ApiDbContext apiDbContext)
        {
            try
            {
                return await Task.Run(() => apiDbContext.PurchaseTypes.Select(s => ModelToDTO(s)).ToList());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PurchaseTypeDTO> GetByIdAsync(int id, ApiDbContext apiDbContext)
        {
            try
            {
                PurchaseType purchaseType = await apiDbContext.PurchaseTypes.FindAsync(id);
                if (purchaseType == null)
                {
                    throw new Exception($"Tipo de pedido con id {id} no encontrado");
                }

                return ModelToDTO(purchaseType);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PurchaseTypeDTO> CreateAsync(PurchaseTypePostRequest model, ApiDbContext apiDbContext)
        {
            try
            {
                var purchaseType = apiDbContext.PurchaseTypes.FirstOrDefault(p => p.Code.ToUpper().Trim().Equals(model.Code.ToUpper().Trim()));
                if (purchaseType != null)
                {
                    throw new Exception($"Ya existe un tipo de pedido con el código {model.Code}");
                }

                PurchaseType newPurchaseType = new PurchaseType
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreationDate = DateTime.Now
                };

                await apiDbContext.PurchaseTypes.AddAsync(newPurchaseType);

                await apiDbContext.SaveChangesAsync();

                return ModelToDTO(newPurchaseType);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PurchaseTypeDTO> UpdateAsync(PurchaseTypePutRequest model, ApiDbContext apiDbContext)
        {
            try
            {
                var purchaseType = await apiDbContext.PurchaseTypes.FindAsync(model.Id);
                if (purchaseType == null)
                {
                    throw new Exception($"No existe el tipo de pedido {model.Name} con id {model.Id}");
                }

                var purchaseTypeFound = apiDbContext.PurchaseTypes.FirstOrDefault(p => p.Id != model.Id && p.Code.ToUpper().Trim().Equals(model.Code.ToUpper().Trim()));
                if (purchaseTypeFound != null)
                {
                    throw new Exception($"Ya existe un el tipo de pedido con el código {model.Code}");
                }

                purchaseTypeFound.Code = model.Code;
                purchaseTypeFound.Name = model.Name;

                await apiDbContext.SaveChangesAsync();

                return ModelToDTO(purchaseTypeFound);
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
                var purchaseType = await apiDbContext.PurchaseTypes.FindAsync(id);
                if (purchaseType == null)
                {
                    throw new Exception($"Tipo de pedido con id {id} no encontrado");
                }

                apiDbContext.PurchaseTypes.Remove(purchaseType);
                await apiDbContext.SaveChangesAsync();

                return purchaseType.Id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #region Support Methods

        private PurchaseTypeDTO ModelToDTO(PurchaseType PurchaseTypeDTO)
        {
            return new PurchaseTypeDTO
            {
                Id = PurchaseTypeDTO.Id,
                Code = PurchaseTypeDTO.Code,
                Name = PurchaseTypeDTO.Name,
                CreationDate = PurchaseTypeDTO.CreationDate
            };
        }

        #endregion Support Methods

    }
}
