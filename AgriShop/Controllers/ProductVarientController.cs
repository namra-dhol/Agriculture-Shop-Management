using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgriShop.Models;
using System.Text.Json.Serialization;

namespace AgriShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductVariantController : ControllerBase
    {
        private readonly AgriShopContext context;

        public ProductVariantController(AgriShopContext context)
        {
            this.context = context;
        }

        #region GetAllProductVariants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductVariant>>> GetProductVariants()
        {
            var variants = await context.ProductVariants.ToListAsync();
            return Ok(variants);
        }
        #endregion

        #region GetProductVariantById
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductVariant>> GetProductVariantById(int id)
        {
            var variant = await context.ProductVariants.FindAsync(id);
            if (variant == null)
                return NotFound();

            return Ok(variant);
        }
        #endregion

        #region InsertProductVariant
        [HttpPost]
        public IActionResult InsertProductVariant(ProductVariant variant)
        {
            try
            {
                variant.CreatedAt = DateTime.Now;

                context.ProductVariants.Add(variant);
                context.SaveChanges();

                return CreatedAtAction(nameof(GetProductVariantById), new { id = variant.VariantId }, variant);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion


        #region UpdateProductVariant
        [HttpPut("{id}")]
        public IActionResult UpdateProductVariant(int id, ProductVariant updatedVariant)
        {
            if (id != updatedVariant.VariantId)
                return BadRequest();

            var existingVariant = context.ProductVariants.Find(id);
            if (existingVariant == null)
                return NotFound();

            existingVariant.ProductId = updatedVariant.ProductId;
            existingVariant.Size = updatedVariant.Size;
            existingVariant.Price = updatedVariant.Price;
            existingVariant.UserId = updatedVariant.UserId;
            existingVariant.ModifiedAt = DateTime.Now;

            context.ProductVariants.Update(existingVariant);
            context.SaveChanges();

            return NoContent();
        }
        #endregion

        #region DeleteProductVariant
        [HttpDelete("{id}")]
        public IActionResult DeleteProductVariant(int id)
        {
            var variant = context.ProductVariants.Find(id);
            if (variant == null)
                return NotFound();

            context.ProductVariants.Remove(variant);
            context.SaveChanges();
            return NoContent();
        }
        #endregion

        #region FilterProductVariants
        [HttpGet("Filter")]
        public async Task<ActionResult<IEnumerable<ProductVariant>>> FilterProductVariant(
            [FromQuery] int? productId,
            [FromQuery] string? size)
        {
            var query = context.ProductVariants.AsQueryable();

            if (productId.HasValue)
                query = query.Where(v => v.ProductId == productId.Value);

            if (!string.IsNullOrEmpty(size))
                query = query.Where(v => v.Size.Contains(size));

            return await query.ToListAsync();
        }
        #endregion

        #region GetTopNProductVariants
        [HttpGet("top")]
        public async Task<ActionResult<IEnumerable<ProductVariant>>> GetTopNVariants([FromQuery] int n = 2)
        {
            var topVariants = await context.ProductVariants.Take(n).ToListAsync();
            return Ok(topVariants);
        }
        #endregion

    }
}
