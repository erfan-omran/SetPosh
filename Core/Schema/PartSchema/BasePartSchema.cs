using DataBase.Columns;
using Core.Enum;

namespace Core.Schema
{
    public class BasePartSchema : BaseSchema
    {
        public SIDColumn ID { get; set; }

        public BasePartSchema()
        {
            ID = new SIDColumn(nameof(ID), "شناسه", TableEnum.None);

            BaseColumns.AddRange(new List<BaseColumn> { ID });
        }
    }
}
