using System.Data;
using DataBase;

namespace Core.Model
{
    public class BaseModel
    {
        public long SID { get; set; } = default;
        public long CreationUSID { get; set; } = default;
        //public Date CreationDate { get; set; } = default;
        //public Time CreationTime { get; set; } = default;
        public long LastModifiedUSID { get; set; } = default;
        //public Date LastModifiedDate { get; set; } = default;
        //public Time LastModifiedTime { get; set; } = default;
        public void InitBaseModel(DataRow dr)
        {
            SID = dr[nameof(SID)].ConvertToLong();
            CreationUSID = dr[nameof(CreationUSID)].ConvertToLong();
            //CreationDate = dr[nameof(CreationDate)].ConvertToDateTime();
            //CreationTime = dr[nameof(CreationTime)].ConvertToDateTime();
            LastModifiedUSID = dr[nameof(LastModifiedUSID)].ConvertToLong();
            //LastModifiedDate = dr[nameof(LastModifiedDate)].ConvertToDateTime();
            //LastModifiedTime = dr[nameof(LastModifiedTime)].ConvertToDateTime();
        }
    }
}
