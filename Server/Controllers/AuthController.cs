using IdentityServer.Requests;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
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
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request model");
            }

            var signInResult = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, false, false);

            if (!signInResult.Succeeded)
            {
                return Ok("Logged in");
            }
            else
            {

                return BadRequest("Error on manager login");
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequestModel registerModel)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest("Invalid request model");
            }

            var user = new IdentityUser(registerModel.UserName);
            var createResult = await _userManager.CreateAsync(user, registerModel.Password);

            if (!createResult.Succeeded) 
            {
                return BadRequest("Error on create user manager");
            }

            await _signInManager.SignInAsync(user, false);

            return Ok("Register Success and Logged in");
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();

            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);

            if (string.IsNullOrWhiteSpace(logoutRequest.PostLogoutRedirectUri)) 
            {
                return BadRequest("Error logging out");
            }

            return Ok(logoutRequest.PostLogoutRedirectUri);
        }
    }
}
