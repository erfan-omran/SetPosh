using DataBase.Columns;
using DataBase.Enum;

namespace Core.Schema
{
    public class ProductSchema : BaseEntitySchema
    {
        public SIDColumn PCSID { get; private set; }
        public StringColumn PName { get; private set; }
        public DecimalColumn PPrice { get; private set; }
        public LongColumn PCount { get; private set; }
        public StringColumn PDescription { get; private set; }

        public ProductSchema()
        {
            EntityName = TableEnum.Product.ConvertToString();
            PersianName = "کالا";
            TableName = $"[{EntityName}]";

            foreach (BaseColumn clm in BaseColumns)
                clm.SetTable(TableEnum.Product);

            PCSID = new SIDColumn(nameof(PCSID), "دسته بندی کالا", TableEnum.Product);
            PName = new StringColumn(nameof(PName), "نام کالا", TableEnum.Product);
            PPrice = new DecimalColumn(nameof(PPrice), "قیمت کالا", TableEnum.Product);
            PCount = new LongColumn(nameof(PCount), "تعداد کالا", TableEnum.Product);
            PDescription = new StringColumn(nameof(PDescription), "شرح کالا", TableEnum.Product);
        }
    }
}
