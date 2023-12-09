using EBazar_BAL.Interfaces;
using EBazar_DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EBazar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductManager _productManager;

        public ProductController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        [HttpPost("createProduct")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductModelCreate productModel)
        {
            var result = await _productManager.CreateProduct(productModel);
            if (result)
            {
                return Ok("Product created successfully");
            }
            else
            {
                return BadRequest("Failed to create Product");
            }
        }

        [HttpDelete("deleteProduct/{name}")]
        public async Task<IActionResult> DeleteProduct(string name)
        {
            var result = await _productManager.DeleteProduct(name);
            if (result)
            {
                return Ok("Product deleted successfully");
            }
            else
            {
                return BadRequest("Failed to delete the Product");
            }
        }

        [HttpGet("getProductById")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _productManager.GetProductById(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed to get Product");
            }
        }

        [HttpGet("getProductsByName")]
        public async Task<IActionResult> GetProductsByName(string name)
        {
            var result = await _productManager.GetProductsByName(name);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed to get Product");
            }
        }

        [HttpGet("getProductsByType")]
        public async Task<IActionResult> GetProductsByType(string type)
        {
            var result = await _productManager.GetProductsByType(type);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed to delete Product");
            }
        }

        [HttpGet("getAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productManager.GetAllProducts();
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed to get Product");
            }
        }

        [HttpPut("updateProduct")]
        public async Task<IActionResult> UpdateProduct(int id, ProductModel productModel)
        {
            var result = await _productManager.UpdateProduct(id, productModel);
            if (result != null)
            {
                return Ok("Product was successfully updated");
            }
            else
            {
                return BadRequest("Failed to delete Product");
            }
        }

    }
}
