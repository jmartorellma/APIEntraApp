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
                List<ProductDTO> result = new List<ProductDTO>();

                foreach(Product p in apiDbContext.Products.ToList())
                {
                    result.Add(await ModelToDTOAsync(p, apiDbContext));
                };

                return result;
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

                return await ModelToDTOAsync(product, apiDbContext);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<ProductDTO>> GetByShopIdAsync(int shopId, ApiDbContext apiDbContext)
        {
            try
            {
                List<ProductDTO> result = new List<ProductDTO>();   

                List<Product> productList = apiDbContext.Products.Where(p => p.Shop.Id == shopId).ToList();
                if (!productList.Any())
                {
                    return result;
                }

                foreach (Product p in productList)
                {
                    result.Add(await ModelToDTOAsync(p, apiDbContext));
                };

                return result;
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

                Shop shop = await apiDbContext.Shops.FindAsync(model.ShopId);
                if (shop == null)
                {
                    throw new Exception($"Tienda del producto no encontrada");
                }

                Provider provider = await apiDbContext.Providers.FindAsync(model.ProviderId);
                if (provider == null)
                {
                    throw new Exception($"Proveedor no encontrado");
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
                    Picture = string.Empty,
                    CreationDate = DateTime.Now,
                    Shop = shop,
                    Provider = provider,
                    Stock = stock
                };

                newProduct.Product_Category = new List<Product_Category>();
                newProduct.User_Product_Cart = new List<User_Product_Cart>();
                newProduct.User_Product_Favorite = new List<User_Product_Favorite>();
                newProduct.User_Product_Rating = new List<User_Product_Rating>();

                foreach(int id in model.CategoryIdList)
                {
                    Category category = await apiDbContext.Categories.FindAsync(id);
                    if (category == null)
                    {
                        throw new Exception($"Categoría no encontrada");
                    }

                    newProduct.Product_Category.Add(new Product_Category
                    {
                        Category = category,
                        Product = newProduct
                    });
                }                

                await apiDbContext.Products.AddAsync(newProduct);
                
                await apiDbContext.SaveChangesAsync();

                return await ModelToDTOAsync(newProduct, apiDbContext);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ProductPictureDTO> UpdatePictureAsync(IFormFile file, int productID, IConfiguration configuration, ApiDbContext apiDbContext)
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

                string extension = file.ContentType.Split("/")[1];
                string fileName = $"{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{extension}";

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

                return new ProductPictureDTO 
                { 
                    FilePath = dbPath,
                    ProductId = productID
                };
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

                List<Product_Category> rel = product.Product_Category;
                if (rel == null)
                {
                    rel = apiDbContext.Products_Categories.Where(pc => pc.ProductId == product.Id).ToList();
                }

                product.Product_Category = rel;

                product.Product_Category.RemoveAll(pc => !model.CategoryIdList.Contains(pc.CategoryId));
                foreach (int id in model.CategoryIdList.Where(mc => !product.Product_Category.Select(s => s.CategoryId).ToList().Contains(mc)).ToList())
                {
                    Category category = await apiDbContext.Categories.FindAsync(id);
                    if (category == null)
                    {
                        throw new Exception($"Categoría no encontrada");
                    }

                    product.Product_Category.Add(new Product_Category
                    {
                        Category = category,
                        Product = product
                    });
                }

                Stock stock = product.Stock;
                if (stock == null)
                {
                    stock = apiDbContext.Stocks.Where(s => s.Id == product.StockId).FirstOrDefault();
                    if (stock == null)
                    {
                        throw new Exception($"Stock no encontrado");
                    }
                    product.Stock = stock;
                }

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

                return await ModelToDTOAsync(product, apiDbContext);
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

                List<Product_Category> rel = apiDbContext.Products_Categories.Where(pc => pc.ProductId == product.Id).ToList();
                if (rel != null)
                {
                    rel.ForEach(r => 
                    {
                        apiDbContext.Products_Categories.Remove(r);
                    });
                    
                }
                apiDbContext.Products.Remove(product);
                Stock stock = apiDbContext.Stocks.Where(s => s.Id == product.StockId).FirstOrDefault();
                if (stock != null)
                {
                    apiDbContext.Stocks.Remove(stock);
                }

                await apiDbContext.SaveChangesAsync();

                return product.Id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #region Support Methods

        private async Task<ProductDTO> ModelToDTOAsync(Product product, ApiDbContext apiDbContext)
        {
            Shop shop = await apiDbContext.Shops.FindAsync(product.ShopId);
            if (shop == null)
            {
                throw new Exception($"Tienda no encontrada");
            }

            Provider provider = product.Provider;
            if (provider == null) 
            { 
                provider = apiDbContext.Providers.Where(p => p.Id == product.ProviderId).FirstOrDefault();
                if (provider == null) 
                {
                    throw new Exception($"Proveedor no encontrado");
                }
            }

            Stock stock = product.Stock;
            if (stock == null)
            {
                stock = apiDbContext.Stocks.Where(s => s.Id == product.StockId).FirstOrDefault();
                if (stock == null)
                {
                    throw new Exception($"Stock no encontrado");
                }
            }

            List<Product_Category> rel = product.Product_Category;

            if (rel == null) 
            {
                rel = apiDbContext.Products_Categories.Where(pc => pc.ProductId == product.Id).ToList();
            }

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
                ShopId = shop.Id,
                ShopName = shop.Name,
                ProviderId = product.Provider.Id,
                Stock = product.Stock.Avaliable,
                Categories = rel.Select(s => s.CategoryId).ToList()
            };
        }

        #endregion Support Methods
    }
}
