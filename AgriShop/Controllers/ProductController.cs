using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgriShop.Models;
using System.Numerics;
using System.Linq;

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

    
        // #region GetProductsByType & Variants (using select)

        // [HttpGet("bytype/{productTypeId}")]
        // public async Task<ActionResult> GetProductsByTypeIncludingVariants(int productTypeId)
        // {
        //     var products = await context.Products
        //         .Where(p => p.ProductTypeId == productTypeId)
        //         .Select(p => new
        //         {
        //             p.ProductId,
        //             p.ProductName,
        //             p.ProductImg,
        //             ProductVariants = p.ProductVariants.Select(v => new
        //             {
        //                 v.Size,
        //                 v.Price
        //             }).ToList()
        //         })
        //         .ToListAsync();

        //     if (products == null || products.Count == 0)
        //         return NotFound($"No products found for ProductTypeId = {productTypeId}");

        //     return Ok(products);
        // }
        // #endregion

        #region GetProductsByType (Simple)
        [HttpGet("bytype-simple/{productTypeId}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByType(int productTypeId)
        {
            var products = await context.Products
                .Where(p => p.ProductTypeId == productTypeId)
                .ToListAsync();

            if (products == null || products.Count == 0)
                return NotFound($"No products found for ProductTypeId = {productTypeId}");

            return Ok(products);
        }
        #endregion

        #region GetProductWithVariants (View More)
        [HttpGet("with-variants/{productId}")]
        public async Task<ActionResult> GetProductWithVariants(int productId)
        {
            var product = await context.Products
                .Where(p => p.ProductId == productId)
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    p.ProductImg,
                    p.Stock,
                    p.CreatedAt,
                    p.ModifiedAt,
                    ProductType = new
                    {
                        p.ProductType.ProductTypeId,
                        p.ProductType.TypeName
                    },
                    ProductVariants = p.ProductVariants.Select(v => new
                    {
                        v.VariantId,
                        v.Size,
                        v.Price,
                        v.CreatedAt
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (product == null)
                return NotFound($"Product with ID {productId} not found");

            return Ok(product);
        }
        #endregion

        #region Dropdown APIs

        // Get all ProductTypes
        [HttpGet("dropdown/producttypes")]
        public async Task<ActionResult<IEnumerable<object>>> GetProductTypes()
        {
            return await context.ProductTypes
                .Select(pt => new { pt.ProductTypeId, pt.TypeName })
                .ToListAsync();
        }

        // Get all Suppliers
        [HttpGet("dropdown/suppliers")]
        public async Task<ActionResult<IEnumerable<object>>> GetSuppliers()
        {
            return await context.Suppliers
                .Select(s => new { s.SupplierId, s.SupplierName })
                .ToListAsync();
        }

        // Get all Users
        [HttpGet("dropdown/users")]
        public async Task<ActionResult<IEnumerable<object>>> GetUsers()
        {
            return await context.Users
                .Select(u => new { u.UserId, u.UserName }) // Replace UserName if different
                .ToListAsync();
        }
        #endregion





    }
}
