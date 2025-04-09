using Core;
using Core.Model;
using DataBase;
using Microsoft.AspNetCore.Http.Authentication;
using Service.ServiceInterface;
using System.Data;
using System.Security.Claims;

namespace Service
{
    public class UserService : IBaseNormalService<UserModel>
    {
        public static List<string> MainColumns = new List<string>()
        {
            Dictionary.User.SID.FullDBName,
            Dictionary.User.UTSID.FullDBName,
            Dictionary.User.UName.FullDBName,
            Dictionary.User.UFirstName.FullDBName,
            Dictionary.User.ULastName.FullDBName,
            Dictionary.User.UEmail.FullDBName,
            Dictionary.User.UTel.FullDBName,
            Dictionary.User.UPass.FullDBName
        };
        public static List<string> DefaultColumns = new List<string>()
        {
            Dictionary.User.Blocked.FullDBName,
            Dictionary.User.Deleted.FullDBName,

            Dictionary.User.CreationUSID.FullDBName,
            Dictionary.User.CreationDate.FullDBName,
            Dictionary.User.CreationTime.FullDBName,

            Dictionary.User.LastModifiedUSID.FullDBName,
            Dictionary.User.LastModifiedDate.FullDBName,
            Dictionary.User.LastModifiedTime.FullDBName
        };
        //------------------------------------------
        public async Task<bool> AddAsync(UserModel entity)
        {
            entity.SaveAddParameters();
            bool Ans = await DBConnection.ExecProcedureAsync("[User.Add]", entity.Parameters);
            return Ans;
        }
        public async Task<bool> EditAsync(UserModel entity)
        {
            entity.SaveEditParameters();
            bool Ans = await DBConnection.ExecProcedureAsync("[User.Edit]", entity.Parameters);
            return Ans;
        }
        public async Task BlockAsync(long SID)
        {
            UserModel user = new UserModel();
            user.SaveBlockedParameter(SID);
            await DBConnection.ExecProcedureAsync("[User.Block]", user.Parameters);
        }
        public async Task DeleteAsync(long SID)
        {
            UserModel user = new UserModel();
            user.SaveBlockedParameter(SID);
            await DBConnection.ExecProcedureAsync("[User.Delete]", user.Parameters);
        }
        //------------------------------------------
        public async Task<UserModel> GetModelSimpleAsync(long SID)
        {
            QueryBuilder qb = GetSimple();
            qb.AddEqualCondition(Dictionary.User.SID.FullDBName, SID);

            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            UserModel user = new UserModel(dr);

            return user;
        }
        public async Task<UserModel> GetModelWithRelatedEntitiesAsync(long SID)
        {
            QueryBuilder qb = GetWithRelatedEntities();
            qb.AddEqualCondition(Dictionary.User.SID.FullDBName, SID);
            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());

            UserModel user = new UserModel(dr);
            user.UserType = new UserTypeModel(dr);

            return user;
        }

        public QueryBuilder GetSimple()
        {
            QueryBuilder qb = new QueryBuilder();
            qb.AddColumns(MainColumns);
            qb.AddColumns(DefaultColumns);
            qb.SetTable(Dictionary.User.TableName);
            return qb;
        }
        public QueryBuilder GetWithRelatedEntities()
        {
            QueryBuilder qb = GetSimple();
            qb.AddColumns(UserTypeService.MainColumns);
            qb.AddLeftJoin(Dictionary.UserType.TableName, qb => { qb.AddEqualCondition(Dictionary.UserType.SID.FullDBName, Dictionary.User.UTSID.FullDBName); });
            return qb;
        }
        //------------------------------------------
        public List<UserModel> MapDTToModel(DataTable dt)
        {
            List<UserModel> list = new List<UserModel>();
            foreach (DataRow dr in dt.Rows)
            {
                UserModel User = new UserModel(dr);
                list.Add(User);
            }
            return list;
        }
        //------------------------------------------
        public async Task<DataRow> Login(UserModel userModel)
        {
            QueryBuilder qb = GetSimple();
            qb.AddEqualCondition(Dictionary.User.UTel.FullDBName, userModel.UTel.SetSingleQuotes());
            qb.AddEqualCondition(Dictionary.User.UPass.FullDBName, userModel.UPass.SetSingleQuotes());
            DataRow dr = await DBConnection.GetDataRowAsync(qb);
            return dr;
        }
        public ClaimsPrincipal CreateCookie(UserModel userModel, string cookieName)
        {
            List<Claim> Claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userModel.UName),
                new Claim(ClaimTypes.NameIdentifier, userModel.SID.ConvertToString()),
                new Claim(ClaimTypes.Role, userModel.UserType.SID.ConvertToString()),
                new Claim(nameof(userModel.UTel), userModel.UTel)
            };

            ClaimsIdentity Identity = new ClaimsIdentity(Claims, cookieName);// ایجاد ClaimsIdentity (نماینده اطلاعات هویتی)
            ClaimsPrincipal Principal = new ClaimsPrincipal(Identity);// ایجاد ClaimsPrincipal (نماینده کاربر)
            return Principal;
        }
        public string GenerateRandomUsername()
        {
            Random random = new Random();
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            int randomNumber = random.Next(1000, 9999);

            string username = $"{timestamp}{randomNumber}";
            return username;
        }
        //------------------------------------------
    }
}
