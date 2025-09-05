using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AgriShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgriShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AgriShopContext context;

        public DashboardController(AgriShopContext context)
        {
            this.context = context;
        }

        #region Dashboard Summary
        // GET: /api/dashboard/summary?start=2024-01-01&end=2024-12-31
        [HttpGet("summary")]
        public async Task<ActionResult<DashboardViewModel>> GetSummary([FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            DateTime startDate = start?.Date ?? DateTime.UtcNow.AddDays(-29).Date;
            DateTime endDate = (end?.Date ?? DateTime.UtcNow.Date);

            if (endDate < startDate)
            {
                return BadRequest("End date must be greater than or equal to start date.");
            }

            var vm = new DashboardViewModel();

            // Totals
            vm.TotalProducts = await context.Products.CountAsync();
            vm.TotalProductTypes = await context.ProductTypes.CountAsync();
            vm.TotalSuppliers = await context.Suppliers.CountAsync();
            vm.TotalUsers = await context.Users.CountAsync();

            // Products by type
            vm.ProductsByType = await context.Products
                .Include(p => p.ProductType)
                .GroupBy(p => p.ProductType != null ? p.ProductType.TypeName : "Unknown")
                .Select(g => new { Key = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Key, x => x.Count);

            // Products per day within range
            var daily = await context.Products
                .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate.AddDays(1).AddTicks(-1))
                .GroupBy(p => p.CreatedAt!.Value.Date)
                .Select(g => new { Day = g.Key, Count = g.Count() })
                .ToListAsync();

            // Fill missing days with zero for chart completeness
            var cursor = startDate;
            while (cursor <= endDate)
            {
                var key = cursor.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                vm.ProductsPerDay[key] = 0;
                cursor = cursor.AddDays(1);
            }
            foreach (var d in daily)
            {
                var key = d.Day.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                vm.ProductsPerDay[key] = d.Count;
            }

            // Variants per product (top 10 by variant count)
            var variantsPerProduct = await context.ProductVariants
                .Include(v => v.Product)
                .GroupBy(v => v.Product != null ? v.Product.ProductName ?? ("Product #" + v.ProductId) : ("Product #" + v.ProductId))
                .Select(g => new { Key = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .ToListAsync();

            foreach (var item in variantsPerProduct)
            {
                vm.VariantsPerProduct[item.Key] = item.Count;
            }

            return Ok(vm);
        }
        #endregion
    }
}


