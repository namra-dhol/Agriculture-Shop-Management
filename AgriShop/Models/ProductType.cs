using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AgriShop.Models;

public partial class ProductType
{
    public int ProductTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public int? UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }
    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    [JsonIgnore]
    public virtual User? User { get; set; }
}
