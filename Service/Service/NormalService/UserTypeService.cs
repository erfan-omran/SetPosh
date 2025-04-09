using Core;
using Core.Model;
using DataBase;
using Service.ServiceInterface;
using System.Data;

namespace Service
{
    public class UserTypeService : IBaseNormalService<UserTypeModel>
    {
        public static List<string> MainColumns = new List<string>()
        {
            Dictionary.UserType.SID.FullDBName,
            Dictionary.UserType.UTName.FullDBName,
            Dictionary.UserType.UTDescription.FullDBName
        };
        public static List<string> DefaultColumns= new List<string>()
        {
            Dictionary.UserType.Blocked.FullDBName,
            Dictionary.UserType.Deleted.FullDBName,

            Dictionary.UserType.CreationUSID.FullDBName,
            Dictionary.UserType.CreationDate.FullDBName,
            Dictionary.UserType.CreationTime.FullDBName,

            Dictionary.UserType.LastModifiedUSID.FullDBName,
            Dictionary.UserType.LastModifiedDate.FullDBName,
            Dictionary.UserType.LastModifiedTime.FullDBName
        };
        //------------------------------------------
        public async Task<bool> AddAsync(UserTypeModel entity)
        {
            entity.SaveAddParameters();
            bool Added = await DBConnection.ExecProcedureAsync("[UserType.Add]", entity.Parameters);
            return Added;
        }
        public async Task<bool> EditAsync(UserTypeModel entity)
        {
            entity.SaveEditParameters();
            bool Edited = await DBConnection.ExecProcedureAsync("[UserType.Edit]", entity.Parameters);
            return Edited;
        }
        public async Task BlockAsync(long SID)
        {
            UserTypeModel UserType = new UserTypeModel();
            UserType.SaveBlockedParameter(SID);
            await DBConnection.ExecProcedureAsync("[UserType.Block]", UserType.Parameters);
        }
        public async Task DeleteAsync(long SID)
        {
            UserTypeModel UserType = new UserTypeModel();
            UserType.SaveBlockedParameter(SID);
            await DBConnection.ExecProcedureAsync("[UserType.Delete]", UserType.Parameters);
        }
        //------------------------------------------
        public async Task<UserTypeModel> GetModelSimpleAsync(long SID)
        {
            QueryBuilder qb = GetSimple();
            qb.AddEqualCondition(Dictionary.UserType.SID.FullDBName, SID);

            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            UserTypeModel UserType = new UserTypeModel(dr);

            return UserType;
        }
        public async Task<UserTypeModel> GetModelWithRelatedEntitiesAsync(long SID)
        {
            QueryBuilder qb = GetWithRelatedEntities();
            qb.AddEqualCondition(Dictionary.UserType.SID.FullDBName, SID);

            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            UserTypeModel UserType = new UserTypeModel(dr);

            return UserType;
        }

        public QueryBuilder GetSimple()
        {
            QueryBuilder qb = new QueryBuilder();
            qb.AddColumns(MainColumns);
            qb.AddColumns(DefaultColumns);
            qb.SetTable(Dictionary.UserType.TableName);
            return qb;
        }
        public QueryBuilder GetWithRelatedEntities()
        {
            QueryBuilder qb = GetSimple();
            return qb;
        }
        //------------------------------------------
        public List<UserTypeModel> MapDTToModel(DataTable dt)
        {
            List<UserTypeModel> list = new List<UserTypeModel>();
            foreach (DataRow dr in dt.Rows)
            {
                UserTypeModel UserType = new UserTypeModel(dr);
                list.Add(UserType);
            }
            return list;
        }
    }
}
