using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgriShop.Models;
using System.Numerics;

namespace AgriShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AgriShopContext context;

        public ProductController(AgriShopContext context)
        {
            this.context = context;
        }

        #region GetALLProduct

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await context.Products.ToListAsync();
            return Ok(products);
        }
        #endregion 

        #region GetProductById
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        #endregion

        #region InsertProduct
        [HttpPost]
        public IActionResult InsertProduct(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
        }

        #endregion


        #region DeleteProductById
        [HttpDelete("{id}")]
        public IActionResult DeleteProductlById(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            context.Products.Remove(product);
            context.SaveChanges();
            return NoContent();
        }
        #endregion

        #region UpdateProductById

        [HttpPut("{id}")]
        public IActionResult UpdateDoctor(int id, Product product)
        {

            if (id != product.ProductId)
            {
                return BadRequest();
            }


            var existingProduct = context.Products.Find(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            // Update the fields
            existingProduct.ProductName = product.ProductName;
            existingProduct.ProductTypeId = product.ProductTypeId;
            existingProduct.SupplierId = product.SupplierId;
            existingProduct.Stock = product.Stock;
            existingProduct.ProductImg = product.ProductImg;
            existingProduct.UserId = product.UserId;
            existingProduct.ModifiedAt = DateTime.Now;


            context.Products.Update(existingProduct);
            context.SaveChanges();
            return NoContent();
        }

        #endregion

        #region select Top n record

        [HttpGet("top n record")]
        public async Task<ActionResult<IEnumerable<Product>>> GetTopNProducts()
        {

            var products = await context.Products.Take(2).ToListAsync();
            return Ok(products);

        }
        #endregion

        #region Filter
        [HttpGet("Filter")]
        public async Task<ActionResult<IEnumerable<Product>>> FilterProduct([FromQuery] int? ProductId, [FromQuery] string? ProductName)
        {
            var query = context.Products.AsQueryable();

            if (ProductId.HasValue)
                query = query.Where(p => p.ProductId == ProductId);

            if (!string.IsNullOrEmpty(ProductName))
                query = query.Where(p => p.ProductName.Contains(ProductName));

            return await query.ToListAsync();
        }
        #endregion

    }
}
