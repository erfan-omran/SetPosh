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
        public ProductModel(DataRow dr) : this(dr, false) { }
        public ProductModel(DataRow dr, bool isNested)
        {
            ProductCategory = new ProductCategoryModel(dr, !isNested);
            ProductCategory.SID = dr.GetValueOfLongColumn(Dictionary.Product.PCSID.EngName);

            PName = dr.GetValueOfStringColumn(Dictionary.Product.PName.EngName);
            PPrice = dr.GetValueOfDecimalColumn(Dictionary.Product.PPrice.EngName);
            PCount = dr.GetValueOfLongColumn(Dictionary.Product.PCount.EngName);
            PDescription = dr.GetValueOfStringColumn(Dictionary.Product.PDescription.EngName);
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
