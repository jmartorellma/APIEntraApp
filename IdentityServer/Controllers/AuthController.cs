using System;
using System.Threading.Tasks;
using IdentityServer4.Services;
using IdentityServer.Data.Identity;
using IdentityServer.Models.Requests;
using IdentityServer.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using IdentityServer.Models;
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
        public async Task<IActionResult> Login(string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(500, "Petición de login inválida");
                }

                var request = await _interactionService.GetAuthorizationContextAsync(returnUrl);

                string username = request.Parameters["username"];
                string password = request.Parameters["password"];

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return Redirect(_configuration["AngularClientEntraAppURL"] + "/accounts/login?error=Usuario o Password no encotrado en la llamada");
                }

                var signInResult = await _signInManager.PasswordSignInAsync(username, password, false, false);

                if (!signInResult.Succeeded)
                {
                    return Redirect(_configuration["AngularClientEntraAppURL"] + "/accounts/login?error=Usuario o Password incorrecto");
                }

                string cleanUrl = returnUrl.Replace($"&username={username}&password={password}", string.Empty);

                return Redirect(cleanUrl);
            }
            catch (Exception e)
            {
                return Redirect(_configuration["AngularClientEntraAppURL"] + "/accounts/login?error=" + e.Message);
            }
        }

        [HttpPost]
        [EnableCors("AllowOrigin")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel registerModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(500, "Petición de registro inválida");
                }

                var userFound = await _userManager.FindByEmailAsync(registerModel.Email);
                if (userFound != null) 
                {
                    return StatusCode(500, $"Ya existe un usuario en el sisitema con el Email {registerModel.Email}");
                }

                userFound = await _userManager.FindByNameAsync(registerModel.Username);
                if (userFound != null)
                {
                    return StatusCode(500, $"Ya existe un usuario en el sisitema con el Usuario {registerModel.Username}");
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
                    return StatusCode(500, $"ERROR dando de alta el usuario - {createResult.Errors}");
                }

                var user = await _userManager.FindByEmailAsync(applicationUser.Email);
                var roleresult = await _userManager.AddToRoleAsync(user, "Customer");

                if (!roleresult.Succeeded) 
                {
                    await _userManager.DeleteAsync(user);
                    return StatusCode(500, $"ERROR asignando el rol de usuario - {roleresult.Errors}");
                }

                await _signInManager.SignInAsync(applicationUser, false);

                return Ok(new RegisterResponseModel 
                { 
                    Message = $"Usuario creado correctamente" 
                });
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
        [EnableCors("AllowOrigin")]
        public async Task<IActionResult> ResetPasswordRequest([FromBody] ResetPasswordRequestModel emailModel)
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
                var callback = Url.Action(nameof(ResetPassword), "Auth", new { token, email = user.Email }, Request.Scheme);

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

            return RedirectToAction("ResetPasswordConfirmation", new ResetPasswordViewModel 
            { 
                Result = "Se ha establecido correctamente el nuevo Password. Ya puedes volver a acceder a la aplicación." 
            });
        }

        public IActionResult ResetPasswordConfirmation(ResetPasswordViewModel model)
        {
            return View(model);
        }

    }
}
