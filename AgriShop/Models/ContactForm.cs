using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AgriShop.Models;

public partial class ContactForm
{
    public int ContactId { get; set; }

    public int? UserId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Message { get; set; }

    public DateTime? SubmittedAt { get; set; }
    [JsonIgnore]
    public virtual User? User { get; set; }
}
