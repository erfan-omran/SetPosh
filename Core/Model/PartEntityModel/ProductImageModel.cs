using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace Core.Model
{
    public class ProductImageModel : BasePartModel
    {
        public long PSID { get; set; } = default;
        public string ImgName { get; set; } = string.Empty;
        public bool IsMain { get; set; } = default;

        public List<SqlParameter> Parameters = new List<SqlParameter>();

        public ProductImageModel() { }
        public ProductImageModel(DataRow dr)
        {
            PSID = dr.GetValueOfLongColumn(Dictionary.ProductImage.PSID.EngName);
            ImgName = dr.GetValueOfStringColumn(Dictionary.ProductImage.ImgName.EngName);
            IsMain = dr.GetValueOfBoolColumn(Dictionary.ProductImage.IsMain.EngName);
        }
        //-------------

        public void SaveMainParameters()
        {
            //Parameters.Add(new SqlParameter("@" + Dictionary.ProductImage.ID.EngName, ID));
            Parameters.Add(new SqlParameter("@" + Dictionary.ProductImage.PSID.EngName, PSID > 0 ? PSID : SqlDbType.BigInt));
            Parameters.Add(new SqlParameter("@" + Dictionary.ProductImage.ImgName.EngName, ImgName));
            Parameters.Add(new SqlParameter("@" + Dictionary.ProductImage.IsMain.EngName, IsMain));
        }
    }
}
