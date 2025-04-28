using System.Data;
using System.Data.SqlClient;

namespace Core.Model
{
    public class BaseModel
    {
        public bool Blocked { get; set; } = default;
        public bool Deleted { get; set; } = default;
        public long CreationUSID { get; set; } = default;
        public PersianDate CreationDate { get; set; } = PersianDate.Now;
        public PersianTime CreationTime { get; set; } = PersianTime.Now;
        public long LastModifiedUSID { get; set; } = default;
        public PersianDate LastModifiedDate { get; set; } = PersianDate.Now;
        public PersianTime LastModifiedTime { get; set; } = PersianTime.Now;
        public List<SqlParameter> Parameters = new List<SqlParameter>();

        public void InitBaseModel(DataRow dr)
        {
            Blocked = dr.GetValueOfBoolColumn(nameof(Blocked));
            Deleted = dr.GetValueOfBoolColumn(nameof(Deleted));
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
            Parameters.Add(new SqlParameter("@CurrentUSID", CreationUSID));
            Parameters.Add(new SqlParameter("@CurrentDate", CreationDate.ConvertToString()));
            Parameters.Add(new SqlParameter("@CurrentTime", CreationTime.ConvertToString()));
        }
        public void SaveModificationParameters()
        {
            Parameters.Add(new SqlParameter("@CurrentUSID", LastModifiedUSID));
            Parameters.Add(new SqlParameter("@CurrentDate", LastModifiedDate.ConvertToString()));
            Parameters.Add(new SqlParameter("@CurrentTime", LastModifiedTime.ConvertToString()));
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
