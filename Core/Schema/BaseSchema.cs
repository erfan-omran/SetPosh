using DataBase.Columns;
using DataBase.Enum;

namespace Core.Schema
{
    public class BaseSchema
    {
        public string EntityName { get; set; } = "";
        public string PersianName { get; set; } = "";
        public string TableName { get; set; } = "";
        protected List<BaseColumn> BaseColumns = new List<BaseColumn>();

        public BaseSchema()
        {
        }
    }
}
