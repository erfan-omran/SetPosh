using DataBase;
using System.Data;

namespace Core.Model.EntityModel
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
    }
}
