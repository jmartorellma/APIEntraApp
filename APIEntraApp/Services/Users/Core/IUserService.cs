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
        Task<List<UserDTO>> GetAll(UserManager<ApplicationUser> userManager);
        Task<UserDTO> GetById(int id, UserManager<ApplicationUser> userManager);
        Task<UserDTO> Create(UserPostRequest model, UserManager<ApplicationUser> userManager);
        Task<UserDTO> Update(UserPutRequest model, UserManager<ApplicationUser> userManager);
        Task<UserDTO> Delete(int id, UserManager<ApplicationUser> userManager);
    }
}
