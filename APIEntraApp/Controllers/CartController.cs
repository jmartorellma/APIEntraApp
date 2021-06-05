using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIEntraApp.Data;
using APIEntraApp.Services.Carts.Core;
using APIEntraApp.Services.Carts.Models.Request;

namespace APIEntraApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/Cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ApiDbContext _apiDbContext;

        public CartController(
            ApiDbContext apiDbContext,
            ICartService cartService)
        {
            _apiDbContext = apiDbContext;
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserCart(int userId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición inválida");
                }

                return Ok(await _cartService.GetByUserIdAsync(userId, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductCartPostRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de alta inválida");
                }

                return Ok(await _cartService.AddAsync(model, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductCartPutRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de actualización inválida");
                }

                return Ok(await _cartService.UpdateAsync(model, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de eliminar inválida");
                }

                return Ok(await _cartService.RemoveAsync(id, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}
