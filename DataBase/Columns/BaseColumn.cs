using DataBase.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Columns
{
    public class BaseColumn
    {
        public string EngName { get; set; }
        public string PerName { get; set; }
        public string DBColumnName { get; set; }
        public string TableName { get; set; }
        public string FullDBName { get; set; }

        public BaseColumn(string engName, string perName, TableEnum tableName)
        {
            TableName = FormatTableName(tableName);
            EngName = engName;
            PerName = perName;
            DBColumnName = FormatColumnName(engName);
            FullDBName = $"{TableName}.{DBColumnName}";
        }
        private string FormatTableName(TableEnum tableName) => $"[{tableName}]";
        private string FormatColumnName(string columnName) => $"[{columnName}]";
    }
}
