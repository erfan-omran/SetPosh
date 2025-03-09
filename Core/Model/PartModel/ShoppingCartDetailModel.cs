using Core.Model.EntityModel;
using System.Data;

namespace Core.Model.PartModel
{
    public class ShoppingCartDetailModel : BasePartModel
    {
        public ShoppingCartModel ShoppingCart { get; set; } = new ShoppingCartModel();
        public ProductModel Product { get; set; } = new ProductModel();
        public long SCDCount { get; set; } = default;
        public ShoppingCartDetailModel() { }
        public ShoppingCartDetailModel(DataRow dr)
        {
            ShoppingCart = new ShoppingCartModel(dr);
            Product = new ProductModel(dr);

            SCDCount = dr[nameof(SCDCount)].ConvertToLong();
            base.InitBasePartModel(dr);
        }
    }
}
