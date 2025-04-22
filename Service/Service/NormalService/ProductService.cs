using Core;
using Core.Model;
using DataBase;
using Service.ServiceInterface;
using System.Collections.Generic;
using System.Data;

namespace Service
{
    public class ProductService : IBaseNormalService<ProductModel>
    {
        private readonly CommentService _commentService = new CommentService();

        public static List<string> MainColumns = new List<string>()
        {
            Dictionary.Product.SID.FullDBName,
            Dictionary.Product.PCSID.FullDBName,
            Dictionary.Product.PName.FullDBName,
            Dictionary.Product.PPrice.FullDBName,
            Dictionary.Product.PCount.FullDBName,
            Dictionary.Product.PDescription.FullDBName
        };
        public static List<string> DefaultColumns = new List<string>()
        {
            Dictionary.Product.Blocked.FullDBName,
            Dictionary.Product.Deleted.FullDBName,

            Dictionary.Product.CreationUSID.FullDBName,
            Dictionary.Product.CreationDate.FullDBName,
            Dictionary.Product.CreationTime.FullDBName,

            Dictionary.Product.LastModifiedUSID.FullDBName,
            Dictionary.Product.LastModifiedDate.FullDBName,
            Dictionary.Product.LastModifiedTime.FullDBName
        };
        //------------------------------------------
        public async Task<bool> AddAsync(ProductModel entity)
        {
            entity.SaveAddParameters();
            bool Added = await DBConnection.ExecProcedureAsync("[Product.Add]", entity.Parameters);
            return Added;
        }
        public async Task<bool> EditAsync(ProductModel entity)
        {
            entity.SaveEditParameters();
            bool Edited = await DBConnection.ExecProcedureAsync("[Product.Edit]", entity.Parameters);
            return Edited;
        }
        public async Task BlockAsync(long SID)
        {
            ProductModel Product = new ProductModel();
            Product.SaveBlockedParameter(SID);
            await DBConnection.ExecProcedureAsync("[Product.Block]", Product.Parameters);
        }
        public async Task DeleteAsync(long SID)
        {
            ProductModel Product = new ProductModel();
            Product.SaveBlockedParameter(SID);
            await DBConnection.ExecProcedureAsync("[Product.Delete]", Product.Parameters);
        }
        //------------------------------------------
        public async Task<ProductModel> GetModelSimpleAsync(long SID)
        {
            QueryBuilder qb = GetSimple();
            qb.AddEqualCondition(Dictionary.Product.SID.FullDBName, SID);
            qb.AddEqualCondition(Dictionary.Product.Blocked.FullDBName, 0);
            qb.AddEqualCondition(Dictionary.Product.Deleted.FullDBName, 0);

            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            ProductModel Product = new ProductModel(dr);

            return Product;
        }
        public async Task<ProductModel> GetModelWithAVGRateAsync(long SID)
        {
            QueryBuilder qb = GetSimple();
            qb.AddColumn(
                SqlFunction.IsNull(
                    SqlFunction.Avg(SqlFunction.Cast(Dictionary.Comment.CRate.FullDBName, "DECIMAL(3,2)")
                    ), 0)
                , "RateAVG");
            qb.AddLeftJoin(Dictionary.Comment.TableName, qb =>
            {
                qb.AddEqualCondition(Dictionary.Comment.PSID.FullDBName, Dictionary.Product.SID.FullDBName);
            });

            qb.AddEqualCondition(Dictionary.Product.SID.FullDBName, SID);
            qb.AddEqualCondition(Dictionary.Product.Blocked.FullDBName, 0);
            qb.AddEqualCondition(Dictionary.Product.Deleted.FullDBName, 0);

            qb.AddGroupBy(MainColumns);
            qb.AddGroupBy(DefaultColumns);
            
            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            ProductModel Product = new ProductModel(dr);
            Product.RateAVG = dr.GetValueOfDecimalColumn("RateAVG");
            return Product;
        }
        public async Task<ProductModel> GetModelWithRelatedEntitiesAsync(long SID)
        {
            QueryBuilder qb = GetWithRelatedEntities();
            qb.AddEqualCondition(Dictionary.Product.SID.FullDBName, SID);
            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());

            ProductModel Product = new ProductModel(dr);
            Product.ProductCategory = new ProductCategoryModel(dr);

