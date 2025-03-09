using System.Data;
using System.Diagnostics;

namespace Core.Model.EntityModel
{
    public class CommentModel : BaseEntityModel
    {
        public UserModel User { get; set; } = new UserModel();
        public ProductModel Product { get; set; } = new ProductModel();
        public short CRate { get; set; } = default;
        public string CDescription { get; set; } = string.Empty;
        public CommentModel() { }
        public CommentModel(DataRow dr)
        {
            User = new UserModel(dr);
            Product = new ProductModel(dr);

            CRate = dr[CRate].ConvertToShort();
            CDescription = dr[CDescription].ConvertToString();
            base.InitBaseEntityModel(dr);
        }
    }
}
