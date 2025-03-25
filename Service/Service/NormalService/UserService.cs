using Core;
using Core.Model;
using DataBase;
using Microsoft.AspNetCore.Http;
using Service.ServiceInterface;
using System.Data;
using System.Security.Claims;

namespace Service.Service
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
            Dictionary.User.UPass.FullDBName,

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
            bool Added = await DBConnection.ExecProcedureAsync("[User.Add]", entity.Parameters);
            return Added;
        }
        public async Task<bool> EditAsync(UserModel entity)
        {
            entity.SaveEditParameters();
            bool Edited = await DBConnection.ExecProcedureAsync("[User.Edit]", entity.Parameters);
            return Edited;
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
        public async Task<UserModel> GetSimpleModelAsync(long SID)
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
            //user.UserType = new UserTypeModel(dr);

            return user;
        }

        public QueryBuilder GetSimple()
        {
            QueryBuilder qb = new QueryBuilder();
            qb.AddColumns(MainColumns);
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
        public ClaimsPrincipal CreateCookie(UserModel UserModel)
        {
            // اگر کاربر پیدا شد، Claims تعریف می‌شود
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, UserModel.UName), // نام کاربر
                new Claim(ClaimTypes.NameIdentifier, UserModel.SID.ToString()), // شناسه کاربر
                new Claim(ClaimTypes.Role, UserModel.UserType.SID.ToString()), // نقش کاربر
                new Claim(nameof(UserModel.UTel), UserModel.UTel) // شماره تلفن (دلخواه)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "SetPoshCookie");// ایجاد ClaimsIdentity (نماینده اطلاعات هویتی)
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);// ایجاد ClaimsPrincipal (نماینده کاربر)
            return claimsPrincipal;
        }
    }
}
