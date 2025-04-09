using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace Core.Model
{
    public class ShoppingCartModel : BaseEntityModel
    {
        public long USID { get; set; } = default;
        public bool IsActive { get; set; } = default;
        public bool Confirmed { get; set; } = default;

        public UserModel User { get; set; } = new UserModel();

        public ShoppingCartModel() { }
        public ShoppingCartModel(DataRow dr)
        {
            USID = dr.GetValueOfLongColumn(Dictionary.ShoppingCart.USID.EngName);
            IsActive = dr.GetValueOfBoolColumn(Dictionary.ShoppingCart.IsActive.EngName);
            Confirmed = dr.GetValueOfBoolColumn(Dictionary.ShoppingCart.Confirmed.EngName);
            base.InitBaseEntityModel(dr);
        }
        //-------------
        public void SaveAddParameters()
        {
            SaveMainParameters(true);
            SaveBlockedParameter();
            SaveDeletedParameter();
            SaveCreationParameters();
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
                Parameters.Add(new SqlParameter("@" + Dictionary.ShoppingCart.SID.EngName, SID));
            Parameters.Add(new SqlParameter("@" + Dictionary.ShoppingCart.USID.EngName, USID));
            Parameters.Add(new SqlParameter("@" + Dictionary.ShoppingCart.IsActive.EngName, IsActive));
            Parameters.Add(new SqlParameter("@" + Dictionary.ShoppingCart.Confirmed.EngName, Confirmed));
        }
    }
}
