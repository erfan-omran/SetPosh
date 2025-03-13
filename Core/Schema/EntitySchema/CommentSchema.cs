using DataBase.Columns;
using DataBase.Enum;

namespace Core.Schema
{
    public class CommentSchema : BaseEntitySchema
    {
        public SIDColumn USID { get; set; }
        public SIDColumn PSID { get; set; }
        public ShortColumn CRate { get; set; }
        public StringColumn CDescription { get; set; }

        public CommentSchema()
        {
            EntityName = TableEnum.Comment.ConvertToString();
            PersianName = "نظر";
            TableName = $"[{EntityName}]";

            foreach (BaseColumn clm in BaseColumns)
                clm.SetTable(TableEnum.Comment);

            USID = new SIDColumn(nameof(USID), "کاربر", TableEnum.Comment);
            PSID = new SIDColumn(nameof(PSID), "کالا", TableEnum.Comment);
            CRate = new ShortColumn(nameof(CRate), "نمره", TableEnum.Comment);
            CDescription = new StringColumn(nameof(CDescription), "شرح نظر", TableEnum.Comment);
        }
    }
}
