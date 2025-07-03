using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AgriShop.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Role { get; set; }

    [JsonIgnore]
    public virtual ICollection<ContactForm> ContactForms { get; set; } = new List<ContactForm>();
    [JsonIgnore]

    public virtual ICollection<ProductType> ProductTypes { get; set; } = new List<ProductType>();
    [JsonIgnore]
    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    [JsonIgnore]
    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}
