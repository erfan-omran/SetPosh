using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace Core.Model
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
        //-------------
        public void SaveAddParameters()
        {
            SaveMainParameters(true);
            SaveBlockedParameter();
            SaveDeletedParameter();
            SaveCreationParameters();
            SaveModificationParameters();
        }
        public void SaveEditParameters()
        {
            SaveMainParameters(false);
            SaveBlockedParameter();
            SaveDeletedParameter();
            SaveModificationParameters();
        }
        public void SaveMainParameters(bool IsAdd)
        {
            SqlParameter SIDParam = new SqlParameter("@" + Dictionary.ProductCategory.SID.EngName, SID);
            if (IsAdd)
                SIDParam.Direction = System.Data.ParameterDirection.Output;
            Parameters.Add(SIDParam);
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.ProductCategory.PCSID.EngName), ProductCategory.SID));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.ProductCategory.PCName.EngName), PCName));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.ProductCategory.PCDescription.EngName), PCDescription));
        }
    }
}
