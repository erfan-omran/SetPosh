using System.Data;

namespace Core.Model.EntityModel
{
    public class UserTypeModel : BaseEntityModel
    {
        public string UTName { get; set; } = string.Empty;
        public string UTDescription { get; set; } = string.Empty;
        public UserTypeModel() { }
        public UserTypeModel(DataRow dr)
        {
            UTName = dr[nameof(UTName)].ConvertToString();
            UTDescription = dr[nameof(UTDescription)].ConvertToString();
            base.InitBaseEntityModel(dr);
        }
    }
}
