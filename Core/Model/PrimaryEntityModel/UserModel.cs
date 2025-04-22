using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace Core.Model
{
    public class UserModel : BaseEntityModel
    {
        public long UTSID { get; set; } = default;
        public string UName { get; set; } = string.Empty;
        public string UFirstName { get; set; } = string.Empty;
        public string ULastName { get; set; } = string.Empty;
        public string UEmail { get; set; } = string.Empty;
        [Required]
        public string UTel { get; set; } = string.Empty;
        [Required]
        public string UPass { get; set; } = string.Empty;

        public Enum_UserTypeModel UserType { get; set; } = new Enum_UserTypeModel();

        public UserModel() { }
        public UserModel(DataRow dr)
        {
            UTSID = dr.GetValueOfLongColumn(Dictionary.User.UTSID.EngName);
            UName = dr.GetValueOfStringColumn(Dictionary.User.UName.EngName);
            UFirstName = dr.GetValueOfStringColumn(Dictionary.User.UFirstName.EngName);
            ULastName = dr.GetValueOfStringColumn(Dictionary.User.ULastName.EngName);
            UEmail = dr.GetValueOfStringColumn(Dictionary.User.UEmail.EngName);
            UTel = dr.GetValueOfStringColumn(Dictionary.User.UTel.EngName);
            UPass = dr.GetValueOfStringColumn(Dictionary.User.UPass.EngName);
                base.InitBaseEntityModel(dr);
        }
        //-------------
        public void SaveAddParameters()
        {
            SaveMainParameters(true);
            SaveBlockedParameter();
            SaveDeletedParameter();
            SaveCreationParameters();
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
                Parameters.Add(new SqlParameter("@" + Dictionary.User.SID.EngName, SID));
            Parameters.Add(new SqlParameter("@" + Dictionary.User.UTSID.EngName, UTSID));
            Parameters.Add(new SqlParameter("@" + Dictionary.User.UName.EngName, UName));
            Parameters.Add(new SqlParameter("@" + Dictionary.User.UFirstName.EngName, UFirstName));
            Parameters.Add(new SqlParameter("@" + Dictionary.User.ULastName.EngName, ULastName));
            Parameters.Add(new SqlParameter("@" + Dictionary.User.UEmail.EngName, UEmail));
            Parameters.Add(new SqlParameter("@" + Dictionary.User.UTel.EngName, UTel));
            Parameters.Add(new SqlParameter("@" + Dictionary.User.UPass.EngName, UPass));
        }
    }
}
