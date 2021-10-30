using Microsoft.EntityFrameworkCore;
using ProductsCrudApi.Data;
using ProductsCrudApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCrudApi.Repository
{
    public class ProductBrandRepository : IProductBrandRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ProductBrandRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddProductBrand(ProductBrand productBrand)
        {
            await _dbContext.ProductBrands.AddAsync(productBrand);
            _dbContext.SaveChanges();
        }

        public async Task DeleteProductBrand(long productBrandId)
        {
            var res = _dbContext.ProductBrands.Where(i => i.Id == productBrandId).FirstOrDefault();
            if(res != null)
            {
                _dbContext.Entry(res).State = EntityState.Deleted;
                await _dbContext.SaveChangesAsync();
            }
        }

        public ICollection<ProductBrand> GetAllProductBrands()
        {
            return _dbContext.ProductBrands.ToList();
        }

        public ProductBrand GetProductBrandById(long productBrandId)
        {
            var res = _dbContext.ProductBrands.Where(i => i.Id == productBrandId).FirstOrDefault();
            return res;
        }

        public async Task UpdateProductBrand(ProductBrand productBrand)
        {
            _dbContext.Entry(productBrand).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
