using DataBase.Columns;
using DataBase.Enum;

namespace Core.Schema
{
    public class ProductImageSchema : BasePartSchema
    {
        public SIDColumn PSID { get; private set; }
        public LongColumn ImgName { get; private set; }
        public BoolColumn IsMain { get; private set; }

        public ProductImageSchema()
        {
            EntityName = TableEnum.ProductImage.ConvertToString();
            PersianName = "عکس های کالا";
            TableName = $"[{EntityName}]";

            foreach (BaseColumn clm in BaseColumns)
                clm.SetTable(TableEnum.ProductImage);

            PSID = new SIDColumn(nameof(PSID), "کالا", TableEnum.ProductImage);
            ImgName = new LongColumn(nameof(ImgName), "تعداد", TableEnum.ProductImage);
            IsMain = new BoolColumn(nameof(IsMain), "قیمت", TableEnum.ProductImage);
        }
    }
}
