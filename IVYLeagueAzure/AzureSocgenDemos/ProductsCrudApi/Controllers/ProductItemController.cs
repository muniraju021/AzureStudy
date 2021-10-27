using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsCrudApi.Models;
using ProductsCrudApi.Repository;
using ProductsCrudApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ProductsCrudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductItemController : ControllerBase
    {
        private readonly IProductItemRepository _productItemRepository;
        private readonly IBlobService _blobService;
        private const string ProductItemContainerName = "ProductItemsBlob";

        public ProductItemController(IProductItemRepository productItemRepository, IBlobService blobService)
        {
            _productItemRepository = productItemRepository;
            _blobService = blobService;
        }

        [HttpGet]
        [Route("api/getListOfBlobs")]
        public IList<string> GetListOfBlobs(string containerName)
        {
            return _blobService.GetBlobs(containerName);
        }


        [HttpGet]
        [Route("getAllProductItems")]
        public IActionResult GetProductAllProductItems()
        {
            var res = _productItemRepository.GetAllProductItems();
            return Ok(res);
        }

        [HttpPost]
        [Route("addProductItem")]
        public async Task<IActionResult> AddProductItem(IFormFile filePath, string productName, string description, long productBrandId)
        {
            ProductItem productItem = new ProductItem { Name = productName, Description = description, ProductBrandId = productBrandId };
            if (productItem != null)
            {
                var blobContainer = await _blobService.CreateContainer(ProductItemContainerName, false);
                var imagePath = _blobService.UploadBlob(blobContainer, filePath);
                productItem.ImagePath = imagePath;

                await _productItemRepository.AddProductItem(productItem);

                if (!string.IsNullOrWhiteSpace(imagePath))
                {
                    using (var imgStream = filePath.OpenReadStream())
                    {
                        var thumbnail = new Thumbnail
                        {
                            ProductName = productName,
                            ImageUrl = new Uri(imagePath),
                            BrandId = productBrandId.ToString()
                        };
                        _blobService.PostMessageToBlobQueue(thumbnail);
                    }
                        
                }

                return StatusCode(200);
            }
            return StatusCode((int)HttpStatusCode.BadRequest);
        }

        [HttpDelete]
        [Route("deleteProductItemById")]
        public async Task<IActionResult> DeleteProductItem(long productItemId)
        {
            await _productItemRepository.DeleteProductItem(productItemId);
            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpPut]
        [Route("updateProductItem")]
        public async Task<IActionResult> UpdateProductItem(ProductItem productItem)
        {
            await _productItemRepository.UpdateProductItem(productItem);
            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("getProductItemById")]
        public IActionResult GetProductItemById(long productItemId)
        {
            var res = _productItemRepository.GetProductItemById(productItemId);
            return Ok(res);
        }
    }
}
