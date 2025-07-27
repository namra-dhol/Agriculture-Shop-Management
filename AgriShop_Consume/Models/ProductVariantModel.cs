using System;

namespace AgriShop_Consume.Models
{
    public class ProductVariantModel
    {
        public int VariantId { get; set; }
        public int ProductId { get; set; }
        public string? Size { get; set; }
        public decimal? Price { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
} 