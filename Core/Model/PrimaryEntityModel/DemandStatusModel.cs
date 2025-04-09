using System.Data;
using System.Data.SqlClient;

namespace Core.Model
{
    public class DemandStatusModel : BaseEntityModel
    {
        public string DSName { get; set; } = string.Empty;
        public string DSDescription { get; set; } = string.Empty;

        public DemandStatusModel() { }
        public DemandStatusModel(DataRow dr)
        {
            DSName = dr.GetValueOfStringColumn(Dictionary.DemandStatus.DSName.EngName);
            DSDescription = dr.GetValueOfStringColumn(Dictionary.DemandStatus.DSDescription.EngName);
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
                Parameters.Add(new SqlParameter("@" + Dictionary.DemandStatus.SID.EngName, SID));
            Parameters.Add(new SqlParameter("@" + Dictionary.DemandStatus.DSName.EngName, DSName));
            Parameters.Add(new SqlParameter("@" + Dictionary.DemandStatus.DSDescription.EngName, DSDescription));
        }
    }
}