            return Product;
        }
        public async Task<ProductModel> GetWithDetailsAsync(long SID)
        {
            ProductModel Product = await GetModelWithAVGRateAsync(SID);
            List<ProductImageModel> ProductImages = await GetProductImagesAsync(SID);
            List<CommentModel> Comments = await _commentService.GetProductCommentsAsync(SID);

            Product.ProductImages.AddRange(ProductImages);
            Product.ProductComments.AddRange(Comments);

            return Product;
        }
        public async Task<List<ProductImageModel>> GetProductImagesAsync(long PSID)
        {
            QueryBuilder qb = new QueryBuilder();
            qb.AddColumns(ProductImageService.MainColumns);
            qb.SetTable(Dictionary.ProductImage.TableName);
            qb.AddEqualCondition(Dictionary.ProductImage.PSID.FullDBName, PSID);

            DataTable dt = await DBConnection.GetDataTableAsync(qb.CreateQuery());
            List<ProductImageModel> productImageList = new List<ProductImageModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ProductImageModel productImage = new ProductImageModel(dr);
                productImageList.Add(productImage);
            }

            return productImageList;
        }
        public async Task<List<ProductModel>> GetRelatedProducts(long PSID, long PCSID)
        {
            QueryBuilder qb = GetSimple();
            qb.AddNotEqualCondition(Dictionary.Product.SID.FullDBName, PSID);
            qb.AddEqualCondition(Dictionary.Product.PCSID.FullDBName, PCSID);
            DataTable dt = await DBConnection.GetDataTableAsync(qb.CreateQuery());

            List<ProductModel> Products = new List<ProductModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ProductModel product = new ProductModel(dr);
                Products.Add(product);
            }

            return Products;
        }
        public async Task<List<ProductModel>> GetProductsWithFilter(string? searchText, string? PCSID, decimal? minPrice, decimal? maxPrice, int? inStock, int? isBlocked)
        {
            QueryBuilder ProductQB = GetWithMainImage();

            if (!string.IsNullOrEmpty(searchText))
                ProductQB.AddLikeCondition(Dictionary.Product.PName.FullDBName, searchText);
            if (!string.IsNullOrEmpty(PCSID))
                ProductQB.AddEqualCondition(Dictionary.Product.PCSID.FullDBName, PCSID);
            if (minPrice.HasValue)
                ProductQB.AddGreaterThanOrEqualCondition(Dictionary.Product.PPrice.FullDBName, minPrice);
            if (maxPrice.HasValue)
                ProductQB.AddLessThanOrEqualCondition(Dictionary.Product.PPrice.FullDBName, maxPrice);
            //inStock ToDo
            if (isBlocked >= 0)
                ProductQB.AddEqualCondition(Dictionary.Product.Blocked.FullDBName, isBlocked);

            string ProductQuery = ProductQB.CreateQuery();
            DataTable ProductDT = await DBConnection.GetDataTableAsync(ProductQuery);
            List<ProductModel> ProductList = MapDTToModel(ProductDT, true);

            return ProductList;
        }

        public QueryBuilder GetSimple()
        {
            QueryBuilder qb = new QueryBuilder();
            qb.AddColumns(MainColumns);
            qb.AddColumns(DefaultColumns);
            qb.SetTable(Dictionary.Product.TableName);
            return qb;
        }
        public QueryBuilder GetWithMainImage()
        {
            QueryBuilder qb = GetSimple();
            qb.AddColumns(
                Dictionary.ProductImage.ID.FullDBName,
                Dictionary.ProductImage.ImgName.FullDBName,
                Dictionary.ProductImage.IsMain.FullDBName
            );
            qb.AddLeftJoin(Dictionary.ProductImage.TableName, qb =>
            {
                qb.AddEqualCondition(Dictionary.ProductImage.PSID.FullDBName, Dictionary.Product.SID.FullDBName);
                qb.AddEqualCondition(Dictionary.ProductImage.IsMain.FullDBName, 1);
            });
            return qb;
        }
        public QueryBuilder GetWithRelatedEntities()
        {
            QueryBuilder qb = GetSimple();
            qb.AddColumns(ProductCategoryService.MainColumns);
            qb.AddLeftJoin(Dictionary.ProductCategory.TableName, qb => { qb.AddEqualCondition(Dictionary.ProductCategory.SID.FullDBName, Dictionary.Product.PCSID.FullDBName); });
            return qb;
        }
        //------------------------------------------
        public List<ProductModel> MapDTToModel(DataTable dt)
        {
            List<ProductModel> list = new List<ProductModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ProductModel Product = new ProductModel(dr);
                list.Add(Product);
            }
            return list;
        }
        public List<ProductModel> MapDTToModel(DataTable dt, bool hasMainImg)
        {
            List<ProductModel> list = new List<ProductModel>();
            if (hasMainImg)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ProductModel Product = new ProductModel(dr);
                    ProductImageModel ProductImage = new ProductImageModel(dr);
                    Product.ProductImages.Add(ProductImage);
                    list.Add(Product);
                }
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ProductModel Product = new ProductModel(dr);
                    list.Add(Product);
                }
            }
            return list;
        }
    }
}
