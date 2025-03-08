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
    public class UserSchema : BaseSchema
    {
        public SIDColumn UTSID { get; private set; }
        public StringColumn UName { get; private set; }
        public StringColumn UFirstName { get; private set; }
        public StringColumn ULastName { get; private set; }
        public StringColumn UEmail { get; private set; }
        public StringColumn UTel { get; private set; }
        public StringColumn UPass { get; private set; }

        public UserSchema()
        {
            EntityName = TableEnum.User.ToString();
            PersianName = "کاربر";
            TableName = $"[{EntityName}]";

            foreach (BaseColumn clm in BaseColumns)
                clm.SetTable(TableEnum.User);

            UTSID = new SIDColumn(nameof(UTSID), "نوع کاربری", TableEnum.User);
            UName = new StringColumn(nameof(UName), "نام کاربری", TableEnum.User);
            UFirstName = new StringColumn(nameof(UFirstName), "نام کاربر", TableEnum.User);
            ULastName = new StringColumn(nameof(ULastName), "نام خانوادگی کاربر", TableEnum.User);
            UEmail = new StringColumn(nameof(UEmail), "ایمیل", TableEnum.User);
            UTel = new StringColumn(nameof(UTel), "شماره تلفن", TableEnum.User);
            UPass = new StringColumn(nameof(UPass), "رمزعبور", TableEnum.User);
        }
    }
}
