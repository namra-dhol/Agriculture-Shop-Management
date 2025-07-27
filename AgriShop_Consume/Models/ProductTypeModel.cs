using System;

namespace AgriShop_Consume.Models
{
    public class ProductTypeModel
    {
        public int ProductTypeId { get; set; }
        public string TypeName { get; set; } = null!;
        public int? UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
} 