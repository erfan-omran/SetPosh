using DataBase.Columns;
using DataBase.Enum;

namespace Core.Schema
{
    public class BaseEntitySchema : BaseSchema
    {
        public LongColumn SID { get; set; }

        public BaseEntitySchema()
        {
            SID = new LongColumn(nameof(SID), "شناسه", TableEnum.None);

            BaseColumns.AddRange(new List<BaseColumn> { SID });
        }
    }
}
