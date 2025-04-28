using Core.Enum;

namespace DataBase.Columns
{
    public class TimeColumn : BaseColumn
    {
        public TimeColumn(string EngName, string PerName, TableEnum TableName)
            : base(EngName, PerName, TableName) { }
    }
}
