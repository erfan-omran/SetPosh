using DataBase.Columns;
using DataBase.Enum;

namespace Core.Schema.EntitySchema
{
    public class ShoppingCartSchema : BaseEntitySchema
    {
        public SIDColumn USID { get; private set; }
        public BoolColumn PName { get; private set; }
        public ShoppingCartSchema()
        {
            EntityName = TableEnum.ShoppingCart.ConvertToString();
            PersianName = "سبد خرید";
            TableName = $"[{EntityName}]";

            foreach (BaseColumn clm in BaseColumns)
                clm.SetTable(TableEnum.ShoppingCart);

            USID = new SIDColumn(nameof(USID), "کاربر", TableEnum.ShoppingCart);
            PName = new BoolColumn(nameof(PName), "تکمیل شده", TableEnum.ShoppingCart);
        }
    }
}
