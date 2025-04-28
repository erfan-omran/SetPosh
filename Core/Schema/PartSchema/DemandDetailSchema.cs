using DataBase.Columns;
using Core.Enum;

namespace Core.Schema
{
    public class DemandDetailSchema : BasePartSchema
    {
        public SIDColumn DSID { get; private set; }
        public SIDColumn PSID { get; private set; }
        public LongColumn DDCount { get; private set; }
        public DecimalColumn DDPrice { get; private set; }

        public DemandDetailSchema()
        {
            EntityName = TableEnum.DemandDetail.ConvertToString();
            PersianName = "جزئیات سفارش";
            TableName = $"[{EntityName}]";

            foreach (BaseColumn clm in BaseColumns)
                clm.SetTable(TableEnum.DemandDetail);

            DSID = new SIDColumn(nameof(DSID), "سفارش", TableEnum.DemandDetail);
            PSID = new SIDColumn(nameof(PSID), "کالا", TableEnum.DemandDetail);
            DDCount = new LongColumn(nameof(DDCount), "تعداد", TableEnum.DemandDetail);
            DDPrice = new DecimalColumn(nameof(DDPrice), "قیمت", TableEnum.DemandDetail);
        }
    }
}
