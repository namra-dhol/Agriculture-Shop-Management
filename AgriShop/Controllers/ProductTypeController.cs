using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgriShop.Models;
using System.Text.Json.Serialization;

namespace  AgriShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductTypeController : ControllerBase
    {
        private readonly AgriShopContext context;

        public ProductTypeController(AgriShopContext context)
        {
            this.context = context;
        }

        #region GetAllProductTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductType>>> GetProductTypes()
        {
            var types = await context.ProductTypes.ToListAsync();
            return Ok(types);
        }
        #endregion

        #region GetProductTypeById
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductType>> GetProductTypeById(int id)
        {
            var type = await context.ProductTypes.FindAsync(id);
            if (type == null)
                return NotFound();

            return Ok(type);
        }
        #endregion

        #region InsertProductType
        [HttpPost]
        public IActionResult InsertProductType(ProductType productType)
        {
            productType.CreatedAt = DateTime.Now;

            context.ProductTypes.Add(productType);
            context.SaveChanges();

            return CreatedAtAction(nameof(GetProductTypeById), new { id = productType.ProductTypeId }, productType);
        }
        #endregion

        #region UpdateProductType
        [HttpPut("{id}")]
        public IActionResult UpdateProductType(int id, ProductType updatedType)
        {
            if (id != updatedType.ProductTypeId)
                return BadRequest();

            var existingType = context.ProductTypes.Find(id);
            if (existingType == null)
                return NotFound();

            existingType.TypeName = updatedType.TypeName;
            existingType.UserId = updatedType.UserId;
            existingType.ModifiedAt = DateTime.Now;

            context.ProductTypes.Update(existingType);
            context.SaveChanges();

            return NoContent();
        }
        #endregion

        #region DeleteProductType
        [HttpDelete("{id}")]
        public IActionResult DeleteProductType(int id)
        {
            var type = context.ProductTypes.Find(id);
            if (type == null)
                return NotFound();

            context.ProductTypes.Remove(type);
            context.SaveChanges();
            return NoContent();
        }
        #endregion

        #region FilterProductType
        [HttpGet("Filter")]
        public async Task<ActionResult<IEnumerable<ProductType>>> FilterProductType([FromQuery] string? typeName)
        {
            var query = context.ProductTypes.AsQueryable();

            if (!string.IsNullOrEmpty(typeName))
                query = query.Where(pt => pt.TypeName.Contains(typeName));

            return await query.ToListAsync();
        }
        #endregion

        #region GetTopNProductTypes
        [HttpGet("top")]
        public async Task<ActionResult<IEnumerable<ProductType>>> GetTopNProductTypes([FromQuery] int n = 2)
        {
            var topTypes = await context.ProductTypes.Take(n).ToListAsync();
            return Ok(topTypes);
        }
        #endregion
    }
}
