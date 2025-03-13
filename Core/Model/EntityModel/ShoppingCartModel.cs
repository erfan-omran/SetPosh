using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace Core.Model
{
    public class ShoppingCartModel : BaseEntityModel
    {
        public UserModel User { get; set; } = new UserModel();
        public bool IsActive { get; set; } = default;
        public bool Confirmed { get; set; } = default;

        public ShoppingCartModel() { }
        public ShoppingCartModel(DataRow dr)
        {
            User = new UserModel(dr);

            IsActive = dr[nameof(IsActive)].ConvertToBool();
            Confirmed = dr[nameof(Confirmed)].ConvertToBool();
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
            SqlParameter SIDParam = new SqlParameter("@" + Dictionary.ShoppingCart.SID.EngName, SID);
            if (IsAdd)
                SIDParam.Direction = System.Data.ParameterDirection.Output;
            Parameters.Add(SIDParam);
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.ShoppingCart.USID.EngName), User.SID));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.ShoppingCart.IsActive.EngName), IsActive));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.ShoppingCart.Confirmed.EngName), Confirmed));
        }
    }
}
