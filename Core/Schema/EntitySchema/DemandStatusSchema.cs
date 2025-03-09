using DataBase.Columns;
using DataBase.Enum;

namespace Core.Schema.EntitySchema
{
    public class DemandStatusSchema : BaseEntitySchema
    {
        public StringColumn DSName { get; private set; }
        public StringColumn DSDescription { get; private set; }
        public DemandStatusSchema()
        {
            EntityName = TableEnum.DemandStatus.ConvertToString();
            PersianName = "وضعیت سفارش";
            TableName = $"[{EntityName}]";

            foreach (BaseColumn clm in BaseColumns)
                clm.SetTable(TableEnum.DemandStatus);

            DSName = new StringColumn(nameof(DSName), "نام وضعیت سفارش", TableEnum.DemandStatus);
            DSDescription = new StringColumn(nameof(DSDescription), "شرح وضعیت سفارش", TableEnum.DemandStatus);
        }
    }
}
