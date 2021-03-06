using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using APIEntraApp.Data;
using APIEntraApp.Services.Shops.Core;
using APIEntraApp.Services.Shops.Models.Request;
using APIEntraApp.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace APIEntraApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/Shops")]
    public class ShopController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IShopService _shopService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApiDbContext _apiDbContext;
        public ShopController(
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            ApiDbContext apiDbContext,
            IShopService shopService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _apiDbContext = apiDbContext;
            _shopService = shopService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() 
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición inválida");
                }

                return Ok(await _shopService.GetAllAsync(_apiDbContext));
            }
            catch (Exception e) 
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición inválida");
                }

                return Ok(await _shopService.GetByIdAsync(id, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("Owner/{id}")]
        [Authorize(Roles = "SuperUser,Admin,Shop")]
        public async Task<IActionResult> GetByOwnerId(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición inválida");
                }

                return Ok(await _shopService.GetByOwnerIdAsync(id, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpGet("Locked/{id}")]
        [Authorize(Roles = "SuperUser,Admin,Shop")]
        public async Task<IActionResult> GetLockedUsers(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición inválida");
                }

                return Ok(await _shopService.GetLockedAsync(id, _apiDbContext, _userManager));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "SuperUser,Admin")]
        public async Task<IActionResult> Create(ShopPostRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de alta inválida");
                }

                return Ok(await _shopService.CreateAsync(model, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Picture")]
        [Authorize(Roles = "SuperUser,Admin,Shop")]
        public async Task<IActionResult> UpdatePicture()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de subir imagen inválida");
                }

                IFormCollection formCollection = await Request.ReadFormAsync();
                if (formCollection == null || !formCollection.Files.Any())
                {
                    throw new Exception("No se ha encontrado la imagen en la llamada");
                }

                IFormFile file = formCollection.Files.First();
                if (file == null || file.Length == 0)
                {
                    throw new Exception("No se ha encontrado la imagen en la llamada");
                }

                string shopId = Request.Headers["shopId"];
                
                if (string.IsNullOrWhiteSpace(shopId))
                {
                    throw new Exception("No se ha encontrado la tienda en la llamada");
                }

                return Ok(await _shopService.UpdatePictureAsync(file, int.Parse(shopId), _configuration, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("PaymentMethod")]
        [Authorize(Roles = "SuperUser,Admin,Shop")]
        public async Task<IActionResult> AddPaymentMethod(ShopPaymentMethodPostRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de alta inválida");
                }

                return Ok(await _shopService.AddPaymentethodAsync(model, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("PurchaseType")]
        [Authorize(Roles = "SuperUser,Admin,Shop")]
        public async Task<IActionResult> AddPurchaseType(ShopPurchaseTypePostRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de alta inválida");
                }

                return Ok(await _shopService.AddAllowedPurchaseTypeAsync(model, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("User/Locked")]
        [Authorize(Roles = "SuperUser,Admin,Shop")]
        public async Task<IActionResult> AddUserLocked(ShopUserLockedPostRequest model) 
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de bloquear inválida");
                }

                return Ok(await _shopService.AddUserLockedAsync(model.ShopId, model.UserId, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "SuperUser,Admin,Shop")]
        public async Task<IActionResult> Update(ShopPutRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de actualización inválida");
                }

                return Ok(await _shopService.UpdateAsync(model, _apiDbContext));
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

                return Ok(await _shopService.DeleteAsync(id, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("PaymentMethod")]
        [Authorize(Roles = "SuperUser,Admin,Shop")]
        public async Task<IActionResult> RemovePaymentMethod(ShopPaymentMethodDeleteRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de eliminar inválida");
                }

                return Ok(await _shopService.RemovePaymentMethodAsync(model, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("PurchaseType")]
        [Authorize(Roles = "SuperUser,Admin,Shop")]
        public async Task<IActionResult> RemovePurchaseType(ShopPurchaseTypeDeleteRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de eliminar inválida");
                }

                return Ok(await _shopService.RemovePurchaseTypeAsync(model, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("User/Locked")]
        [Authorize(Roles = "SuperUser,Admin,Shop")]
        public async Task<IActionResult> RemoveUserLocked(ShopUserLockedDeleteRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de desbloquear inválida");
                }

                return Ok(await _shopService.RemoveUserLockedAsync(model.ShopId, model.UserId, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }

}
