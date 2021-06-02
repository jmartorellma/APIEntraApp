using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using APIEntraApp.Data.Identity;
using APIEntraApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace APIEntraApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(
            UserManager<ApplicationUser> userManager,
            IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers() 
        {
            try
            {
                return Ok(await _userService.GetAllUsers(_userManager));
            }
            catch (Exception e) 
            {
                return StatusCode(500, e.Message);
            }
        }

        //[AllowAnonymous]
        //[Route("{id}")]
        //public IActionResult Opened(int id)
        //{
        //    try
        //    {
        //        return Ok(new ResponseResult { Result = "Open message from API" });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}
    }

    //public class ResponseResult 
    //{
    //    public string Result { get; set; }
    //}
}
