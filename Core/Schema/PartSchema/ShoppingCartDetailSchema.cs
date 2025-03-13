using DataBase.Columns;
using DataBase.Enum;

namespace Core.Schema
{
    public class ShoppingCartDetailSchema : BasePartSchema
    {
        public SIDColumn SCSID { get; private set; }
        public SIDColumn PSID { get; private set; }
        public LongColumn SCDCount { get; private set; }

        public ShoppingCartDetailSchema()
        {
            EntityName = TableEnum.ShoppingCartDetail.ConvertToString();
            PersianName = "جزئیات سبد خرید";
            TableName = $"[{EntityName}]";

            foreach (BaseColumn clm in BaseColumns)
                clm.SetTable(TableEnum.ShoppingCartDetail);

            SCSID = new SIDColumn(nameof(SCSID), "سبد خرید", TableEnum.ShoppingCartDetail);
            PSID = new SIDColumn(nameof(PSID), "کالا", TableEnum.ShoppingCartDetail);
            SCDCount = new LongColumn(nameof(SCDCount), "تعداد", TableEnum.ShoppingCartDetail);
        }
    }
}
