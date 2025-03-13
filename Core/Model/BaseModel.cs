using System.Data;
using System.Data.SqlClient;

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
        public List<SqlParameter> Parameters = new List<SqlParameter>();

        public void InitBaseModel(DataRow dr)
        {
            CreationUSID = dr[nameof(CreationUSID)].ConvertToLong();
            //CreationDate = dr[nameof(CreationDate)].ConvertToDateTime();
            //CreationTime = dr[nameof(CreationTime)].ConvertToDateTime();
            LastModifiedUSID = dr[nameof(LastModifiedUSID)].ConvertToLong();
            //LastModifiedDate = dr[nameof(LastModifiedDate)].ConvertToDateTime();
            //LastModifiedTime = dr[nameof(LastModifiedTime)].ConvertToDateTime();
        }
        //-------------
        public void SaveCreationParameters()
        {
            Parameters.Add(new SqlParameter("@" + nameof(CreationUSID), CreationUSID));
            //Parameters.Add(new SqlParameter("@" + nameof(CreationDate), CreationDate));
            //Parameters.Add(new SqlParameter("@" + nameof(CreationTime), CreationTime));
        }
        public void SaveModificationParameters()
        {
            Parameters.Add(new SqlParameter("@" + nameof(LastModifiedUSID), LastModifiedUSID));
            //Parameters.Add(new SqlParameter("@" + nameof(LastModifiedDate), LastModifiedDate));
            //Parameters.Add(new SqlParameter("@" + nameof(LastModifiedTime), LastModifiedTime));
        }
        public void SaveBlockedParameter(long SID = -1)
        {
            if (SID > 0)
                Parameters.Add(new SqlParameter("@" + nameof(SID), SID));
            Parameters.Add(new SqlParameter("@" + nameof(Blocked), Blocked));
        }
        public void SaveDeletedParameter(long SID = -1)
        {
            if (SID > 0)
                Parameters.Add(new SqlParameter("@" + nameof(SID), SID));
            Parameters.Add(new SqlParameter("@" + nameof(Deleted), Deleted));
        }
    }
}
