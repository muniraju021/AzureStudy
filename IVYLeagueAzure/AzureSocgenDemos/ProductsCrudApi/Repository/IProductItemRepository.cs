using ProductsCrudApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCrudApi.Repository
{
    public interface IProductItemRepository
    {
        ICollection<ProductItem> GetAllProductItems();
        Task AddProductItem(ProductItem productItem);
        Task DeleteProductItem(long productId);
        ProductItem GetProductItemById(long productId);
        Task UpdateProductItem(ProductItem productItem);
    }
}
