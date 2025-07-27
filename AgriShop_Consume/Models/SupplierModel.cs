using System;

namespace AgriShop_Consume.Models
{
    public class SupplierModel
    {
        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public string? Contact { get; set; }
        public string? Address { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
} 