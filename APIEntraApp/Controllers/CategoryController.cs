using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIEntraApp.Data;
using APIEntraApp.Services.Categories.Core;
using APIEntraApp.Services.Categories.Models.Request;

namespace APIEntraApp.Controllers
{
    [Authorize(Roles = "SuperUser,Admin,Shop")]
    [ApiController]
    [Route("/Category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ApiDbContext _apiDbContext;

        public CategoryController(
            ApiDbContext apiDbContext,
            ICategoryService categoryService)
        {
            _apiDbContext = apiDbContext;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición inválida");
                }

                return Ok(await _categoryService.GetAllAsync(_apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
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

                return Ok(await _categoryService.GetByIdAsync(id, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryPostRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de alta inválida");
                }

                return Ok(await _categoryService.CreateAsync(model, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(CategoryPutRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de actualización inválida");
                }

                return Ok(await _categoryService.UpdateAsync(model, _apiDbContext));
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

                return Ok(await _categoryService.DeleteAsync(id, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}
