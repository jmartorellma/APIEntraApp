using APIEntraApp.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;
using APIEntraApp.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace APIEntraApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllUsers(UserManager<ApplicationUser> userManager);
        Task<UserDTO> GetUserById(int id, UserManager<ApplicationUser> userManager);
    }
}
