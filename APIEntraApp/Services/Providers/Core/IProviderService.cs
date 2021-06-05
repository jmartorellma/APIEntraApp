using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using APIEntraApp.Data;
using APIEntraApp.Services.Providers.Models.DTOs;
using APIEntraApp.Services.Providers.Models.Request;

namespace APIEntraApp.Services.Providers.Core
{
    public interface IProviderService
    {
        Task<List<ProviderDTO>> GetAllAsync(ApiDbContext apiDbContext);
        Task<ProviderDTO> GetByIdAsync(int id, ApiDbContext apiDbContext);
        Task<ProviderDTO> CreateAsync(ProviderPostRequest model, ApiDbContext apiDbContext);
        Task<ProviderDTO> UpdateAsync(ProviderPutRequest model, ApiDbContext apiDbContext);
        Task<string> UpdatePictureAsync(IFormFile file, int providerId, IConfiguration configuration, ApiDbContext apiDbContext);
        Task<int> DeleteAsync(int id, ApiDbContext apiDbContext);
    }
}
