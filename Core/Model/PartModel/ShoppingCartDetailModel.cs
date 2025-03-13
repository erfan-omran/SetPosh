using System.Data;
using System.Data.SqlClient;

namespace Core.Model
{
    public class ShoppingCartDetailModel : BasePartModel
    {
        public ShoppingCartModel ShoppingCart { get; set; } = new ShoppingCartModel();
        public ProductModel Product { get; set; } = new ProductModel();
        public long SCDCount { get; set; } = default;

        public ShoppingCartDetailModel() { }
        public ShoppingCartDetailModel(DataRow dr)
        {
            ShoppingCart = new ShoppingCartModel(dr);
            Product = new ProductModel(dr);

            SCDCount = dr[nameof(SCDCount)].ConvertToLong();
            base.InitBasePartModel(dr);
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
            SqlParameter IDParam = new SqlParameter("@" + Dictionary.ShoppingCartDetail.ID.EngName, ID);
            if (IsAdd)
                IDParam.Direction = System.Data.ParameterDirection.Output;
            Parameters.Add(IDParam);
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.ShoppingCartDetail.SCSID.EngName), ShoppingCart.SID));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.ShoppingCartDetail.PSID.EngName), Product.SID));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.ShoppingCartDetail.SCDCount.EngName), SCDCount));
        }
    }
}
