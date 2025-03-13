using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace Core.Model
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
            SqlParameter SIDParam = new SqlParameter("@" + Dictionary.Product.SID.EngName, SID);
            if (IsAdd)
                SIDParam.Direction = System.Data.ParameterDirection.Output;
            Parameters.Add(SIDParam);
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.Product.PCSID.EngName), ProductCategory.SID));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.Product.PName.EngName), PName));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.Product.PPrice.EngName), PPrice));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.Product.PCount.EngName), PCount));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.Product.PDescription.EngName), PDescription));
        }
    }
}
