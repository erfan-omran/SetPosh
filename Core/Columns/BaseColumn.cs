using Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Columns
{
    public class BaseColumn
    {
        public string EngName { get; internal set; }
        public string PerName { get; internal set; }
        public string DBColumnName { get; internal set; }
        public string TableName { get; internal set; }
        public virtual string FullDBName { get; internal set; } = "";

        public BaseColumn(string EngName, string PerName, TableEnum TableName, string DBName = "")
        {
            this.TableName = "[" + TableName.ToString() + "]";
            this.EngName = EngName;
            this.PerName = PerName;
            this.DBColumnName = DBName == "" ? string.Format("[{0}]", EngName) : DBName;
            this.FullDBName = this.TableName + "." + this.DBColumnName;
        }
        public void SetTable(TableEnum TableName, string DBName = "")
        {
            this.TableName = "[" + TableName.ToString() + "]";
            this.DBColumnName = DBName == "" ? string.Format("[{0}]", EngName) : DBName;
            this.FullDBName = this.TableName + "." + this.DBColumnName;
        }
    }
}
