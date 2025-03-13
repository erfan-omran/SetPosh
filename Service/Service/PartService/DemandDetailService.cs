using Core;
using Core.Model;
using DataBase;
using Service.ServiceInterface;
using System.Data;

namespace Service.Service
{
    public class DemandDetailService : IBasePartService<DemandDetailModel>
    {
        public static List<string> MainColumns = new List<string>()
        {
            Dictionary.DemandDetail.ID.FullDBName,
            Dictionary.DemandDetail.DSID.FullDBName,
            Dictionary.DemandDetail.PSID.FullDBName,
            Dictionary.DemandDetail.DDCount.FullDBName,
            Dictionary.DemandDetail.DDPrice.FullDBName,

            Dictionary.DemandDetail.Blocked.FullDBName,
            Dictionary.DemandDetail.Deleted.FullDBName,

            Dictionary.DemandDetail.CreationUSID.FullDBName,
            Dictionary.DemandDetail.CreationDate.FullDBName,
            Dictionary.DemandDetail.CreationTime.FullDBName,

            Dictionary.DemandDetail.LastModifiedUSID.FullDBName,
            Dictionary.DemandDetail.LastModifiedDate.FullDBName,
            Dictionary.DemandDetail.LastModifiedTime.FullDBName
        };
        //------------------------------------------
        public async Task<bool> AddAsync(DemandDetailModel entity)
        {
            entity.SaveAddParameters();
            bool Added = await DBConnection.ExecProcedureAsync("[DemandDetail.Add]", entity.Parameters);
            return Added;
        }
        public async Task<bool> EditAsync(DemandDetailModel entity)
        {
            entity.SaveEditParameters();
            bool Edited = await DBConnection.ExecProcedureAsync("[DemandDetail.Edit]", entity.Parameters);
            return Edited;
        }
        public async Task BlockAsync(long SID)
        {
            DemandDetailModel DemandDetail = new DemandDetailModel();
            DemandDetail.SaveBlockedParameter(SID);
            await DBConnection.ExecProcedureAsync("[DemandDetail.Block]", DemandDetail.Parameters);
        }
        public async Task DeleteAsync(long SID)
        {
            DemandDetailModel DemandDetail = new DemandDetailModel();
            DemandDetail.SaveBlockedParameter(SID);
            await DBConnection.ExecProcedureAsync("[DemandDetail.Delete]", DemandDetail.Parameters);
        }
        //------------------------------------------
        public async Task<DemandDetailModel> GetSimpleModelAsync(long SID)
        {
            QueryBuilder qb = GetSimple();
            qb.AddEqualCondition(Dictionary.DemandDetail.ID.FullDBName, SID);

            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            DemandDetailModel DemandDetail = new DemandDetailModel(dr);

            return DemandDetail;
        }
        public async Task<DemandDetailModel> GetModelWithRelatedEntitiesAsync(long SID)
        {
            QueryBuilder qb = GetWithRelatedEntities();
            qb.AddEqualCondition(Dictionary.DemandDetail.ID.FullDBName, SID);

            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            DemandDetailModel DemandDetail = new DemandDetailModel(dr);

            return DemandDetail;
        }

        public QueryBuilder GetSimple()
        {
            QueryBuilder qb = new QueryBuilder();
            qb.AddColumns(MainColumns);
            qb.SetTable(Dictionary.DemandDetail.TableName);
            return qb;
        }
        public QueryBuilder GetWithRelatedEntities()
        {
            QueryBuilder qb = GetSimple();
            qb.AddColumns(ShoppingCartService.MainColumns);
            qb.AddColumns(ProductService.MainColumns);

            qb.AddLeftJoin(Dictionary.Demand.TableName, qb => { qb.AddEqualCondition(Dictionary.Demand.SID.FullDBName, Dictionary.DemandDetail.DSID.FullDBName); });
            qb.AddLeftJoin(Dictionary.Product.TableName, qb => { qb.AddEqualCondition(Dictionary.Product.SID.FullDBName, Dictionary.DemandDetail.PSID.FullDBName); });
            return qb;
        }
        //------------------------------------------
        public List<DemandDetailModel> MapDTToModel(DataTable dt)
        {
            List<DemandDetailModel> list = new List<DemandDetailModel>();
            foreach (DataRow dr in dt.Rows)
            {
                DemandDetailModel DemandDetail = new DemandDetailModel(dr);
                list.Add(DemandDetail);
            }
            return list;
        }
    }
}
