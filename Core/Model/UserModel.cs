using System.Data;

namespace Core.Model
{
    public class UserModel : BaseModel
    {
        public UserTypeModel UserType { get; set; }
        public string UName { get; set; }
        public string UFirstName { get; set; }
        public string ULastName { get; set; }
        public string UEmail { get; set; }
        public string UTel { get; set; }
        public string UPass { get; set; }

        public UserModel() { }
        public UserModel(DataRow dr)
        {
            UserType = new UserTypeModel(dr);
            UName = (string)dr[nameof(UName)];
            UFirstName = (string)dr[nameof(UFirstName)];
            ULastName = (string)dr[nameof(ULastName)];
            UEmail = (string)dr[nameof(UEmail)];
            UTel = (string)dr[nameof(UTel)];
            UPass = (string)dr[nameof(UPass)];
            base.InitBaseModel(dr);
        }
    }
}
