using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityServer4.Services;
using IdentityServer.Data.Identity;
using IdentityServer.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using IdentityServer.Models;
using IdentityServer.Models.Responses;
using IdentityServer.Services.Interfaces;

namespace IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailSenderService _emailSenderService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public AuthController(
            IConfiguration configuration,
            IEmailSenderService emailSenderService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interactionService)
        {
            _configuration = configuration;
            _emailSenderService = emailSenderService;
            _userManager = userManager;
            _signInManager = signInManager;
            _interactionService = interactionService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid request model");
                }

                return View(new LoginViewModel 
                { 
                    ReturnUrl = returnUrl
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel) 
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(loginModel);
                }

                var signInResult = await _signInManager.PasswordSignInAsync(loginModel.Username, loginModel.Password, false, false);

                if (!signInResult.Succeeded)
                {                    
                    return StatusCode(500, signInResult.IsNotAllowed);
                }

                return Redirect(loginModel.ReturnUrl);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }            
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid request model");
                }

                return View(new RegisterViewModel 
                { 
                    ReturnUrl = returnUrl 
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(registerModel);
                }

                ApplicationUser applicationUser = new ApplicationUser
                {
                    UserName = registerModel.Username,
                    Email = registerModel.Email,
                    PhoneNumber = registerModel.PhoneNumber,
                    Name = registerModel.Name,
                    Surname = registerModel.Surname,
                    CreationDate = DateTime.Now,
                    IsActive = true
                };

                var createResult = await _userManager.CreateAsync(applicationUser, registerModel.Password);

                if (!createResult.Succeeded)
                {
                    return StatusCode(500, createResult.Errors);
                }

                var user = await _userManager.FindByEmailAsync(applicationUser.Email);
                var roleresult = await _userManager.AddToRoleAsync(user, "Customer");

                if (!roleresult.Succeeded) 
                {
                    return StatusCode(500, $"ERROR on role asign - {roleresult.Errors}");
                }

                await _signInManager.SignInAsync(applicationUser, false);

                return Redirect(registerModel.ReturnUrl);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
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

                return Redirect(logoutRequest.PostLogoutRedirectUri);
            }
            catch (Exception e) 
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordRequest(ResetPasswordRequestModel emailModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid request email model");
                }

                var user = await _userManager.FindByEmailAsync(emailModel.Email);
                if (user == null)
                {
                    return BadRequest($"No user with email {emailModel.Email} exists");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callback = Url.Action(nameof(ResetPassword), "Auth", new { token, email = user.Email }, HttpMethod.Get.Method); // Request.Scheme

                var message = new EmailMessage(new string[] { user.Email }, "Entra Identity Reset Password Functionallity", callback, null);

                await _emailSenderService.SendEmailAsync(message);

                return Ok(new ResetPasswordResponseModel { Response = $"Email sent to {emailModel.Email} to reset the Password"});
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }            
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {            
            try
            {
                return View(new ResetPasswordViewModel
                {
                    Token = token,
                    Email = email
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordModel)
        {
            if (!ModelState.IsValid) 
            {
                return View(resetPasswordModel);
            }
             
            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);

            if (user == null) 
            {
                return View("ResetPasswordConfirmation", new ResetPasswordViewModel
                {
                    Result = $"El usuario con email {resetPasswordModel.Email} no existe"
                });
            }
                
            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }

            return View("ResetPasswordConfirmation", new ResetPasswordViewModel 
            { 
                Result = "Se ha establecido correctamente el nuevo Password. Ya puedes volver a acceder a la aplicación." 
            });
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

    }
}
