using System.Data;

namespace Core.Model.EntityModel
{
    public class ProductModel : BaseEntityModel
    {
        public ProductCategoryModel ProductCategory { get; set; } = new ProductCategoryModel();
        public string PName { get; set; } = string.Empty;
        public decimal PPrice { get; set; } = default;
        public long PCount { get; set; } = default;
        public string PDescription { get; set; } = string.Empty;
        public ProductModel() { }
        public ProductModel(DataRow dr)
        {
            ProductCategory = new ProductCategoryModel(dr);

            PName = dr[nameof(PName)].ConvertToString();
            PPrice = dr[nameof(PPrice)].ConvertToDecimal();
            PCount = dr[nameof(PCount)].ConvertToLong();
            PDescription = dr[nameof(PDescription)].ConvertToString();
            base.InitBaseEntityModel(dr);
        }
    }
}
