using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace Core.Model
{
    //public class UserTypeModel : BaseEntityModel
    //{
    //    public string UTName { get; set; } = string.Empty;
    //    public string UTDescription { get; set; } = string.Empty;

    //    public UserTypeModel() { }
    //    public UserTypeModel(DataRow dr)
    //    {
    //        UTName = dr.GetValueOfStringColumn(nameof(UTName));
    //        UTDescription = dr.GetValueOfStringColumn(nameof(UTDescription));
    //        base.InitBaseEntityModel(dr);
    //    }
    //    //-------------
    //    public void SaveAddParameters()
    //    {
    //        SaveMainParameters(true);
    //        SaveBlockedParameter();
    //        SaveDeletedParameter();
    //        SaveCreationParameters();
    //        SaveModificationParameters();
    //    }
    //    public void SaveEditParameters()
    //    {
    //        SaveMainParameters(false);
    //        SaveBlockedParameter();
    //        SaveDeletedParameter();
    //        SaveModificationParameters();
    //    }
    //    public void SaveMainParameters(bool IsAdd)
    //    {
    //        if (!IsAdd)
    //            Parameters.Add(new SqlParameter("@" + Dictionary.UserType.SID.EngName, SID));
    //        Parameters.Add(new SqlParameter("@" + Dictionary.UserType.UTName.EngName, UTName));
    //        Parameters.Add(new SqlParameter("@" + Dictionary.UserType.UTDescription.EngName, UTDescription));
    //    }
    //}
}
