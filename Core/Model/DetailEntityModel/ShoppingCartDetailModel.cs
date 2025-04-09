using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace Core.Model
{
    public class ShoppingCartDetailModel : BaseDetailModel
    {
        public long SCSID { get; set; } = default;
        public long PSID { get; set; } = default;
        public long SCDCount { get; set; } = default;

        public ShoppingCartModel ShoppingCart { get; set; } = new ShoppingCartModel();
        public ProductModel Product { get; set; } = new ProductModel();

        public ShoppingCartDetailModel() { }
        public ShoppingCartDetailModel(DataRow dr)
        {
            SCSID = dr.GetValueOfLongColumn(Dictionary.ShoppingCartDetail.SCSID.EngName);
            PSID = dr.GetValueOfLongColumn(Dictionary.ShoppingCartDetail.PSID.EngName);
            SCDCount = dr.GetValueOfLongColumn(Dictionary.ShoppingCartDetail.SCDCount.EngName);
            base.InitBaseDetailModel(dr);
        }
        //-------------
        public void SaveAddParameters(long USID)
        {
            SaveMainParameters(USID, true);
            SaveBlockedParameter();
            SaveDeletedParameter();
            SaveCreationParameters();
        }
        public void SaveEditParameters(long USID)
        {
            SaveMainParameters(USID, false);
            SaveBlockedParameter();
            SaveDeletedParameter();
            SaveModificationParameters();
        }
        public void SaveMainParameters(long USID, bool IsAdd)
        {
            if (!IsAdd)
                Parameters.Add(new SqlParameter("@" + Dictionary.ShoppingCartDetail.ID.EngName, ID));
            Parameters.Add(new SqlParameter("@" + Dictionary.ShoppingCart.USID.EngName, USID));
            Parameters.Add(new SqlParameter("@" + Dictionary.ShoppingCartDetail.PSID.EngName, PSID));
            Parameters.Add(new SqlParameter("@" + Dictionary.ShoppingCartDetail.SCDCount.EngName, SCDCount));
        }
    }
}
