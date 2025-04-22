using System.Data;
using System.Data.SqlClient;

namespace Core.Model
{
    public class Enum_UserTypeModel : BaseEnumModel
    {
        public string UTName { get; set; } = string.Empty;
        public string UTDescription { get; set; } = string.Empty;

        public Enum_UserTypeModel() { }
        public Enum_UserTypeModel(DataRow dr)
        {
            UTName = dr.GetValueOfStringColumn(nameof(UTName));
            UTDescription = dr.GetValueOfStringColumn(nameof(UTDescription));
        }
        //-------------

        public void SaveMainParameters(bool IsAdd)
        {
            if (!IsAdd)
                Parameters.Add(new SqlParameter("@" + Dictionary.UserType.ID.EngName, ID));
            Parameters.Add(new SqlParameter("@" + Dictionary.UserType.UTName.EngName, UTName));
            Parameters.Add(new SqlParameter("@" + Dictionary.UserType.UTDescription.EngName, UTDescription));
        }
    }
}
