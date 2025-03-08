using DataBase.Columns;
using DataBase.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Schema
{
    public class UserTypeSchema : BaseSchema
    {
        public StringColumn UTName { get; private set; }
        public StringColumn UTDescription { get; private set; }
        public UserTypeSchema()
        {
            EntityName = TableEnum.UserType.ToString();
            PersianName = "نوع کاربری";
            TableName = $"[{EntityName}]";

            foreach (BaseColumn clm in BaseColumns)
                clm.SetTable(TableEnum.User);

            UTName = new StringColumn(nameof(UTName), "نام نوع کاربری", TableEnum.UserType);
            UTDescription = new StringColumn(nameof(UTDescription), "شرح نوع کاربری", TableEnum.UserType);
        }
    }
}
