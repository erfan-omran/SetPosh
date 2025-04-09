using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace Core.Model
{
    public class ProductCategoryModel : BaseEntityModel
    {
        public string PCName { get; set; } = string.Empty;
        public string PCDescription { get; set; } = string.Empty;

        public ProductCategoryModel() { }
        public ProductCategoryModel(DataRow dr)
        {
            PCName = dr.GetValueOfStringColumn(Dictionary.ProductCategory.PCName.EngName);
            PCDescription = dr.GetValueOfStringColumn(Dictionary.ProductCategory.PCDescription.EngName);
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
            if (!IsAdd)
                Parameters.Add(new SqlParameter("@" + Dictionary.ProductCategory.SID.EngName, SID));
            Parameters.Add(new SqlParameter("@" + Dictionary.ProductCategory.PCName.EngName, PCName));
            Parameters.Add(new SqlParameter("@" + Dictionary.ProductCategory.PCDescription.EngName, PCDescription));
        }
    }
}
