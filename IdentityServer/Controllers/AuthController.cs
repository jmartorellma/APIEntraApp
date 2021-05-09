﻿using System;
using System.Threading.Tasks;
using IdentityServer4.Services;
using IdentityServer.Data.Identity;
using IdentityServer.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Controllers
{
    public class AuthController : Controller
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

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid request model");
                }

                return View(new LoginViewModel { ReturnUrl = returnUrl});
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

                return View(new RegisterViewModel { ReturnUrl = returnUrl });
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
    }
}