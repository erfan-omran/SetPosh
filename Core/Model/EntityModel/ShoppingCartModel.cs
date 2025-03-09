using System.Data;

namespace Core.Model.EntityModel
{
    public class ShoppingCartModel : BaseEntityModel
    {
        public UserModel User { get; set; } = new UserModel();
        public bool Confirmed { get; set; } = default;

        public ShoppingCartModel() { }
        public ShoppingCartModel(DataRow dr)
        {
            User = new UserModel(dr);

            Confirmed = dr[nameof(Confirmed)].ConvertToBool();
            base.InitBaseEntityModel(dr);
        }
    }
}
