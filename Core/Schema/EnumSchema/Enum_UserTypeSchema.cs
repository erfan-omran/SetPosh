using DataBase.Columns;
using DataBase.Enum;

namespace Core.Schema
{
    public class Enum_UserTypeSchema : BaseEnumSchema
    {
        public StringColumn UTName { get; set; }
        public StringColumn UTDescription { get; set; }

        public Enum_UserTypeSchema()
        {
            EntityName = TableEnum.UserType.ConvertToString();
            PersianName = "نوع کاربر";
            TableName = $"[{EntityName}]";

            foreach (BaseColumn clm in BaseColumns)
                clm.SetTable(TableEnum.UserType);

            UTName = new StringColumn(nameof(UTName), "نام نوع کاربری", TableEnum.UserType);
            UTDescription = new StringColumn(nameof(UTDescription), "توضیحات", TableEnum.UserType);
        }
    }
}
