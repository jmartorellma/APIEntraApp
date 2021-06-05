using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIEntraApp.Data;
using APIEntraApp.Services.Purchases.Core;
using APIEntraApp.Services.Purchases.Models.Request;
using Microsoft.Extensions.Configuration;

namespace APIEntraApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/Purchase")]
    public class PurchaseController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IPurchaseService _purchaseService;
        private readonly ApiDbContext _apiDbContext;

        public PurchaseController(
            IConfiguration configuration,
            ApiDbContext apiDbContext,
            IPurchaseService purchaseService)
        {
            _configuration = configuration;
            _apiDbContext = apiDbContext;
            _purchaseService = purchaseService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición inválida");
                }

                return Ok(await _purchaseService.GetByIdAsync(id, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("User/{id}")]
        public async Task<IActionResult> GetByUserId(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición inválida");
                }

                return Ok(await _purchaseService.GetByUserIdAsync(id, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("Shop/{id}")]
        public async Task<IActionResult> GetByShopId(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición inválida");
                }

                return Ok(await _purchaseService.GetByShopIdAsync(id, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(PurchasePostRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de alta inválida");
                }

                return Ok(await _purchaseService.CreateAsync(model, _apiDbContext, _configuration));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(PurchasePutRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de actualización inválida");
                }

                return Ok(await _purchaseService.UpdateAsync(model, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de eliminar inválida");
                }

                return Ok(await _purchaseService.DeleteAsync(id, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}
