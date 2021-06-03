using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using APIEntraApp.DTOs;
using APIEntraApp.Data.Identity;
using APIEntraApp.Services.Interfaces;

namespace APIEntraApp.Services
{
    public class UserService : IUserService
    {
        public async Task<List<UserDTO>> GetAllUsers(UserManager<ApplicationUser> userManager)
        {
            try
            {
                return await Task.Run(() =>
                {
                    return userManager.Users.Select(u => new UserDTO
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Surname = u.Surname,
                        IsActive = u.IsActive,
                        UserName = u.UserName,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                        CreationDate = u.CreationDate
                    }).ToList();
                });                
            }
            catch(Exception e) 
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserDTO> GetUserById(int id, UserManager<ApplicationUser> userManager)
        {
            try
            {
                ApplicationUser user = await userManager.FindByIdAsync(id.ToString());
                if (user == null) 
                {
                    throw new Exception($"Usuario con id {id} no encontrado");
                }

                return new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    IsActive = user.IsActive,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    CreationDate = user.CreationDate
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
