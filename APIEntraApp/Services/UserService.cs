using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using APIEntraApp.DTOs;
using APIEntraApp.Data.Identity;
using APIEntraApp.Services.Interfaces;
using System.Security.Claims;

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
    }
}
