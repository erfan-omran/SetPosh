using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Core.Model
{
    public class DemandModel : BaseEntityModel
    {
        public DemandStatusModel DemandStatus { get; set; } = new DemandStatusModel();
        public ShoppingCartModel ShoppingCart { get; set; } = new ShoppingCartModel();
        public UserModel User { get; set; } = new UserModel();
        public PersianDate DeliveryDate { get; set; } = PersianDate.Now;
        public bool Confirmed { get; set; } = default;

        public DemandModel() { }
        public DemandModel(DataRow dr) : this(dr, false) { }
        public DemandModel(DataRow dr, bool isNested)
        {
            DemandStatus = new DemandStatusModel(dr, !isNested);
            DemandStatus.SID = dr.GetValueOfLongColumn(Dictionary.Demand.DSSID.EngName);
            ShoppingCart = new ShoppingCartModel(dr, !isNested);
            ShoppingCart.SID = dr.GetValueOfLongColumn(Dictionary.Demand.SCSID.EngName);
            User = new UserModel(dr, !isNested);
            User.SID = dr.GetValueOfLongColumn(Dictionary.Demand.USID.EngName);

            DeliveryDate = dr.GetValueOfPersianDateColumn(Dictionary.Demand.DeliveryDate.EngName);
            Confirmed = dr.GetValueOfBoolColumn(Dictionary.Demand.Confirmed.EngName);
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
            SqlParameter SIDParam = new SqlParameter("@" + Dictionary.Demand.SID.EngName, SID);
            if (IsAdd)
                SIDParam.Direction = System.Data.ParameterDirection.Output;
            Parameters.Add(SIDParam);
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.Demand.SCSID.EngName), ShoppingCart.SID));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.Demand.DSSID.EngName), DemandStatus.SID));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.Demand.USID.EngName), User.SID));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.Demand.DeliveryDate.EngName), DeliveryDate));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.Demand.Confirmed.EngName), Confirmed));
        }
    }
}
