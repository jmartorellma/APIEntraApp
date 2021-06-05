using System.Threading.Tasks;
using APIEntraApp.Data;
using APIEntraApp.Services.Carts.Models.DTOs;
using APIEntraApp.Services.Carts.Models.Request;

namespace APIEntraApp.Services.Carts.Core
{
    public interface ICartService
    {
        Task<CartDTO> GetByUserIdAsync(int userId, ApiDbContext apiDbContext);
        Task<CartDTO> AddAsync(ProductCartPostRequest model, ApiDbContext apiDbContext);
        Task<CartDTO> UpdateAsync(ProductCartPutRequest model, ApiDbContext apiDbContext);
        Task<int> RemoveAsync(int id, ApiDbContext apiDbContext);
    }
}
