using DataBase.Enum;

namespace DataBase.Columns
{
    public class DateColumn: BaseColumn
    {
        public DateColumn(string EngName, string PerName, TableEnum TableName)
            : base(EngName, PerName, TableName) { }
    }
}
