using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace Core.Model
{
    public class ProductModel : BaseEntityModel
    {
        public long PCSID { get; set; } = default;
        public string PName { get; set; } = string.Empty;
        public decimal PPrice { get; set; } = default;
        public long PCount { get; set; } = default;
        public string PDescription { get; set; } = string.Empty;

        public ProductCategoryModel ProductCategory { get; set; } = new ProductCategoryModel();
        public decimal RateAVG { get; set; } = 0;

        public List<ProductImageModel> ProductImages = new List<ProductImageModel>();
        public List<CommentModel> ProductComments = new List<CommentModel>();

        public ProductModel() { }
        public ProductModel(DataRow dr)
        {
            PCSID = dr.GetValueOfLongColumn(Dictionary.Product.PCSID.EngName);
            PName = dr.GetValueOfStringColumn(Dictionary.Product.PName.EngName);
            PPrice = dr.GetValueOfDecimalColumn(Dictionary.Product.PPrice.EngName);
            PCount = dr.GetValueOfLongColumn(Dictionary.Product.PCount.EngName);
            PDescription = dr.GetValueOfStringColumn(Dictionary.Product.PDescription.EngName);
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
                Parameters.Add(new SqlParameter("@" + Dictionary.Product.SID.EngName, SID));
            Parameters.Add(new SqlParameter("@" + Dictionary.Product.PCSID.EngName, PCSID));
            Parameters.Add(new SqlParameter("@" + Dictionary.Product.PName.EngName, PName));
            Parameters.Add(new SqlParameter("@" + Dictionary.Product.PPrice.EngName, PPrice));
            Parameters.Add(new SqlParameter("@" + Dictionary.Product.PCount.EngName, PCount));
            Parameters.Add(new SqlParameter("@" + Dictionary.Product.PDescription.EngName, PDescription));
        }
    }
}
