using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AgriShop.Models;

public partial class ProductVariant
{
    public int VariantId { get; set; }

    public int ProductId { get; set; }

    public string? Size { get; set; }

    public decimal? Price { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }
    [JsonIgnore]
    public virtual Product? Product { get; set; } = null!;
    [JsonIgnore]
    public virtual User? User { get; set; }
}
