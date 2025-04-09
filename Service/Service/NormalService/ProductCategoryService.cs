using Core;
using Core.Model;
using DataBase;
using Service.ServiceInterface;
using System.Data;

namespace Service
{
    public class ProductCategoryService : IBaseNormalService<ProductCategoryModel>
    {
        public static List<string> MainColumns = new List<string>()
        {
            Dictionary.ProductCategory.SID.FullDBName,
            //Dictionary.ProductCategory.PCSID.FullDBName,
            Dictionary.ProductCategory.PCName.FullDBName,
            Dictionary.ProductCategory.PCDescription.FullDBName
        };
        public static List<string> DefaultColumns = new List<string>()
        {
            Dictionary.ProductCategory.Blocked.FullDBName,
            Dictionary.ProductCategory.Deleted.FullDBName,

            Dictionary.ProductCategory.CreationUSID.FullDBName,
            Dictionary.ProductCategory.CreationDate.FullDBName,
            Dictionary.ProductCategory.CreationTime.FullDBName,

            Dictionary.ProductCategory.LastModifiedUSID.FullDBName,
            Dictionary.ProductCategory.LastModifiedDate.FullDBName,
            Dictionary.ProductCategory.LastModifiedTime.FullDBName
        };
        //------------------------------------------
        public async Task<bool> AddAsync(ProductCategoryModel entity)
        {
            entity.SaveAddParameters();
            bool Added = await DBConnection.ExecProcedureAsync("[ProductCategory.Add]", entity.Parameters);
            return Added;
        }
        public async Task<bool> EditAsync(ProductCategoryModel entity)
        {
            entity.SaveEditParameters();
            bool Edited = await DBConnection.ExecProcedureAsync("[ProductCategory.Edit]", entity.Parameters);
            return Edited;
        }
        public async Task BlockAsync(long SID)
        {
            ProductCategoryModel ProductCategory = new ProductCategoryModel();
            ProductCategory.SaveBlockedParameter(SID);
            await DBConnection.ExecProcedureAsync("[ProductCategory.Block]", ProductCategory.Parameters);
        }
        public async Task DeleteAsync(long SID)
        {
            ProductCategoryModel ProductCategory = new ProductCategoryModel();
            ProductCategory.SaveBlockedParameter(SID);
            await DBConnection.ExecProcedureAsync("[ProductCategory.Delete]", ProductCategory.Parameters);
        }
        //------------------------------------------
        public async Task<ProductCategoryModel> GetModelSimpleAsync(long SID)
        {
            QueryBuilder qb = GetSimple();
            qb.AddEqualCondition(Dictionary.ProductCategory.SID.FullDBName, SID);

            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            ProductCategoryModel ProductCategory = new ProductCategoryModel(dr);

            return ProductCategory;
        }
        public async Task<ProductCategoryModel> GetModelWithRelatedEntitiesAsync(long SID)
        {
            QueryBuilder qb = GetWithRelatedEntities();
            qb.AddEqualCondition(Dictionary.ProductCategory.SID.FullDBName, SID);

            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            ProductCategoryModel ProductCategory = new ProductCategoryModel(dr);

            return ProductCategory;
        }

        public QueryBuilder GetSimple()
        {
            QueryBuilder qb = new QueryBuilder();
            qb.AddColumns(MainColumns);
            qb.AddColumns(DefaultColumns);
            qb.SetTable(Dictionary.ProductCategory.TableName);
            return qb;
        }
        public QueryBuilder GetWithRelatedEntities()
        {
            QueryBuilder qb = GetSimple();
            //qb.AddColumns(ProductCategoryService.MainColumns);
            //qb.AddLeftJoin(Dictionary.ProductCategory.TableName, qb => { qb.AddEqualCondition(Dictionary.ProductCategory.SID.FullDBName, Dictionary.ProductCategory.PCSID.FullDBName); });
            return qb;
        }
        //------------------------------------------
        public List<ProductCategoryModel> MapDTToModel(DataTable dt)
        {
            List<ProductCategoryModel> list = new List<ProductCategoryModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ProductCategoryModel ProductCategory = new ProductCategoryModel(dr);
                list.Add(ProductCategory);
            }
            return list;
        }
    }
}
