using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using APIEntraApp.Data;
using APIEntraApp.Data.Models;
using APIEntraApp.Services.Providers.Core;
using APIEntraApp.Services.Providers.Models.DTOs;
using APIEntraApp.Services.Providers.Models.Request;

namespace APIEntraApp.Services.Providers
{
    public class ProviderService : IProviderService
    {
        public async Task<List<ProviderDTO>> GetAllAsync(ApiDbContext apiDbContext)
        {
            try
            {
                return await Task.Run(() =>
                {
                    List<ProviderDTO> result = new List<ProviderDTO>();

                    apiDbContext.Providers.ToList().ForEach(p =>
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

        public async Task<ProviderDTO> GetByIdAsync(int id, ApiDbContext apiDbContext)
        {
            try
            {
                Provider provider = await apiDbContext.Providers.FindAsync(id);
                if (provider == null)
                {
                    throw new Exception($"Proveedor con id {id} no encontrado");
                }

                return ModelToDTO(provider);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ProviderDTO> CreateAsync(ProviderPostRequest model, ApiDbContext apiDbContext)
        {
            try
            {
                var provider = apiDbContext.Providers.FirstOrDefault(p => p.Code.ToUpper().Trim().Equals(model.Code.ToUpper().Trim()));
                if (provider != null)
                {
                    throw new Exception($"Ya existe un proveedor con el código {model.Code}");
                }

                Provider newProvider = new Provider
                {
                    Code = model.Code,
                    Name = model.Name,
                    Web = model.Web,
                    CreationDate = DateTime.Now
                };

                await apiDbContext.Providers.AddAsync(newProvider);

                await apiDbContext.SaveChangesAsync();

                return ModelToDTO(newProvider);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ProviderDTO> UpdateAsync(ProviderPutRequest model, ApiDbContext apiDbContext)
        {
            try
            {
                var provider = await apiDbContext.Providers.FindAsync(model.Id);
                if (provider == null)
                {
                    throw new Exception($"No existe el proveedor {model.Name} con id {model.Id}");
                }

                var providerFound = apiDbContext.Providers.FirstOrDefault(p => p.Id != model.Id && p.Code.ToUpper().Trim().Equals(model.Code.ToUpper().Trim()));
                if (providerFound != null)
                {
                    throw new Exception($"Ya existe un proveedor con el código {model.Code}");
                }

                provider.Code = model.Code;
                provider.Name = model.Name;
                provider.Web = model.Web;

                await apiDbContext.SaveChangesAsync();

                return ModelToDTO(provider);
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
                var provider = await apiDbContext.Providers.FindAsync(id);
                if (provider == null)
                {
                    throw new Exception($"Proveedor con id {id} no encontrado");
                }

                apiDbContext.Providers.Remove(provider);
                await apiDbContext.SaveChangesAsync();

                return provider.Id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #region Support Methods

        private ProviderDTO ModelToDTO(Provider provider)
        {
            return new ProviderDTO
            {
                Id = provider.Id,
                Code = provider.Code,
                Name = provider.Name,
                Web = provider.Web,
                CreationDate = provider.CreationDate
            };
        }

        #endregion Support Methods
    }
}
