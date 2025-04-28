using DataBase.Columns;
using Core.Enum;

namespace Core.Schema
{
    public class BaseEnumSchema : BaseSchema
    {
        public SIDColumn ID { get; set; }

        public BaseEnumSchema()
        {
            ID = new SIDColumn(nameof(ID), "شناسه", TableEnum.None);

            BaseColumns.AddRange(new List<BaseColumn> { ID });
        }
    }
}
