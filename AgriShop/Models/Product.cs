using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AgriShop.Models;

public partial class Product
{
   
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public int ProductTypeId { get; set; }

    public int SupplierId { get; set; }

    public int? Stock { get; set; }

    public string? ProductImg { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    [JsonIgnore]
    public virtual ProductType? ProductType { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
    [JsonIgnore]
    public virtual Supplier? Supplier { get; set; } = null!;
    [JsonIgnore]
    public virtual User? User { get; set; }
}
