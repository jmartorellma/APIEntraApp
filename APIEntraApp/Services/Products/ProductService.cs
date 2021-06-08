using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using APIEntraApp.Data;
using APIEntraApp.Data.Models;
using APIEntraApp.Services.Products.Core;
using APIEntraApp.Services.Products.Models.DTOs;
using APIEntraApp.Services.Products.Models.Request;

namespace APIEntraApp.Services.Products
{
    public class ProductService : IProductService
    {
        public async Task<List<ProductDTO>> GetAllAsync(ApiDbContext apiDbContext)
        {
            try
            {
                return await Task.Run(() =>
                {
                    List<ProductDTO> result = new List<ProductDTO>();

                    apiDbContext.Products.ToList().ForEach(p =>
                    {
                        result.Add(ModelToDTO(p));
                    });

                    return result;
                });
            }
            catch(Exception e) 
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ProductDTO> GetByIdAsync(int id, ApiDbContext apiDbContext)
        {
            try
            {
                Product product = await apiDbContext.Products.FindAsync(id);
                if (product == null) 
                {
                    throw new Exception($"Producto con id {id} no encontrado");
                }

                return ModelToDTO(product);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ProductDTO> CreateAsync(ProductPostRequest model, ApiDbContext apiDbContext)
        {
            try
            {
                var productFound = apiDbContext.Products.FirstOrDefault(p => p.Code.ToUpper().Trim().Equals(model.Code.ToUpper().Trim()));
                if (productFound != null)
                {
                    throw new Exception($"Ya existe un producto con el código {model.Code}");
                }

                Stock stock = new Stock
                {
                    Avaliable = model.Stock,
                    UpdatedDate = DateTime.Now
                };

                Product newProduct = new Product
                {                    
                    Code = model.Code,
                    Name = model.Name,
                    Description = model.Description,
                    IsActive = model.IsActive,
                    Price = model.Price,
                    Tax= model.Tax,
                    Pvp = model.Pvp,
                    CreationDate = DateTime.Now,
                    ShopId = model.ShopId,
                    ProviderId = model.ProviderId,
                    Stock = stock
                };

                model.CategoryIdList.ForEach(id => 
                {
                    newProduct.Product_Categories.Add(new Product_Category
                    {
                        CategoryId = id,
                        Product = newProduct
                    });
                });                

                await apiDbContext.Products.AddAsync(newProduct);
                
                await apiDbContext.SaveChangesAsync();

                return ModelToDTO(newProduct);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<string> UpdatePictureAsync(IFormFile file, int productID, IConfiguration configuration, ApiDbContext apiDbContext)
        {
            try
            {
                Product product = await apiDbContext.Products.FindAsync(productID);
                if (product == null)
                {
                    throw new Exception($"No se ha encontrado el producto con id {productID}");
                }

                string folderName = Path.Combine(configuration["ResourcesFolder"].ToString(),
                                                 configuration["ImagesFolder"].ToString(),
                                                 configuration["ProductsFolder"].ToString());

                string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                string fileName = string.Join("-", product.Shop.Code, product.Code);

                string fullPath = Path.Combine(pathToSave, fileName);
                string dbPath = Path.Combine(folderName, fileName);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                FileStream stream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(stream);
                await stream.DisposeAsync();

                product.Picture = dbPath;
                await apiDbContext.SaveChangesAsync();

                return dbPath;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ProductDTO> UpdateAsync(ProductPutRequest model, ApiDbContext apiDbContext)
        {
            try
            {
                var product = await apiDbContext.Products.FindAsync(model.Id);
                if (product == null)
                {
                    throw new Exception($"No existe el producto {model.Name} con id {model.Id}");
                }

                var productFound = apiDbContext.Products.FirstOrDefault(p => p.Id != model.Id && p.Code.ToUpper().Trim().Equals(model.Code.ToUpper().Trim()));
                if (productFound != null)
                {
                    throw new Exception($"Ya existe un producto con el código {model.Code}");
                }

                product.Product_Categories.RemoveAll(pc => !model.CategoryIdList.Contains(pc.CategoryId));
                model.CategoryIdList.Where(mc => !product.Product_Categories.Select(s => s.CategoryId).ToList().Contains(mc)).ToList().ForEach(catId => 
                {
                    product.Product_Categories.Add(new Product_Category
                    {
                        ProductId = product.Id,
                        CategoryId = catId
                    });
                });

                if (product.Stock.Avaliable != model.Stock) 
                {
                    product.Stock.Avaliable = model.Stock;
                    product.Stock.UpdatedDate = DateTime.Now;
                }               

                product.Code = model.Code;
                product.Name = model.Name;
                product.Description = model.Description;
                product.IsActive = model.IsActive;
                product.Price = model.Price;
                product.Tax = model.Tax;
                product.Pvp = model.Pvp;

                await apiDbContext.SaveChangesAsync();

                return ModelToDTO(product);
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
                var product = await apiDbContext.Products.FindAsync(id);
                if (product == null)
                {
                    throw new Exception($"Producto con id {id} no encontrado");
                }

                apiDbContext.Products.Remove(product);
                await apiDbContext.SaveChangesAsync();

                return product.Id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #region Support Methods

        private ProductDTO ModelToDTO(Product product) 
        {
            return new ProductDTO
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                Description = product.Description,
                IsActive = product.IsActive,
                Price = product.Price,
                Tax = product.Tax,
                Pvp = product.Pvp,
                Picture = product.Picture,
                CreationDate = product.CreationDate,
                Shop = product.Shop.Code,
                Stock = product.Stock.Avaliable,
                Categories = product.Product_Categories.Select(s => s.Category.Code).ToList()
            };
        }

        #endregion Support Methods
    }
}
