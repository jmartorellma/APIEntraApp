using System;
using System.IO;
using System.Threading.Tasks;
using IdentityServer4.Services;
using IdentityServer.Models;
using IdentityServer.Models.Requests;
using IdentityServer.Models.Responses;
using IdentityServer.Data.Identity;
using IdentityServer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

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
                    throw new Exception("Petición de login inválida");
                }

                var request = await _interactionService.GetAuthorizationContextAsync(returnUrl);

                string username = request.Parameters["username"];
                string password = request.Parameters["password"];

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    throw new Exception("Usuario o Contraseña no encotrado en la llamada");
                }

                ApplicationUser user = await _userManager.FindByNameAsync(username);
                if (user != null && !user.IsActive) 
                {
                    throw new Exception("Tu usuario se encuentra desactivado. Contacta con el administrador.");
                }

                var signInResult = await _signInManager.PasswordSignInAsync(username, password, false, false);

                if (!signInResult.Succeeded)
                {
                    throw new Exception("Usuario o Password incorrecto");
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
                    throw new Exception("Petición de registro inválida");
                }

                var userFound = await _userManager.FindByEmailAsync(registerModel.Email);
                if (userFound != null) 
                {
                    throw new Exception($"Ya existe un usuario en el sisitema con el Email {registerModel.Email}");
                }

                userFound = await _userManager.FindByNameAsync(registerModel.Username);
                if (userFound != null)
                {
                    throw new Exception($"Ya existe un usuario en el sisitema con el Usuario {registerModel.Username}");
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
                    throw new Exception($"ERROR dando de alta el usuario - {createResult.Errors}");
                }

                var user = await _userManager.FindByEmailAsync(applicationUser.Email);
                var roleresult = await _userManager.AddToRoleAsync(user, "Customer");

                if (!roleresult.Succeeded) 
                {
                    await _userManager.DeleteAsync(user);
                    throw new Exception($"ERROR asignando el rol de usuario - {roleresult.Errors}");
                }

                // await _signInManager.SignInAsync(applicationUser, false);

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
                    throw new Exception("Petición de cerrar sesión inválida");
                }

                await _signInManager.SignOutAsync();

                var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);

                if (string.IsNullOrWhiteSpace(logoutRequest.PostLogoutRedirectUri))
                {
                    throw new Exception("Error cerrando la sesión");
                }

                return Redirect(logoutRequest.PostLogoutRedirectUri);
            }
            catch (Exception e) 
            {
                return Redirect(_configuration["AngularClientEntraAppURL"] + "/accounts/logout?error=" + e.Message);
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
                    throw new Exception("Petición de restablecer contarseña inválida");
                }

                var user = await _userManager.FindByEmailAsync(emailModel.Email);
                if (user == null)
                {
                    throw new Exception($"No existe ningún usuario con el email {emailModel.Email}");
                }

                Stream stream = GetType().Assembly.GetManifestResourceStream(_configuration["PathMailResetTemplate"]);

                if (stream == null)
                {
                    throw new Exception("Error catgando la plantilla del correo");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callback = Url.Action(nameof(ResetPassword), "Auth", new { token, email = user.Email }, Request.Scheme);

                StreamReader reader = new StreamReader(stream);
                string mailResetTemplate = reader.ReadToEnd();
                mailResetTemplate = mailResetTemplate.Replace(_configuration["TemplateNameTextToReplace"], user.Name);
                mailResetTemplate = mailResetTemplate.Replace(_configuration["TemplateSurameTextToReplace"], user.Surname);
                mailResetTemplate = mailResetTemplate.Replace(_configuration["TemplateLinkTextToReplace"], callback);                

                var message = new EmailMessage(new string[] { user.Email }, "Restablecer contarseña de Entra App Identity", mailResetTemplate, null);

                await _emailSenderService.SendEmailAsync(message);

                return Ok(new ResetPasswordResponseModel { Message = $"Se ha enviado un email a {emailModel.Email} para restablecer la contraseña"});
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
                    resetPasswordModel.Result += error.Description;
                }
                return View(resetPasswordModel);
            }

            return RedirectToAction("ResetPasswordConfirmation", new ResetPasswordViewModel 
            { 
                Result = "Se ha establecido correctamente la nueva contraseña. Ya puedes volver a acceder a la aplicación." 
            });
        }

        public IActionResult ResetPasswordConfirmation(ResetPasswordViewModel model)
        {
            return View(model);
        }

    }
}
