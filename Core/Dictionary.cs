using Core.Schema;

namespace Core
{
    public static class Dictionary
    {
        public static CommentSchema Comment = new CommentSchema();
        public static DemandSchema Demand = new DemandSchema();
        public static DemandDetailSchema DemandDetail = new DemandDetailSchema();
        public static DemandStatusSchema DemandStatus = new DemandStatusSchema();
        public static ProductSchema Product = new ProductSchema();
        public static ProductCategorySchema ProductCategory = new ProductCategorySchema();
        public static ShoppingCartSchema ShoppingCart = new ShoppingCartSchema();
        public static ShoppingCartDetailSchema ShoppingCartDetail = new ShoppingCartDetailSchema();
        public static UserSchema User = new UserSchema();
        public static UserTypeSchema UserType = new UserTypeSchema();
    }
}
