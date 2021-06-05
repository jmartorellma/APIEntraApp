using System.Threading.Tasks;
using System.Collections.Generic;
using APIEntraApp.Data;
using APIEntraApp.Services.Categories.Models.DTOs;
using APIEntraApp.Services.Categories.Models.Request;


namespace APIEntraApp.Services.Categories.Core
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllAsync(ApiDbContext apiDbContext);
        Task<CategoryDTO> GetByIdAsync(int id, ApiDbContext apiDbContext);
        Task<CategoryDTO> CreateAsync(CategoryPostRequest model, ApiDbContext apiDbContext);
        Task<CategoryDTO> UpdateAsync(CategoryPutRequest model, ApiDbContext apiDbContext);
        Task<int> DeleteAsync(int id, ApiDbContext apiDbContext);
    }
}
