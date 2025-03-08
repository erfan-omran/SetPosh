using System.Data;

namespace Core.Model
{
    public class UserTypeModel : BaseModel
    {
        public string UTName { get; set; }
        public string UTDescription { get; set; }
        public UserTypeModel() { }
        public UserTypeModel(DataRow dr)
        {
            UTName = (string)dr[nameof(UTName)];
            UTDescription = (string)dr[nameof(UTDescription)];
            base.InitBaseModel(dr);
        }
    }
}
