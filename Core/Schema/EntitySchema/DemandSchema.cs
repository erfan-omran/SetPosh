using DataBase.Columns;
using DataBase.Enum;

namespace Core.Schema.EntitySchema
{
    public class DemandSchema : BaseEntitySchema
    {
        public SIDColumn DSSID { get; private set; }
        public SIDColumn USID { get; private set; }
        public DateColumn DeliveryDate { get; private set; }
        public BoolColumn PName { get; private set; }
        public DemandSchema()
        {
            EntityName = TableEnum.Demand.ConvertToString();
            PersianName = "سفارش";
            TableName = $"[{EntityName}]";

            foreach (BaseColumn clm in BaseColumns)
                clm.SetTable(TableEnum.Demand);

            DSSID = new SIDColumn(nameof(DSSID), "وضعیت سفارش", TableEnum.Demand);
            USID = new SIDColumn(nameof(USID), "کاربر", TableEnum.Demand);
            DeliveryDate = new DateColumn(nameof(DeliveryDate), "زمان ارسال", TableEnum.Demand);
            PName = new BoolColumn(nameof(PName), "تکمیل شده", TableEnum.Demand);
        }
    }
}
