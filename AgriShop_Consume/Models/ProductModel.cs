using Microsoft.AspNetCore.Mvc.Rendering;

namespace AgriShop_Consume.Models
{
    public class ProductModel
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

        // Dropdown lists
        public List<SelectListItem>? ProductTypeList { get; set; }
        public List<SelectListItem>? SupplierList { get; set; }
        public List<SelectListItem>? UserList { get; set; }
    }
}
