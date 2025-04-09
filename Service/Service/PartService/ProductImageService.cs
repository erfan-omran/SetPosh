using Core;
using Core.Model;
using DataBase;
using Service.ServiceInterface;
using System.Data;

namespace Service
{
    public class ProductImageService : IBasePartService<ProductImageModel>
    {
        public static List<string> MainColumns = new List<string>()
        {
            Dictionary.ProductImage.ID.FullDBName,
            Dictionary.ProductImage.PSID.FullDBName,
            Dictionary.ProductImage.ImgName.FullDBName,
            Dictionary.ProductImage.IsMain.FullDBName
        };
        public static List<string> DefaultColumns = new List<string>()
        {
            Dictionary.ProductImage.Blocked.FullDBName,
            Dictionary.ProductImage.Deleted.FullDBName,

            Dictionary.ProductImage.CreationUSID.FullDBName,
            Dictionary.ProductImage.CreationDate.FullDBName,
            Dictionary.ProductImage.CreationTime.FullDBName,

            Dictionary.ProductImage.LastModifiedUSID.FullDBName,
            Dictionary.ProductImage.LastModifiedDate.FullDBName,
            Dictionary.ProductImage.LastModifiedTime.FullDBName
        };
        //------------------------------------------
        public async Task<bool> AddAsync(ProductImageModel entity)
        {
            //entity.SaveMainParameters(true);
            //bool Added = await DBConnection.ExecProcedureAsync("[ProductImage.Add]", entity.Parameters);
            //return Added;
            return true;
        }
        public async Task<bool> EditAsync(ProductImageModel entity)
        {
            //entity.SaveMainParameters(false);
            //bool Edited = await DBConnection.ExecProcedureAsync("[ProductImage.Edit]", entity.Parameters);
            //return Edited;
            return true;
        }
        public async Task BlockAsync(long SID)
        {
            //ProductImageModel ProductImage = new ProductImageModel();
            //ProductImage.SaveBlockedParameter(SID);
            //await DBConnection.ExecProcedureAsync("[ProductImage.Block]", ProductImage.Parameters);
        }
        public async Task DeleteAsync(long SID)
        {
            //ProductImageModel ProductImage = new ProductImageModel();
            //ProductImage.SaveBlockedParameter(SID);
            //await DBConnection.ExecProcedureAsync("[ProductImage.Delete]", ProductImage.Parameters);
        }
        //------------------------------------------
        public async Task<ProductImageModel> GetModelSimpleAsync(long SID)
        {
            QueryBuilder qb = GetSimple();
            qb.AddEqualCondition(Dictionary.ProductImage.ID.FullDBName, SID);

            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            ProductImageModel ProductImage = new ProductImageModel(dr);

            return ProductImage;
        }
        public async Task<ProductImageModel> GetModelWithRelatedEntitiesAsync(long SID)
        {
            QueryBuilder qb = GetWithRelatedEntities();
            qb.AddEqualCondition(Dictionary.ProductImage.ID.FullDBName, SID);

            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            ProductImageModel ProductImage = new ProductImageModel(dr);

            return ProductImage;
        }

        public QueryBuilder GetSimple()
        {
            QueryBuilder qb = new QueryBuilder();
            qb.AddColumns(MainColumns);
            qb.AddColumns(DefaultColumns);
            qb.SetTable(Dictionary.ProductImage.TableName);
            return qb;
        }
        public QueryBuilder GetWithRelatedEntities()
        {
            QueryBuilder qb = GetSimple();
            //qb.AddColumns(ShoppingCartService.MainColumns);
            //qb.AddColumns(ProductService.MainColumns);

            //qb.AddLeftJoin(Dictionary.Demand.TableName, qb => { qb.AddEqualCondition(Dictionary.Demand.SID.FullDBName, Dictionary.ProductImage.DSID.FullDBName); });
            //qb.AddLeftJoin(Dictionary.Product.TableName, qb => { qb.AddEqualCondition(Dictionary.Product.SID.FullDBName, Dictionary.ProductImage.PSID.FullDBName); });
            return qb;
        }
        //------------------------------------------
        public List<ProductImageModel> MapDTToModel(DataTable dt)
        {
            List<ProductImageModel> list = new List<ProductImageModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ProductImageModel ProductImage = new ProductImageModel(dr);
                list.Add(ProductImage);
            }
            return list;
        }
    }
}
