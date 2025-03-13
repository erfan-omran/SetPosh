using Core;
using Core.Model;
using DataBase;
using Service.ServiceInterface;
using System.Data;

namespace Service.Service
{
    public class ProductService : IBaseNormalService<ProductModel>
    {
        public static List<string> MainColumns = new List<string>()
        {
            Dictionary.Product.SID.FullDBName,
            Dictionary.Product.PCSID.FullDBName,
            Dictionary.Product.PName.FullDBName,
            Dictionary.Product.PPrice.FullDBName,
            Dictionary.Product.PCount.FullDBName,
            Dictionary.Product.PDescription.FullDBName,

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
        public async Task<ProductModel> GetSimpleModelAsync(long SID)
        {
            QueryBuilder qb = GetSimple();
            qb.AddEqualCondition(Dictionary.Product.SID.FullDBName, SID);

            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            ProductModel Product = new ProductModel(dr);

            return Product;
        }
        public async Task<ProductModel> GetModelWithRelatedEntitiesAsync(long SID)
        {
            QueryBuilder qb = GetWithRelatedEntities();
            qb.AddEqualCondition(Dictionary.Product.SID.FullDBName, SID);

            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            ProductModel Product = new ProductModel(dr);

            return Product;
        }

        public QueryBuilder GetSimple()
        {
            QueryBuilder qb = new QueryBuilder();
            qb.AddColumns(MainColumns);
            qb.SetTable(Dictionary.Product.TableName);
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
    }
}
