using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsCrudApi.Models;
using ProductsCrudApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ProductsCrudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductBrandController : ControllerBase
    {
        private readonly IProductBrandRepository _productBrandRepository;

        public ProductBrandController(IProductBrandRepository productBrandRepository)
        {
            _productBrandRepository = productBrandRepository;
        }

        [HttpGet]
        [Route("getAllProductBrands")]
        public IActionResult GetProductAllProductBrands()
        {
            var res = _productBrandRepository.GetAllProductBrands();
            return Ok(res);
        }

        [HttpPost]
        [Route("addProductBrand")]
        public async Task<IActionResult> AddProductBrand([FromBody]ProductBrand productBrand)
        {
            if (productBrand != null)
            {
                await _productBrandRepository.AddProductBrand(productBrand);
                return StatusCode(200);
            }
            return StatusCode((int)HttpStatusCode.BadRequest);
        }
        
        [HttpDelete]
        [Route("deleteProductBrandById")]
        public async Task<IActionResult> DeleteProductBrand(long productBrandId)
        {
            await _productBrandRepository.DeleteProductBrand(productBrandId);
            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpPut]
        [Route("updateProductBrand")]
        public async Task<IActionResult> UpdateProductBrand(ProductBrand productBrand)
        {
            await _productBrandRepository.UpdateProductBrand(productBrand);
            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("getProductBrandById")]
        public IActionResult GetProductBrandById(long productBrandId)
        {
            var res = _productBrandRepository.GetProductBrandById(productBrandId);
            return Ok(res);
        }
    }
}
