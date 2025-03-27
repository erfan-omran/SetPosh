using DataBase.Columns;
using DataBase.Enum;

namespace Core.Schema
{
    public class ProductCategorySchema : BaseEntitySchema
    {
        //public SIDColumn PCSID { get; set; }
        public StringColumn PCName { get; private set; }
        public StringColumn PCDescription { get; private set; }
        public ProductCategorySchema()
        {
            EntityName = TableEnum.ProductCategory.ConvertToString();
            PersianName = "دسته بندی کالا";
            TableName = $"[{EntityName}]";

            foreach (BaseColumn clm in BaseColumns)
                clm.SetTable(TableEnum.ProductCategory);

            //PCSID = new SIDColumn(nameof(PCSID), "دسته بندی کالای پدر", TableEnum.ProductCategory);
            PCName = new StringColumn(nameof(PCName), "نام نوع کاربری", TableEnum.ProductCategory);
            PCDescription = new StringColumn(nameof(PCDescription), "شرح نوع کاربری", TableEnum.ProductCategory);
        }
    }
}
