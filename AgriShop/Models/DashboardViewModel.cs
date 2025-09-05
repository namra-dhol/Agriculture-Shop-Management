namespace AgriShop.Models
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int TotalProductTypes { get; set; }
        public int TotalSuppliers { get; set; }
        public int TotalUsers { get; set; }

        // Products count by type name
        public Dictionary<string, int> ProductsByType { get; set; } = new Dictionary<string, int>();

        // Products added per day in the requested window (date string -> count)
        public Dictionary<string, int> ProductsPerDay { get; set; } = new Dictionary<string, int>();

        // Variants count per product id (or name if included in query)
        public Dictionary<string, int> VariantsPerProduct { get; set; } = new Dictionary<string, int>();
    }
}
