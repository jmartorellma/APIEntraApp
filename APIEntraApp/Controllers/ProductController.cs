using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using APIEntraApp.Data;
using APIEntraApp.Services.Products.Core;
using APIEntraApp.Services.Products.Models.Request;
using System.Linq;

namespace APIEntraApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/Product")]
    public class ProductController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        private readonly IProductService _productService;
        private readonly ApiDbContext _apiDbContext;
        
        public ProductController(
            IConfiguration configuration,
            IProductService productService,
            ApiDbContext apiDbContext)
        {
            _configuration = configuration;
            _productService = productService;
            _apiDbContext = apiDbContext;
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

                return Ok(await _productService.GetAllAsync(_apiDbContext));
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

                return Ok(await _productService.GetByIdAsync(id, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "SuperUser,Admin,Shop")]
        public async Task<IActionResult> Create(ProductPostRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de alta inválida");
                }

                return Ok(await _productService.CreateAsync(model, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
                
        [HttpPost("{id}/Picture")]
        [Authorize(Roles = "SuperUser,Admin,Shop")]
        public async Task<IActionResult> UpdatePicture(ProductPicturePutRequest model) 
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

                return Ok(await _productService.UpdatePictureAsync(file, model.ProductId, _configuration, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "SuperUser,Admin,Shop")]
        public async Task<IActionResult> Update(ProductPutRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de actualización inválida");
                }

                return Ok(await _productService.UpdateAsync(model, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperUser,Admin,Shop")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de eliminar inválida");
                }

                return Ok(await _productService.DeleteAsync(id, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }

}
