using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIEntraApp.Data.Identity;
using APIEntraApp.Services.Users.Core;
using APIEntraApp.Services.Users.Models.Request;

namespace APIEntraApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/Users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersController(
            UserManager<ApplicationUser> userManager,
            IUserService userService)
        {
             _userManager = userManager;
             _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "SuperUser,Admin")]
        public async Task<IActionResult> GetAll() 
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición inválida");
                }

                return Ok(await _userService.GetAll(_userManager));
            }
            catch (Exception e) 
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "SuperUser,Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición inválida");
                }

                return Ok(await _userService.GetById(id, _userManager));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "SuperUser,Admin")]
        public async Task<IActionResult> Create(UserPostRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de alta inválida");
                }

                return Ok(await _userService.Create(model, _userManager));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserPutRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de actualización inválida");
                }

                return Ok(await _userService.Update(model, _userManager));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperUser,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de eliminar inválida");
                }

                return Ok(await _userService.Delete(id, _userManager));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }

}
