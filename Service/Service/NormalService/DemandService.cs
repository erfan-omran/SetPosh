using Core;
using Core.Model;
using DataBase;
using Service.ServiceInterface;
using System.Data;

namespace Service
{
    public class DemandService : IBaseNormalService<DemandModel>
    {
        public static List<string> MainColumns = new List<string>()
        {
            Dictionary.Demand.SID.FullDBName,
            Dictionary.Demand.SCSID.FullDBName,
            Dictionary.Demand.DSSID.FullDBName,
            Dictionary.Demand.USID.FullDBName,
            Dictionary.Demand.DeliveryDate.FullDBName,
            Dictionary.Demand.Confirmed.FullDBName,
        };
        public static List<string> DefaultColumns = new List<string>()
        {
            Dictionary.Demand.Blocked.FullDBName,
            Dictionary.Demand.Deleted.FullDBName,

            Dictionary.Demand.CreationUSID.FullDBName,
            Dictionary.Demand.CreationDate.FullDBName,
            Dictionary.Demand.CreationTime.FullDBName,

            Dictionary.Demand.LastModifiedUSID.FullDBName,
            Dictionary.Demand.LastModifiedDate.FullDBName,
            Dictionary.Demand.LastModifiedTime.FullDBName
        };
        //------------------------------------------
        public async Task<bool> AddAsync(DemandModel entity)
        {
            entity.SaveAddParameters();
            bool Added = await DBConnection.ExecProcedureAsync("[Demand.Add]", entity.Parameters);
            return Added;
        }
        public async Task<bool> EditAsync(DemandModel entity)
        {
            entity.SaveEditParameters();
            bool Edited = await DBConnection.ExecProcedureAsync("[Demand.Edit]", entity.Parameters);
            return Edited;
        }

        public async Task<bool> BlockAsync(long SID)
        {
            DemandModel Demand = new DemandModel();
            Demand.Blocked = true;
            Demand.SaveBlockedParameter(SID);
            bool Ans = await DBConnection.ExecProcedureAsync("[Demand.Block]", Demand.Parameters);
            return Ans;
        }
        public async Task<bool> UnBlockAsync(long SID)
        {
            DemandModel Demand = new DemandModel();
            Demand.Blocked = false;
            Demand.SaveBlockedParameter(SID);
            bool Ans = await DBConnection.ExecProcedureAsync("[Demand.Block]", Demand.Parameters);
            return Ans;
        }

        public async Task<bool> DeleteAsync(long SID)
        {
            DemandModel Demand = new DemandModel();
            Demand.Deleted = true;
            Demand.SaveBlockedParameter(SID);
            bool Ans = await DBConnection.ExecProcedureAsync("[Demand.Delete]", Demand.Parameters);
            return Ans;
        }
        public async Task<bool> UnDeleteAsync(long SID)
        {
            DemandModel Demand = new DemandModel();
            Demand.Deleted = false;
            Demand.SaveBlockedParameter(SID);
            bool Ans = await DBConnection.ExecProcedureAsync("[Demand.Delete]", Demand.Parameters);
            return Ans;
        }
        //------------------------------------------
        public async Task<DemandModel> GetModelSimpleAsync(long SID)
        {
            QueryBuilder qb = GetSimple();
            qb.AddEqualCondition(Dictionary.Demand.SID.FullDBName, SID);

            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            DemandModel Demand = new DemandModel(dr);

            return Demand;
        }
        public async Task<DemandModel> GetModelWithRelatedEntitiesAsync(long SID)
        {
            QueryBuilder qb = GetWithRelatedEntities();
            qb.AddEqualCondition(Dictionary.Demand.SID.FullDBName, SID);
            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());

            DemandModel Demand = new DemandModel(dr);
            Demand.ShoppingCart = new ShoppingCartModel(dr);
            Demand.DemandStatus = new DemandStatusModel(dr);
            Demand.User = new UserModel(dr);

            return Demand;
        }

        public QueryBuilder GetSimple()
        {
            QueryBuilder qb = new QueryBuilder();
            qb.AddColumns(MainColumns);
            qb.AddColumns(DefaultColumns);
            qb.SetTable(Dictionary.Demand.TableName);
            return qb;
        }
        public QueryBuilder GetWithRelatedEntities()
        {
            QueryBuilder qb = GetSimple();
            qb.AddColumns(ShoppingCartService.MainColumns);
            qb.AddColumns(DemandStatusService.MainColumns);
            qb.AddColumns(UserService.MainColumns);

            qb.AddLeftJoin(Dictionary.ShoppingCart.TableName, qb => { qb.AddEqualCondition(Dictionary.ShoppingCart.SID.FullDBName, Dictionary.Demand.SCSID.FullDBName); });
            qb.AddLeftJoin(Dictionary.DemandStatus.TableName, qb => { qb.AddEqualCondition(Dictionary.DemandStatus.SID.FullDBName, Dictionary.Demand.DSSID.FullDBName); });
            qb.AddLeftJoin(Dictionary.User.TableName, qb => { qb.AddEqualCondition(Dictionary.User.SID.FullDBName, Dictionary.Demand.USID.FullDBName); });
            return qb;
        }
        //------------------------------------------
        public List<DemandModel> MapDTToModel(DataTable dt)
        {
            List<DemandModel> list = new List<DemandModel>();
            foreach (DataRow dr in dt.Rows)
            {
                DemandModel Demand = new DemandModel(dr);
                list.Add(Demand);
            }
            return list;
        }
    }
}
