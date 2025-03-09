using DataBase.Columns;
using DataBase.Enum;

namespace Core.Schema
{
    public class BaseEntitySchema : BaseSchema
    {
        public LongColumn ID { get; set; }

        public BaseEntitySchema()
        {
            ID = new LongColumn(nameof(ID), "شناسه", TableEnum.None);

            BaseColumns.AddRange(new List<BaseColumn> { ID });
        }
    }
}
