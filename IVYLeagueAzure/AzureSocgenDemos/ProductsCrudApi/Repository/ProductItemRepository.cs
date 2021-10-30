using Microsoft.EntityFrameworkCore;
using ProductsCrudApi.Data;
using ProductsCrudApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCrudApi.Repository
{
    public class ProductItemRepository : IProductItemRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductItemRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddProductItem(ProductItem productItem)
        {
            await _dbContext.ProductItems.AddAsync(productItem);
            _dbContext.SaveChanges();
        }

        public async Task DeleteProductItem(long productId)
        {
            var res = _dbContext.ProductItems.Where(i => i.Id == productId).FirstOrDefault();
            if(res != null)
            {
                _dbContext.Entry(res).State = EntityState.Deleted;
                await _dbContext.SaveChangesAsync();
            }
        }

        public ICollection<ProductItem> GetAllProductItems()
        {
            return _dbContext.ProductItems.ToList();
        }

        public ProductItem GetProductItemById(long productId)
        {
            var res = _dbContext.ProductItems.Where(i => i.Id == productId).FirstOrDefault();
            return res;
        }

        public async Task UpdateProductItem(ProductItem productItem)
        {
            _dbContext.Entry(productItem).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
