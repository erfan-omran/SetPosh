using DataBase.Enum;

namespace DataBase.Columns
{
    public class StringColumn: BaseColumn
    {
        public StringColumn(string EngName, string PerName, TableEnum TableName)
            : base(EngName, PerName, TableName) { }
    }
}
