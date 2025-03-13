using DataBase.Columns;
using DataBase.Enum;

namespace Core.Schema
{
    public class BaseSchema
    {
        public string EntityName { get; set; } = "";
        public string PersianName { get; set; } = "";
        public string TableName { get; set; } = "";
        public BoolColumn Blocked { get; set; }
        public BoolColumn Deleted { get; set; }
        public SIDColumn CreationUSID { get; set; }
        public DateColumn CreationDate { get; set; }
        public TimeColumn CreationTime { get; set; }
        public SIDColumn LastModifiedUSID { get; set; }
        public DateColumn LastModifiedDate { get; set; }
        public TimeColumn LastModifiedTime { get; set; }
        protected List<BaseColumn> BaseColumns = new List<BaseColumn>();

        public BaseSchema()
        {
            Blocked = new BoolColumn(nameof(Blocked), "مسدود", TableEnum.None);
            Deleted = new BoolColumn(nameof(Deleted), "حذف", TableEnum.None);
            CreationUSID = new SIDColumn(nameof(CreationUSID), "کاربر ایجاد کننده", TableEnum.None);
            CreationDate = new DateColumn(nameof(CreationDate), "تاریخ ایجاد", TableEnum.None);
            CreationTime = new TimeColumn(nameof(CreationTime), "زمان ایجاد", TableEnum.None);
            LastModifiedUSID = new SIDColumn(nameof(LastModifiedUSID), "آخرین کاربر تغییر دهنده", TableEnum.None);
            LastModifiedDate = new DateColumn(nameof(LastModifiedDate), "تاریخ تغییر", TableEnum.None);
            LastModifiedTime = new TimeColumn(nameof(LastModifiedTime), "زمان تغییر", TableEnum.None);

            BaseColumns.AddRange(new List<BaseColumn> {
                Blocked, Deleted,
                CreationUSID, CreationDate, CreationTime,
                LastModifiedUSID, LastModifiedDate, LastModifiedTime
            });
        }
    }
}
