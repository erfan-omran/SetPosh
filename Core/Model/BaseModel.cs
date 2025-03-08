using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class BaseModel
    {
        public long SID { get; set; }
        public long CreationUSID { get; set; }
        //public Date CreationDate { get; set; }
        //public Time CreationTime { get; set; }
        public long LastModifiedUSID { get; set; }
        //public Date LastModifiedDate { get; set; }
        //public Time LastModifiedTime { get; set; }
        public void InitBaseModel(DataRow dr)
        {
            SID = (long)dr[nameof(SID)];
            CreationUSID = (long)dr[nameof(CreationUSID)];
            //CreationDate = dr[nameof(CreationDate)];
            //CreationTime = dr[nameof(CreationTime)];
            LastModifiedUSID = (long)dr[nameof(LastModifiedUSID)];
            //LastModifiedDate = dr[nameof(LastModifiedDate)];
            //LastModifiedTime = dr[nameof(LastModifiedTime)];

        }
    }
}
