using DataBase.Columns;
using DataBase.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Core.Schema
{
    public class BaseSchema
    {
        public string EntityName { get; set; } = "";
        public string PersianName { get; set; } = "";
        public string TableName { get; set; } = "";
        public SIDColumn SID { get; set; }
        public BoolColumn Blocked { get; set; }
        public BoolColumn Deleted { get; set; }
        public SIDColumn CreationUSID { get; set; }
        public DateColumn CreationDate { get; set; }
        public TimeColumn CreationTime { get; set; }
        public SIDColumn LastModifiedUSID { get; set; }
        public DateColumn LastModifiedDate { get; set; }
        public TimeColumn LastModifiedTime { get; set; }
        public List<BaseColumn> BaseColumns = new List<BaseColumn>();

        public BaseSchema()
        {
            SID = new SIDColumn(nameof(SID), "شناسه", TableEnum.None);
            Blocked = new BoolColumn(nameof(Blocked), "مسدود", TableEnum.None);
            Deleted = new BoolColumn(nameof(Deleted), "حذف", TableEnum.None);
            CreationUSID = new SIDColumn(nameof(CreationUSID), "کاربر ایجاد کننده", TableEnum.None);
            CreationDate = new DateColumn(nameof(CreationDate), "تاریخ ایجاد", TableEnum.None);
            CreationTime = new TimeColumn(nameof(CreationTime), "زمان ایجاد", TableEnum.None);
            LastModifiedUSID = new SIDColumn(nameof(LastModifiedUSID), "آخرین کاربر تغییر دهنده", TableEnum.None);
            LastModifiedDate = new DateColumn(nameof(LastModifiedDate), "تاریخ تغییر", TableEnum.None);
            LastModifiedTime = new TimeColumn(nameof(LastModifiedTime), "زمان تغییر", TableEnum.None);

            BaseColumns.AddRange(new List<BaseColumn> {
                SID, Blocked, Deleted,
                CreationUSID, CreationDate, CreationTime,
                LastModifiedUSID, LastModifiedDate, LastModifiedTime
            });
        }
    }
}
