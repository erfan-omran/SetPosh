using DataBase.Enum;

namespace DataBase.Columns
{
    public class BoolColumn : BaseColumn
    {
        public BoolColumn(string EngName, string PerName, TableEnum TableName)
            : base(EngName, PerName, TableName) { }
    }
}
