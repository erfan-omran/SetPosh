using Core;
using Core.Model;
using DataBase;
using Service.ServiceInterface;
using System.Data;

namespace Service
{
    public class Enum_UserTypeService : IBaseEnumService<Enum_UserTypeModel>
    {
        public static List<string> MainColumns = new List<string>()
        {
            Dictionary.UserType.ID.FullDBName,
            Dictionary.UserType.UTName.FullDBName,
            Dictionary.UserType.UTDescription.FullDBName
        };
        //------------------------------------------
        public async Task<Enum_UserTypeModel> GetModelSimpleAsync(long SID)
        {
            QueryBuilder qb = GetSimple();
            qb.AddEqualCondition(Dictionary.UserType.ID.FullDBName, SID);

            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            Enum_UserTypeModel UserType = new Enum_UserTypeModel(dr);

            return UserType;
        }

        public QueryBuilder GetSimple()
        {
            QueryBuilder qb = new QueryBuilder();
            qb.AddColumns(MainColumns);
            qb.SetTable(Dictionary.UserType.TableName);
            return qb;
        }
        //------------------------------------------
        public List<Enum_UserTypeModel> MapDTToModel(DataTable dt)
        {
            List<Enum_UserTypeModel> list = new List<Enum_UserTypeModel>();
            foreach (DataRow dr in dt.Rows)
            {
                Enum_UserTypeModel UserType = new Enum_UserTypeModel(dr);
                list.Add(UserType);
            }
            return list;
        }
    }
}
