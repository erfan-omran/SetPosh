using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Core.Model
{
    public class DemandModel : BaseEntityModel
    {
        public DemandStatusModel DimandStatus { get; set; } = new DemandStatusModel();
        public ShoppingCartModel ShoppingCart { get; set; } = new ShoppingCartModel();
        public UserModel User { get; set; } = new UserModel();
        public PersianDate DeliveryDate { get; set; } = PersianDate.Now;
        public bool Confirmed { get; set; } = default;

        public DemandModel() { }
        public DemandModel(DataRow dr)
        {
            DimandStatus = new DemandStatusModel(dr);
            ShoppingCart = new ShoppingCartModel(dr);
            User = new UserModel(dr);

            DeliveryDate = new PersianDate(dr[nameof(DeliveryDate)].ConvertToString());
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
            SqlParameter SIDParam = new SqlParameter("@" + Dictionary.Demand.SID.EngName, SID);
            if (IsAdd)
                SIDParam.Direction = System.Data.ParameterDirection.Output;
            Parameters.Add(SIDParam);
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.Demand.SCSID.EngName), ShoppingCart.SID));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.Demand.DSSID.EngName), DimandStatus.SID));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.Demand.USID.EngName), User.SID));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.Demand.DeliveryDate.EngName), DeliveryDate));   
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.Demand.Confirmed.EngName), Confirmed));
        }
    }
}
