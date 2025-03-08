using DataBase.Enum;

namespace DataBase.Columns
{
    public class SIDColumn : BaseColumn
    {
        public SIDColumn(string EngName, string PerName, TableEnum TableName)
            : base(EngName, PerName, TableName) { }
    }
}
