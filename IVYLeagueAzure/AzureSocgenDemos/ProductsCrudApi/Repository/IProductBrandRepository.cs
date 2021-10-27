using ProductsCrudApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCrudApi.Repository
{
    public interface IProductBrandRepository
    {
        ICollection<ProductBrand> GetAllProductBrands();
        Task AddProductBrand(ProductBrand productBrandId);
        Task DeleteProductBrand(long productBrandId);
        ProductBrand GetProductBrandById(long productBrandId);
        Task UpdateProductBrand(ProductBrand productBrand);
    }
}
