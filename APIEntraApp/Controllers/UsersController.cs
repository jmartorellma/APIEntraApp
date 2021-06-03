using System;
using System.Threading.Tasks;
using APIEntraApp.Data.Identity;
using APIEntraApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIEntraApp.Data;

namespace APIEntraApp.Controllers
{
    [Authorize(Roles = "SuperUser,Admin")]
    [ApiController]
    [Route("/Users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ApiDbContext _apiContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersController(
            UserManager<ApplicationUser> userManager,
            ApiDbContext apiContext,
            IUserService userService)
        {
             _userManager = userManager;
             _apiContext = apiContext;
             _userService = userService;
        }

        [HttpGet]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await _userService.GetUserById(id, _userManager));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        //[AllowAnonymous]
        ////[Route("{id}")]
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

    public class ResponseResult
    {
        public string Result { get; set; }
    }
}
