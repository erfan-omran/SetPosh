using System.Data;
using System.Data.SqlClient;

namespace Core.Model
{
    public class UserModel : BaseEntityModel
    {
        public UserTypeModel UserType { get; set; } = new UserTypeModel();
        public string UName { get; set; } = string.Empty;
        public string UFirstName { get; set; } = string.Empty;
        public string ULastName { get; set; } = string.Empty;
        public string UEmail { get; set; } = string.Empty;
        public string UTel { get; set; } = string.Empty;
        public string UPass { get; set; } = string.Empty;

        public UserModel() { }
        public UserModel(DataRow dr)
        {
            UserType = new UserTypeModel(dr);

            UName = dr[nameof(UName)].ConvertToString();
            UFirstName = dr[nameof(UFirstName)].ConvertToString();
            ULastName = dr[nameof(ULastName)].ConvertToString();
            UEmail = dr[nameof(UEmail)].ConvertToString();
            UTel = dr[nameof(UTel)].ConvertToString();
            UPass = dr[nameof(UPass)].ConvertToString();
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
            SqlParameter SIDParam = new SqlParameter("@" + Dictionary.User.SID.EngName, SID);
            if (IsAdd)
                SIDParam.Direction = System.Data.ParameterDirection.Output;
            Parameters.Add(SIDParam);
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.User.UTSID.EngName), UserType.SID));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.User.UName.EngName), UName));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.User.UFirstName.EngName), UFirstName));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.User.ULastName.EngName), ULastName));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.User.UEmail.EngName), UEmail));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.User.UTel.EngName), UTel));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.User.UPass.EngName), UPass));
        }
    }
}
