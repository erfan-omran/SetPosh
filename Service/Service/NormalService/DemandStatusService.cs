using Core;
using Core.Model;
using DataBase;
using Service.ServiceInterface;
using System.Data;

namespace Service
{
    public class DemandStatusService : IBaseNormalService<DemandStatusModel>
    {
        public static List<string> MainColumns = new List<string>()
        {
            Dictionary.DemandStatus.SID.FullDBName,
            Dictionary.DemandStatus.DSName.FullDBName,
            Dictionary.DemandStatus.DSDescription.FullDBName
        };
        public static List<string> DefaultColumns = new List<string>()
        {
            Dictionary.DemandStatus.Blocked.FullDBName,
            Dictionary.DemandStatus.Deleted.FullDBName,

            Dictionary.DemandStatus.CreationUSID.FullDBName,
            Dictionary.DemandStatus.CreationDate.FullDBName,
            Dictionary.DemandStatus.CreationTime.FullDBName,

            Dictionary.DemandStatus.LastModifiedUSID.FullDBName,
            Dictionary.DemandStatus.LastModifiedDate.FullDBName,
            Dictionary.DemandStatus.LastModifiedTime.FullDBName
        };
        //------------------------------------------
        public async Task<bool> AddAsync(DemandStatusModel entity)
        {
            entity.SaveAddParameters();
            bool Added = await DBConnection.ExecProcedureAsync("[DemandStatus.Add]", entity.Parameters);
            return Added;
        }
        public async Task<bool> EditAsync(DemandStatusModel entity)
        {
            entity.SaveEditParameters();
            bool Edited = await DBConnection.ExecProcedureAsync("[DemandStatus.Edit]", entity.Parameters);
            return Edited;
        }
        public async Task BlockAsync(long SID)
        {
            DemandStatusModel DemandStatus = new DemandStatusModel();
            DemandStatus.SaveBlockedParameter(SID);
            await DBConnection.ExecProcedureAsync("[DemandStatus.Block]", DemandStatus.Parameters);
        }
        public async Task DeleteAsync(long SID)
        {
            DemandStatusModel DemandStatus = new DemandStatusModel();
            DemandStatus.SaveBlockedParameter(SID);
            await DBConnection.ExecProcedureAsync("[DemandStatus.Delete]", DemandStatus.Parameters);
        }
        //------------------------------------------
        public async Task<DemandStatusModel> GetModelSimpleAsync(long SID)
        {
            QueryBuilder qb = GetSimple();
            qb.AddEqualCondition(Dictionary.DemandStatus.SID.FullDBName, SID);

            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            DemandStatusModel DemandStatus = new DemandStatusModel(dr);

            return DemandStatus;
        }
        public async Task<DemandStatusModel> GetModelWithRelatedEntitiesAsync(long SID)
        {
            QueryBuilder qb = GetWithRelatedEntities();
            qb.AddEqualCondition(Dictionary.DemandStatus.SID.FullDBName, SID);

            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            DemandStatusModel DemandStatus = new DemandStatusModel(dr);

            return DemandStatus;
        }

        public QueryBuilder GetSimple()
        {
            QueryBuilder qb = new QueryBuilder();
            qb.AddColumns(MainColumns);
            qb.AddColumns(DefaultColumns);
            qb.SetTable(Dictionary.DemandStatus.TableName);
            return qb;
        }
        public QueryBuilder GetWithRelatedEntities()
        {
            QueryBuilder qb = GetSimple();
            return qb;
        }
        //------------------------------------------
        public List<DemandStatusModel> MapDTToModel(DataTable dt)
        {
            List<DemandStatusModel> list = new List<DemandStatusModel>();
            foreach (DataRow dr in dt.Rows)
            {
                DemandStatusModel DemandStatus = new DemandStatusModel(dr);
                list.Add(DemandStatus);
            }
            return list;
        }
    }
}
