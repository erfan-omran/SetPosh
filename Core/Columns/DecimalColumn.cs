using DataBase.Enum;

namespace DataBase.Columns
{
    public class DecimalColumn : BaseColumn
    {
        public DecimalColumn(string EngName, string PerName, TableEnum TableName)
            : base(EngName, PerName, TableName) { }
    }
}
