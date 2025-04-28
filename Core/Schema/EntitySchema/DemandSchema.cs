using DataBase.Columns;
using Core.Enum;

namespace Core.Schema
{
    public class DemandSchema : BaseEntitySchema
    {
        public SIDColumn SCSID { get; private set; }
        public SIDColumn DSSID { get; private set; }
        public SIDColumn USID { get; private set; }
        public DateColumn DeliveryDate { get; private set; }
        public BoolColumn Confirmed { get; private set; }

        public DemandSchema()
        {
            EntityName = TableEnum.Demand.ConvertToString();
            PersianName = "سفارش";
            TableName = $"[{EntityName}]";

            foreach (BaseColumn clm in BaseColumns)
                clm.SetTable(TableEnum.Demand);

            SCSID = new SIDColumn(nameof(SCSID), "سبد خرید", TableEnum.Demand);
            DSSID = new SIDColumn(nameof(DSSID), "وضعیت سفارش", TableEnum.Demand);
            USID = new SIDColumn(nameof(USID), "کاربر", TableEnum.Demand);
            DeliveryDate = new DateColumn(nameof(DeliveryDate), "زمان ارسال", TableEnum.Demand);
            Confirmed = new BoolColumn(nameof(Confirmed), "تکمیل شده", TableEnum.Demand);
        }
    }
}
