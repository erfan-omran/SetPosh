using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace Core.Model
{
    public class BaseModel
    {
        public bool Blocked { get; set; } = default;
        public bool Deleted { get; set; } = default;
        public long CreationUSID { get; set; } = default;
        public PersianDate CreationDate { get; set; } = new PersianDate();
        public PersianTime CreationTime { get; set; } = new PersianTime();
        public long LastModifiedUSID { get; set; } = default;
        public PersianDate LastModifiedDate { get; set; } = new PersianDate();
        public PersianTime LastModifiedTime { get; set; } = new PersianTime();
        public List<SqlParameter> Parameters = new List<SqlParameter>();

        public void InitBaseModel(DataRow dr)
        {
            //CreationUSID = dr[nameof(CreationUSID)].ConvertToLong();
            //CreationDate = new PersianDate(dr[nameof(CreationDate)].ConvertToString());
            //CreationTime = new PersianTime(dr[nameof(CreationTime)].ConvertToString());
            //LastModifiedUSID = dr[nameof(LastModifiedUSID)].ConvertToLong();
            //LastModifiedDate = new PersianDate(dr[nameof(LastModifiedDate)].ConvertToString());
            //LastModifiedTime = new PersianTime(dr[nameof(LastModifiedTime)].ConvertToString());
            
            CreationUSID = dr.GetValueOfLongColumn(nameof(CreationUSID));
            CreationDate = dr.GetValueOfPersianDateColumn(nameof(CreationDate));
            CreationTime = dr.GetValueOfPersianTimeColumn(nameof(CreationTime));
            LastModifiedUSID = dr.GetValueOfLongColumn(nameof(LastModifiedUSID));
            LastModifiedDate = dr.GetValueOfPersianDateColumn(nameof(LastModifiedDate));
            LastModifiedTime = dr.GetValueOfPersianTimeColumn(nameof(LastModifiedTime));
        }
        //-------------
        public void SaveCreationParameters()
        {
            Parameters.Add(new SqlParameter("@" + nameof(CreationUSID), CreationUSID));
            Parameters.Add(new SqlParameter("@" + nameof(CreationDate), CreationDate.ToString()));
            Parameters.Add(new SqlParameter("@" + nameof(CreationTime), CreationTime.ToString()));
        }
        public void SaveModificationParameters()
        {
            Parameters.Add(new SqlParameter("@" + nameof(LastModifiedUSID), LastModifiedUSID));
            Parameters.Add(new SqlParameter("@" + nameof(LastModifiedDate), LastModifiedDate.ToString()));
            Parameters.Add(new SqlParameter("@" + nameof(LastModifiedTime), LastModifiedTime.ToString()));
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
