using Core;
using Core.Model;
using DataBase;
using Service.ServiceInterface;
using System.Data;

namespace Service.Service
{
    public class ShoppingCartDetailService : IBasePartService<ShoppingCartDetailModel>
    {
        public static List<string> MainColumns = new List<string>()
        {
            Dictionary.ShoppingCartDetail.ID.FullDBName,
            Dictionary.ShoppingCartDetail.SCSID.FullDBName,
            Dictionary.ShoppingCartDetail.PSID.FullDBName,
            Dictionary.ShoppingCartDetail.SCDCount.FullDBName,

            Dictionary.ShoppingCartDetail.Blocked.FullDBName,
            Dictionary.ShoppingCartDetail.Deleted.FullDBName,

            Dictionary.ShoppingCartDetail.CreationUSID.FullDBName,
            Dictionary.ShoppingCartDetail.CreationDate.FullDBName,
            Dictionary.ShoppingCartDetail.CreationTime.FullDBName,

            Dictionary.ShoppingCartDetail.LastModifiedUSID.FullDBName,
            Dictionary.ShoppingCartDetail.LastModifiedDate.FullDBName,
            Dictionary.ShoppingCartDetail.LastModifiedTime.FullDBName
        };
        //------------------------------------------
        public async Task<bool> AddAsync(ShoppingCartDetailModel entity)
        {
            entity.SaveAddParameters();
            bool Added = await DBConnection.ExecProcedureAsync("[ShoppingCartDetail.Add]", entity.Parameters);
            return Added;
        }
        public async Task<bool> EditAsync(ShoppingCartDetailModel entity)
        {
            entity.SaveEditParameters();
            bool Edited = await DBConnection.ExecProcedureAsync("[ShoppingCartDetail.Edit]", entity.Parameters);
            return Edited;
        }
        public async Task BlockAsync(long SID)
        {
            ShoppingCartDetailModel ShoppingCartDetail = new ShoppingCartDetailModel();
            ShoppingCartDetail.SaveBlockedParameter(SID);
            await DBConnection.ExecProcedureAsync("[ShoppingCartDetail.Block]", ShoppingCartDetail.Parameters);
        }
        public async Task DeleteAsync(long SID)
        {
            ShoppingCartDetailModel ShoppingCartDetail = new ShoppingCartDetailModel();
            ShoppingCartDetail.SaveBlockedParameter(SID);
            await DBConnection.ExecProcedureAsync("[ShoppingCartDetail.Delete]", ShoppingCartDetail.Parameters);
        }
        //------------------------------------------
        public async Task<ShoppingCartDetailModel> GetSimpleModelAsync(long SID)
        {
            QueryBuilder qb = GetSimple();
            qb.AddEqualCondition(Dictionary.ShoppingCartDetail.ID.FullDBName, SID);

            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            ShoppingCartDetailModel ShoppingCartDetail = new ShoppingCartDetailModel(dr);

            return ShoppingCartDetail;
        }
        public async Task<ShoppingCartDetailModel> GetModelWithRelatedEntitiesAsync(long SID)
        {
            QueryBuilder qb = GetWithRelatedEntities();
            qb.AddEqualCondition(Dictionary.ShoppingCartDetail.ID.FullDBName, SID);

            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            ShoppingCartDetailModel ShoppingCartDetail = new ShoppingCartDetailModel(dr);

            return ShoppingCartDetail;
        }

        public QueryBuilder GetSimple()
        {
            QueryBuilder qb = new QueryBuilder();
            qb.AddColumns(MainColumns);
            qb.SetTable(Dictionary.ShoppingCartDetail.TableName);
            return qb;
        }
        public QueryBuilder GetWithRelatedEntities()
        {
            QueryBuilder qb = GetSimple();
            qb.AddColumns(ShoppingCartService.MainColumns);
            qb.AddColumns(ProductService.MainColumns);

            qb.AddLeftJoin(Dictionary.ShoppingCart.TableName, qb => { qb.AddEqualCondition(Dictionary.ShoppingCart.SID.FullDBName, Dictionary.ShoppingCartDetail.SCSID.FullDBName); });
            qb.AddLeftJoin(Dictionary.Product.TableName, qb => { qb.AddEqualCondition(Dictionary.Product.SID.FullDBName, Dictionary.ShoppingCartDetail.PSID.FullDBName); });
            return qb;
        }
        //------------------------------------------
        public List<ShoppingCartDetailModel> MapDTToModel(DataTable dt)
        {
            List<ShoppingCartDetailModel> list = new List<ShoppingCartDetailModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ShoppingCartDetailModel ShoppingCartDetail = new ShoppingCartDetailModel(dr);
                list.Add(ShoppingCartDetail);
            }
            return list;
        }
    }
}
