using System.Data;

namespace Core.Model.EntityModel
{
    public class ProductCategoryModel : BaseEntityModel
    {
        public ProductCategoryModel ProductCategory { get; set; } = new ProductCategoryModel();
        public string PCName { get; set; } = string.Empty;
        public string PCDescription { get; set; } = string.Empty;
        public ProductCategoryModel() { }
        public ProductCategoryModel(DataRow dr)
        {
            ProductCategory = new ProductCategoryModel(dr);

            PCName = dr[nameof(PCName)].ConvertToString();
            PCDescription = dr[nameof(PCDescription)].ConvertToString();
            base.InitBaseEntityModel(dr);
        }
    }
}
