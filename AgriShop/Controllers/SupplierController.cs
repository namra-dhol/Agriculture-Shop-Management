using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgriShop.Models;
using System.Text.Json.Serialization;

namespace AgriShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly AgriShopContext context;

        public SupplierController(AgriShopContext context)
        {
            this.context = context;
        }

        #region GetAllSuppliers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
        {
            var suppliers = await context.Suppliers.ToListAsync();
            return Ok(suppliers);
        }
        #endregion

        #region GetSupplierById
        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplierById(int id)
        {
            var supplier = await context.Suppliers.FindAsync(id);
            if (supplier == null)
                return NotFound();

            return Ok(supplier);
        }
        #endregion

        #region InsertSupplier
        [HttpPost]
        public IActionResult InsertSupplier(Supplier supplier)
        {
            supplier.CreatedAt = DateTime.Now;

            context.Suppliers.Add(supplier);
            context.SaveChanges();

            return CreatedAtAction(nameof(GetSupplierById), new { id = supplier.SupplierId }, supplier);
        }
        #endregion

        #region UpdateSupplierById
        [HttpPut("{id}")]
        public IActionResult UpdateSupplier(int id, Supplier supplier)
        {
            if (id != supplier.SupplierId)
            {
                return BadRequest();
            }

            var existingSupplier = context.Suppliers.Find(id);
            if (existingSupplier == null)
            {
                return NotFound();
            }

            // Update fields
            existingSupplier.SupplierName = supplier.SupplierName;
            existingSupplier.Contact = supplier.Contact;
            existingSupplier.Address = supplier.Address;
            existingSupplier.UserId = supplier.UserId;
            existingSupplier.ModifiedAt = DateTime.Now;

            context.Suppliers.Update(existingSupplier);
            context.SaveChanges();
            return NoContent();
        }
        #endregion

        #region DeleteSupplierById
        [HttpDelete("{id}")]
        public IActionResult DeleteSupplier(int id)
        {
            var supplier = context.Suppliers.Find(id);
            if (supplier == null)
                return NotFound();

            context.Suppliers.Remove(supplier);
            context.SaveChanges();
            return NoContent();
        }
        #endregion

        #region FilterSuppliers
        [HttpGet("Filter")]
        public async Task<ActionResult<IEnumerable<Supplier>>> FilterSuppliers(
            [FromQuery] string? name,
            [FromQuery] string? contact)
        {
            var query = context.Suppliers.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(s => s.SupplierName!.Contains(name));

            if (!string.IsNullOrEmpty(contact))
                query = query.Where(s => s.Contact!.Contains(contact));

            return await query.ToListAsync();
        }
        #endregion

        #region GetTopNSuppliers
        [HttpGet("top")]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetTopNSuppliers([FromQuery] int n = 2)
        {
            var topSuppliers = await context.Suppliers.Take(n).ToListAsync();
            return Ok(topSuppliers);
        }
        #endregion
    }
}
