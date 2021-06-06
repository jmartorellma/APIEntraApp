using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using APIEntraApp.Data;
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
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApiDbContext _apiDbContext;

        public UsersController(
            IConfiguration configuration,
            IUserService userService,
            UserManager<ApplicationUser> userManager, 
            ApiDbContext apiDbContext)
        {
            _configuration = configuration;
            _userManager = userManager;
            _userService = userService;
            _apiDbContext = apiDbContext;
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

                return Ok(await _userService.GetAllAsync(_userManager));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("Roles")]
        [Authorize(Roles = "SuperUser,Admin")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición inválida");
                }

                ClaimsPrincipal currentUser = User;

                return Ok(await _userService.GetRolesAsync(_apiDbContext, _userManager, currentUser));
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

                return Ok(await _userService.GetByIdAsync(id, _userManager));
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

                return Ok(await _userService.CreateAsync(model, _userManager));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Shop/Favorites")]
        public async Task<IActionResult> AddShopFavorites(UserAddShopFavoritesPostRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de favoritos inválida");
                }

                return Ok(await _userService.AddShopFavoritesAsync(model.UserId, model.ShopId, _userManager, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Shop/Rate")]
        public async Task<IActionResult> ShopRate(UserShopRatePostRequestcs model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de puntuar inválida");
                }

                return Ok(await _userService.RateShopsAsync(model, _userManager, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Product/Favorites")]
        public async Task<IActionResult> AddProductFavorites(UserAddProductFavoritesPostRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de favoritos inválida");
                }

                return Ok(await _userService.AddProductFavoritesAsync(model.UserId, model.ProductId, _userManager, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Product/Rate")]
        public async Task<IActionResult> ProductRate(UserProductRatePostRequestcs model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de puntuar inválida");
                }

                return Ok(await _userService.RateProductAsync(model, _userManager, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Picture")]
        public async Task<IActionResult> UpdatePicture(UserPicturePostRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de subir imagen inválida");
                }

                IFormCollection formCollection = await Request.ReadFormAsync();
                if (formCollection == null || !formCollection.Any())
                {
                    throw new Exception("No se ha encontrado la imagen en la llamada");
                }

                IFormFile file = formCollection.Files.First();
                if (file == null || file.Length == 0)
                {
                    throw new Exception("No se ha encontrado la imagen en la llamada");
                }

                return Ok(await _userService.UpdatePictureAsync(file, model.UserId, _configuration, _userManager));
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

                ClaimsPrincipal currentUser = User;

                return Ok(await _userService.UpdateAsync(model, _userManager, currentUser));
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

                return Ok(await _userService.DeleteAsync(id, _userManager));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("Shop/Favorites")]
        public async Task<IActionResult> RemoveFavorites(UserRemoveFavoritesDeleteRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de eliminar inválida");
                }

                return Ok(await _userService.RemoveShopFavoritesAsync(model.UserId, model.ShopId, _userManager, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }

}
