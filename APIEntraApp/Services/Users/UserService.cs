using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using APIEntraApp.Data.Identity;
using APIEntraApp.Services.Users.Core;
using APIEntraApp.Services.Users.Models.DTOs;
using APIEntraApp.Services.Users.Models.Request;

namespace APIEntraApp.Services.Users
{
    public class UserService : IUserService
    {
        public async Task<List<UserDTO>> GetAll(UserManager<ApplicationUser> userManager)
        {
            try
            {
                List<UserDTO> users = new List<UserDTO>();

                List<ApplicationUser> userLIst = userManager.Users.ToList();

                foreach (ApplicationUser appUser in userLIst) 
                {
                    UserDTO user = await ModelToDTO(appUser, userManager);
                    if (user != null) 
                    {
                        users.Add(user);
                    }
                }

                return users;
            }
            catch(Exception e) 
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserDTO> GetById(int id, UserManager<ApplicationUser> userManager)
        {
            try
            {
                ApplicationUser user = await userManager.FindByIdAsync(id.ToString());
                if (user == null) 
                {
                    throw new Exception($"Usuario con id {id} no encontrado");
                }

                IList<string> roleList = await userManager.GetRolesAsync(user);
                if (roleList == null || !roleList.Any()) 
                {
                    throw new Exception($"Rol del usuario {user.UserName} no encontrado");
                }

                return await ModelToDTO(user, userManager);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserDTO> Create(UserPostRequest model, UserManager<ApplicationUser> userManager)
        {
            try
            {
                var userFound = await userManager.FindByEmailAsync(model.Email);
                if (userFound != null)
                {
                    throw new Exception($"Ya existe un usuario en el sisitema con el Email {model.Email}");
                }

                userFound = await userManager.FindByNameAsync(model.Username);
                if (userFound != null)
                {
                    throw new Exception($"Ya existe un usuario en el sisitema con el Usuario {model.Username}");
                }

                ApplicationUser applicationUser = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Name = model.Name,
                    Surname = model.Surname,
                    CreationDate = DateTime.Now,
                    IsActive = true
                };

                var createResult = await userManager.CreateAsync(applicationUser, model.Password);

                if (!createResult.Succeeded)
                {
                    throw new Exception($"ERROR dando de alta el usuario - {createResult.Errors}");
                }

                var user = await userManager.FindByEmailAsync(applicationUser.Email);
                var roleresult = await userManager.AddToRoleAsync(user, model.Role);

                if (!roleresult.Succeeded)
                {
                    await userManager.DeleteAsync(user);
                    throw new Exception($"ERROR asignando el rol de usuario {model.Role} - {roleresult.Errors}");
                }

                return new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    IsActive = user.IsActive,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = model.Role,
                    PhoneNumber = user.PhoneNumber,
                    CreationDate = user.CreationDate
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserDTO> Update(UserPutRequest model, UserManager<ApplicationUser> userManager)
        {
            try
            {
                var appUser = await userManager.FindByIdAsync(model.Id.ToString());
                if (appUser == null)
                {
                    throw new Exception($"No existe el usuario con id {model.Id}");
                }

                var userFoundEmail = userManager.Users.FirstOrDefault(u => u.Id != model.Id && u.Email.ToUpper().Trim().Equals(model.Email.ToUpper().Trim()));
                if (userFoundEmail != null)
                {
                    throw new Exception($"Ya existe otro usuario con el Email {model.Email}");
                }

                var userFoundName = userManager.Users.FirstOrDefault(u => u.Id != model.Id && u.UserName.ToUpper().Trim().Equals(model.Username.ToUpper().Trim()));
                if (userFoundName != null)
                {
                    throw new Exception($"Ya existe otro usuario {model.Username}");
                }

                if (!string.IsNullOrWhiteSpace(model.OldPassword)) 
                {
                    var passwordResult = await userManager.ChangePasswordAsync(appUser, model.OldPassword, model.Password);
                    if (!passwordResult.Succeeded)
                    {
                        throw new Exception($"ERROR actualizando la contraseña - {passwordResult.Errors}");
                    }
                }
                
                appUser.UserName = model.Username;
                appUser.Name = model.Name;
                appUser.Surname = model.Surname;
                appUser.PhoneNumber = model.PhoneNumber;
                appUser.Email = model.Email;
                appUser.IsActive = model.IsActive;

                var upadateResult = await userManager.UpdateAsync(appUser);

                if (!upadateResult.Succeeded)
                {
                    throw new Exception($"ERROR actualizando los datos de usuario - {upadateResult.Errors}");
                }

                return await ModelToDTO(appUser, userManager);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserDTO> Delete(int id, UserManager<ApplicationUser> userManager)
        {
            try
            {
                ApplicationUser user = await userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    throw new Exception($"Usuario con id {id} no encontrado");
                }

                var deletResult = await userManager.DeleteAsync(user);
                if (!deletResult.Succeeded)
                {
                    throw new Exception($"ERROR eliminando el ususario {user.UserName}");
                }

                return await ModelToDTO(user, userManager);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #region Support Methods

        public async Task<UserDTO> ModelToDTO(ApplicationUser user, UserManager<ApplicationUser> userManager) 
        {
            IList<string> roleList = await userManager.GetRolesAsync(user);
            if (roleList == null || !roleList.Any())
            {
                throw new Exception($"Rol del usuario {user.UserName} no encontrado");
            }

            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                IsActive = user.IsActive,
                UserName = user.UserName,
                Email = user.Email,
                Role = roleList.First(), // Los usuarios solo tendran un rol
                PhoneNumber = user.PhoneNumber,
                CreationDate = user.CreationDate
            };
        }

        #endregion Support Methods
    }
}
