using System.Data;
using DataBase;

namespace Core.Model
{
    public class BaseModel
    {
        public bool Blocked { get; set; } = default;
        public bool Deleted { get; set; } = default;
        public long CreationUSID { get; set; } = default;
        //public Date CreationDate { get; set; } = default;
        //public Time CreationTime { get; set; } = default;
        public long LastModifiedUSID { get; set; } = default;
        //public Date LastModifiedDate { get; set; } = default;
        //public Time LastModifiedTime { get; set; } = default;
        public void InitBaseModel(DataRow dr)
        {
            CreationUSID = dr[nameof(CreationUSID)].ConvertToLong();
            //CreationDate = dr[nameof(CreationDate)].ConvertToDateTime();
            //CreationTime = dr[nameof(CreationTime)].ConvertToDateTime();
            LastModifiedUSID = dr[nameof(LastModifiedUSID)].ConvertToLong();
            //LastModifiedDate = dr[nameof(LastModifiedDate)].ConvertToDateTime();
            //LastModifiedTime = dr[nameof(LastModifiedTime)].ConvertToDateTime();
        }
    }
}
