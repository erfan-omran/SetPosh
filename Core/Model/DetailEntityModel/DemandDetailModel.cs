using System.Data;
using System.Data.SqlClient;

namespace Core.Model
{
    public class DemandDetailModel : BaseDetailModel
    {
        public long DSID { get; set; } = default;
        public long PSID { get; set; } = default;
        public long DDCount { get; set; } = default;
        public decimal DDPrice { get; set; } = default;

        public DemandModel Demand { get; set; } = new DemandModel();
        public ProductModel Product { get; set; } = new ProductModel();

        public DemandDetailModel() { }
        public DemandDetailModel(DataRow dr)
        {
            DSID = dr.GetValueOfLongColumn(Dictionary.DemandDetail.DSID.EngName);
            PSID = dr.GetValueOfLongColumn(Dictionary.DemandDetail.PSID.EngName);
            DDCount = dr.GetValueOfLongColumn(Dictionary.DemandDetail.DDCount.EngName);
            DDPrice = dr.GetValueOfDecimalColumn(Dictionary.DemandDetail.DDPrice.EngName);
            base.InitBaseDetailModel(dr);
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
                Parameters.Add(new SqlParameter("@" + Dictionary.DemandDetail.ID.EngName, ID));
            Parameters.Add(new SqlParameter("@" + Dictionary.DemandDetail.DSID.EngName, DSID));
            Parameters.Add(new SqlParameter("@" + Dictionary.DemandDetail.PSID.EngName, PSID));
            Parameters.Add(new SqlParameter("@" + Dictionary.DemandDetail.DDCount.EngName, DDCount));
            Parameters.Add(new SqlParameter("@" + Dictionary.DemandDetail.DDPrice.EngName, DDPrice));
        }
    }
}
