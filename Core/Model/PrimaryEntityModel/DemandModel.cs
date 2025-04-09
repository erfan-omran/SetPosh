using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Core.Model
{
    public class DemandModel : BaseEntityModel
    {
        public long DSSID { get; set; } = default;
        public long SCSID { get; set; } = default;
        public long USID { get; set; } = default;
        public PersianDate DeliveryDate { get; set; } = PersianDate.Now;
        public bool Confirmed { get; set; } = default;

        public DemandStatusModel DemandStatus { get; set; } = new DemandStatusModel();
        public ShoppingCartModel ShoppingCart { get; set; } = new ShoppingCartModel();
        public UserModel User { get; set; } = new UserModel();

        public DemandModel() { }
        public DemandModel(DataRow dr)
        {
            DSSID = dr.GetValueOfLongColumn(Dictionary.Demand.DSSID.EngName);
            SCSID = dr.GetValueOfLongColumn(Dictionary.Demand.SCSID.EngName);
            USID = dr.GetValueOfLongColumn(Dictionary.Demand.USID.EngName);
            DeliveryDate = dr.GetValueOfPersianDateColumn(Dictionary.Demand.DeliveryDate.EngName);
            Confirmed = dr.GetValueOfBoolColumn(Dictionary.Demand.Confirmed.EngName);
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
                Parameters.Add(new SqlParameter("@" + Dictionary.Demand.SID.EngName, SID));
            Parameters.Add(new SqlParameter("@" + Dictionary.Demand.SCSID.EngName, SCSID));
            Parameters.Add(new SqlParameter("@" + Dictionary.Demand.DSSID.EngName, DSSID));
            Parameters.Add(new SqlParameter("@" + Dictionary.Demand.USID.EngName, USID));
            Parameters.Add(new SqlParameter("@" + Dictionary.Demand.DeliveryDate.EngName, DeliveryDate));
            Parameters.Add(new SqlParameter("@" + Dictionary.Demand.Confirmed.EngName, Confirmed));
        }
    }
}
