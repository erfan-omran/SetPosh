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
        public ProductCategoryModel(DataRow dr) : this(dr, false) { }
        public ProductCategoryModel(DataRow dr, bool isNested)
        {
            PCName = dr.GetValueOfStringColumn(Dictionary.ProductCategory.PCName.EngName);
            PCDescription = dr.GetValueOfStringColumn(Dictionary.ProductCategory.PCDescription.EngName);
            if (!isNested)
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
            //Parameters.Add(new SqlParameter("@" + nameof(Dictionary.ProductCategory.PCSID.EngName), ProductCategory.SID));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.ProductCategory.PCName.EngName), PCName));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.ProductCategory.PCDescription.EngName), PCDescription));
        }
    }
}
