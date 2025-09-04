namespace AgriShop_Consume.Models
{
    public class QueryRequestModel
    {
        public int ProductTypeId { get; set; }
        public int ProductId { get; set; }
        public int VariantId { get; set; }
    }

    public class QueryResponseModel
    {
        public string Message { get; set; } = string.Empty;
    }

    //public class ProductTypeModel
    //{
    //    public int ProductTypeId { get; set; }
    //    public string TypeName { get; set; } = string.Empty;
    //}

    //public class ProductModel
    //{
    //    public int ProductId { get; set; }
    //    public string ProductName { get; set; } = string.Empty;
    //    public string ProductImg { get; set; } = string.Empty;
    //    public int ProductTypeId { get; set; }
    //}

    //public class ProductVariantModel
    //{
    //    public int VariantId { get; set; }
    //    public int ProductId { get; set; }
    //    public string Size { get; set; } = string.Empty;
    //    public decimal? Price { get; set; }
    //}
}
