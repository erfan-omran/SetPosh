using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Core.Model
{
    public class DemandDetailModel : BasePartModel
    {
        public DemandModel Demand { get; set; } = new DemandModel();
        public ProductModel Product { get; set; } = new ProductModel();
        public long DDCount { get; set; } = default;
        public decimal DDPrice { get; set; } = default;

        public DemandDetailModel() { }
        public DemandDetailModel(DataRow dr)
        {
            Demand = new DemandModel(dr);
            Product = new ProductModel(dr);

            DDCount = dr[nameof(DDCount)].ConvertToLong();
            DDPrice = dr[nameof(DDPrice)].ConvertToDecimal();
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
            SqlParameter IDParam = new SqlParameter("@" + Dictionary.DemandDetail.ID.EngName, ID);
            if (IsAdd)
                IDParam.Direction = System.Data.ParameterDirection.Output;
            Parameters.Add(IDParam);
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.DemandDetail.DSID.EngName), Demand.SID));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.DemandDetail.PSID.EngName), Product.SID));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.DemandDetail.DDCount.EngName), DDCount));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.DemandDetail.DDPrice.EngName), DDPrice));
        }
    }
}
