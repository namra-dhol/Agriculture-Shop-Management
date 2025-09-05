using System.Collections.Generic;

namespace AgriShop_Consume.Models
{
    public class DashboardModel
    {
        public int TotalProducts { get; set; }
        public int TotalProductTypes { get; set; }
        public int TotalSuppliers { get; set; }
        public int TotalUsers { get; set; }

        public Dictionary<string, int> ProductsByType { get; set; } = new Dictionary<string, int>();

        public Dictionary<string, int> ProductsPerDay { get; set; } = new Dictionary<string, int>();

        public Dictionary<string, int> VariantsPerProduct { get; set; } = new Dictionary<string, int>();
    }
}


