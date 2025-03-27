using System.Data;
using System.Data.SqlClient;

namespace Core.Model
{
    public class DemandStatusModel : BaseEntityModel
    {
        public string DSName { get; set; } = string.Empty;
        public string DSDescription { get; set; } = string.Empty;

        public DemandStatusModel() { }
        public DemandStatusModel(DataRow dr) : this(dr, false) { }
        public DemandStatusModel(DataRow dr, bool isNested)
        {
            DSName = dr.GetValueOfStringColumn(Dictionary.DemandStatus.DSName.EngName);
            DSDescription = dr.GetValueOfStringColumn(Dictionary.DemandStatus.DSDescription.EngName);
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
            SqlParameter SIDParam = new SqlParameter("@" + Dictionary.DemandStatus.SID.EngName, SID);
            if (IsAdd)
                SIDParam.Direction = System.Data.ParameterDirection.Output;
            Parameters.Add(SIDParam);
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.DemandStatus.DSName.EngName), DSName));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.DemandStatus.DSDescription.EngName), DSDescription));
        }
    }
}
