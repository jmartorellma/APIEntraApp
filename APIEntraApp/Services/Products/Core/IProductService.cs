using System.Threading.Tasks;
using System.Collections.Generic;
using APIEntraApp.Data;
using APIEntraApp.Services.Products.Models.DTOs;
using APIEntraApp.Services.Products.Models.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace APIEntraApp.Services.Products.Core
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllAsync(ApiDbContext apiDbContext);
        Task<ProductDTO> GetByIdAsync(int id, ApiDbContext apiDbContext);
        Task<List<ProductDTO>> GetByShopIdAsync(int shopId, ApiDbContext apiDbContext);
        Task<ProductDTO> CreateAsync(ProductPostRequest model, ApiDbContext apiDbContext);
        Task<string> UpdatePictureAsync(IFormFile file, int productID, IConfiguration configuration, ApiDbContext apiDbContext);
        Task<ProductDTO> UpdateAsync(ProductPutRequest model, ApiDbContext apiDbContext);
        Task<int> DeleteAsync(int id, ApiDbContext apiDbContext);
    }
}
