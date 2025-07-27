using System;

namespace AgriShop_Consume.Models
{
    public class ProductWithVariantsModel
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductImg { get; set; }
        public int? Stock { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public ProductTypeInfo? ProductType { get; set; }
        public List<ProductVariantInfo>? ProductVariants { get; set; }
    }

    public class ProductTypeInfo
    {
        public int ProductTypeId { get; set; }
        public string? TypeName { get; set; }
    }

    public class ProductVariantInfo
    {
        public int VariantId { get; set; }
        public string? Size { get; set; }
        public decimal? Price { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
} 