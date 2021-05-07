using System;
using System.Threading.Tasks;
using IdentityServer4.Services;
using IdentityServer.Data.Identity;
using IdentityServer.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interactionService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interactionService = interactionService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequestModel loginModel) 
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid request model");
                }

                var signInResult = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, false, false);

                if (!signInResult.Succeeded)
                {                    
                    return StatusCode(500, signInResult.IsNotAllowed);
                }

                return Ok("Logged in");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }            
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequestModel registerModel)
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

                var createResult = await _userManager.CreateAsync(applicationUser, registerModel.Password);

                if (!createResult.Succeeded)
                {
                    return StatusCode(500, createResult.Errors);
                }

                await _signInManager.SignInAsync(applicationUser, false);

                return Ok("Register Success and Logged in");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid request model");
                }

                await _signInManager.SignOutAsync();

                var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);

                if (string.IsNullOrWhiteSpace(logoutRequest.PostLogoutRedirectUri))
                {
                    return StatusCode(500, "Error logging out");
                }

                return Ok(logoutRequest.PostLogoutRedirectUri);
            }
            catch (Exception e) 
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
