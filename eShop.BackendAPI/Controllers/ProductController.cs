using eShop.Application.Catalog.Products;
using eShop.ViewModels.Catalog.ProductImages;
using eShop.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _imanageProductService;

        public ProductController(IProductService imanageProductService)
        {
            _imanageProductService = imanageProductService;
        }

        [HttpGet("{langId}")]
        public async Task<IActionResult> Get(string langId, [FromQuery] GetPublicProductPagingRequest request)
        {
            var product = await _imanageProductService.GetAllByCategoryId(langId, request);
            return Ok(product);
        }

        [HttpGet("GetAllPaging/{langId}")]
        public async Task<IActionResult> GetAllPaging(string langId, [FromQuery] GetPublicProductPagingRequest getPublicProductPagingRequest)
        {
            var product = await _imanageProductService.GetAllByCategoryId(langId, getPublicProductPagingRequest);
            return Ok(product);
        }


        [HttpGet("Paging")]
        public async Task<IActionResult> Paging([FromQuery] GetManageProductPagingRequest request)
        {
            var product = await _imanageProductService.GetAllPaging(request);
            return Ok(product);
        }

        [HttpGet("getById/{productId}/{langId}")]
        public async Task<IActionResult> getById(int productId, string langId)
        {
            var product = await _imanageProductService.GetById(productId, langId);
            if (product == null)
                return BadRequest("Cannot find product");
            return Ok(product);
        }

        [HttpPost("CreateProduct")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateRequest productCreateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productId = await _imanageProductService.Create(productCreateRequest);
            if (productId == 0)
            {
                return BadRequest();
            }
            else
            {
                var product = await _imanageProductService.GetById(productId, productCreateRequest.LanguageID);
                return CreatedAtAction(nameof(getById), new { productId = productId, langId = product.LanguageId }, product);
            }
        }

        [HttpPost("CategoryAssign/{id}/categories")]
        public async Task<IActionResult> CategoryAssign(int id,[FromBody] CategoryAssignRequest categoryAssignRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _imanageProductService.CategoryAssign(id, categoryAssignRequest);

            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductUpdateRequest productUpdateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productUpdate = await _imanageProductService.Update(productUpdateRequest);
            if (productUpdate == 0)
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }

        [HttpPatch("UpdatePrice/{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var productUpdatePrice = await _imanageProductService.UpdatePrice(productId, newPrice);
            if (productUpdatePrice == false)
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }

        [HttpDelete("DeleteProduct/DeleteProduct/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var productUpdate = await _imanageProductService.Delete(productId);
            if (productUpdate == 0)
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }

        [HttpPost("CreateImage/{productId}/images")]
        public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImagesCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imageId = await _imanageProductService.AddImage(productId, request);
            if (imageId == 0)
            {
                return BadRequest();
            }

            var productImage = await _imanageProductService.GetImageById(productId, imageId);

            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, productImage);
        }

        [HttpPut("UpdateImage/{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateImage(int productId, int imageId, [FromForm] ProductImagesUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var image = await _imanageProductService.UpdateImage(productId, imageId, request);
            if (image == 0)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("DeleteImage/{productId}/images/{imageId}")]
        public async Task<IActionResult> DeleteImage(int productId, int imageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var image = await _imanageProductService.DeleteImage(productId, imageId);
            if (image == 0)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("GetImageById/{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var productImage = await _imanageProductService.GetImageById(productId, imageId);
            if (productImage == null) { return BadRequest(); }
            return Ok(productImage);
        }

        [HttpPost("AddImages/{productId}")]
        public async Task<IActionResult> AddImages(int productId, [FromForm] List<IFormFile> images)
        {
            if (images == null)
            {
                return BadRequest();
            }

            var adds = await _imanageProductService.AddManyImageProduct(productId, images);
            if (adds == 0)
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }

        [HttpGet("GetImagesByProductId/{productId}")]
        public async Task<IActionResult> GetImagesByProductId(int productId)
        {
            var images = await _imanageProductService.GetImagesByProductId(productId);
            if (images == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(images);
            }
        }
    }
}