using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using APIEntraApp.Data.Identity;
using APIEntraApp.Services.Users.Models.DTOs;
using APIEntraApp.Services.Users.Models.Request;

namespace APIEntraApp.Services.Users.Core
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllAsync(UserManager<ApplicationUser> userManager);
        Task<UserDTO> GetByIdAsync(int id, UserManager<ApplicationUser> userManager);
        Task<UserDTO> CreateAsync(UserPostRequest model, UserManager<ApplicationUser> userManager);
        Task<UserDTO> UpdateAsync(UserPutRequest model, UserManager<ApplicationUser> userManager);
        Task<int> DeleteAsync(int id, UserManager<ApplicationUser> userManager);
    }
}
