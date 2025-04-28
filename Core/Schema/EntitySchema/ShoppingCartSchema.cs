using DataBase.Columns;
using Core.Enum;

namespace Core.Schema
{
    public class ShoppingCartSchema : BaseEntitySchema
    {
        public SIDColumn USID { get; private set; }
        public BoolColumn IsActive { get; private set; }
        public BoolColumn Confirmed { get; private set; }
        public ShoppingCartSchema()
        {
            EntityName = TableEnum.ShoppingCart.ConvertToString();
            PersianName = "سبد خرید";
            TableName = $"[{EntityName}]";

            foreach (BaseColumn clm in BaseColumns)
                clm.SetTable(TableEnum.ShoppingCart);

            USID = new SIDColumn(nameof(USID), "کاربر", TableEnum.ShoppingCart);
            IsActive = new BoolColumn(nameof(IsActive), "فعال", TableEnum.ShoppingCart);
            Confirmed = new BoolColumn(nameof(Confirmed), "تکمیل شده", TableEnum.ShoppingCart);
        }
    }
}
