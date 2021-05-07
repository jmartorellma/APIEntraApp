using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIEntraApp.Controllers
{
    [ApiController]
    public class SecretController : ControllerBase
    {
        /*Solamente para iniciar el User Admin desde Postman - Comentar una vez iniciado*/
        [AllowAnonymous]
        [HttpPost]
        [Route("init-admin-user")]
        public async Task<ActionResult<ApplicationUser>> InitAdminUser([FromBody] RegisterRequestModel registerModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid request model");
                }

                ApplicationUser applicationUser = new ApplicationUser
                {
                    UserName = registerModel.UserName,
                    Email = registerModel.Email,
                    PhoneNumber = registerModel.PhoneNumber,
                    Name = registerModel.UserName,
                    Surname = registerModel.Surname,
                    CreationDate = DateTime.Now,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(applicationUser, registerModel.Password);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(applicationUser.Email);
                    var roleresult = _userManager.AddToRoleAsync(user, "SuperUser");

                    if (roleresult.Result.Succeeded)
                    {
                        return CreatedAtAction("GetApplicationUser", new { id = user.Id }, new UserModel
                        {
                            UserName = user.UserName,
                            Email = user.Email,
                            Name = user.Name,
                            Surname = user.Surname,
                            PhoneNumber = user.PhoneNumber,
                            CreationDate = user.CreationDate.ToString("dd/MM/yyyy HH:mm:ss"),
                            IsActive = user.IsActive
                        });
                    }
                    else
                    {
                        return StatusCode(500, roleresult.Result.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, result.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
