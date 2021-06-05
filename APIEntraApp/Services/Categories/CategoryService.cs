using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using APIEntraApp.Data;
using APIEntraApp.Data.Models;
using APIEntraApp.Services.Categories.Models.DTOs;
using APIEntraApp.Services.Categories.Models.Request;

namespace APIEntraApp.Services.Categories
{
    public class CategoryService
    {
        public async Task<List<CategoryDTO>> GetAllAsync(ApiDbContext apiDbContext)
        {
            try
            {
                return await Task.Run(() => apiDbContext.Categories.Select(s => ModelToDTO(s)).ToList());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CategoryDTO> GetByIdAsync(int id, ApiDbContext apiDbContext)
        {
            try
            {
                Category category = await apiDbContext.Categories.FindAsync(id);
                if (category == null)
                {
                    throw new Exception($"Método de pago con id {id} no encontrado");
                }

                return ModelToDTO(category);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CategoryDTO> CreateAsync(CategoryPostRequest model, ApiDbContext apiDbContext)
        {
            try
            {
                var categoryFound = apiDbContext.Categories.FirstOrDefault(p => p.Code.ToUpper().Trim().Equals(model.Code.ToUpper().Trim()));
                if (categoryFound != null)
                {
                    throw new Exception($"Ya existe una categoría con el código {model.Code}");
                }

                Category newCategory = new Category
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreationDate = DateTime.Now
                };

                await apiDbContext.Categories.AddAsync(newCategory);

                await apiDbContext.SaveChangesAsync();

                return ModelToDTO(newCategory);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CategoryDTO> UpdateAsync(CategoryPutRequest model, ApiDbContext apiDbContext)
        {
            try
            {
                var category = await apiDbContext.Categories.FindAsync(model.Id);
                if (category == null)
                {
                    throw new Exception($"No existe la categoría {model.Name} con id {model.Id}");
                }

                var categoryFound = apiDbContext.Categories.FirstOrDefault(p => p.Id != model.Id && p.Code.ToUpper().Trim().Equals(model.Code.ToUpper().Trim()));
                if (categoryFound != null)
                {
                    throw new Exception($"Ya existe una categoría con el código {model.Code}");
                }

                category.Code = model.Code;
                category.Name = model.Name;

                await apiDbContext.SaveChangesAsync();

                return ModelToDTO(category);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<int> DeleteAsync(int id, ApiDbContext apiDbContext)
        {
            try
            {
                var category = await apiDbContext.Categories.FindAsync(id);
                if (category == null)
                {
                    throw new Exception($"Categotía con id {id} no encontrada");
                }

                apiDbContext.Categories.Remove(category);
                await apiDbContext.SaveChangesAsync();

                return category.Id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #region Support Methods

        private CategoryDTO ModelToDTO(Category category)
        {
            return new CategoryDTO
            {
                Id = category.Id,
                Code = category.Code,
                Name = category.Name,
                CreationDate = category.CreationDate
            };
        }

        #endregion Support Methods
    }
}
