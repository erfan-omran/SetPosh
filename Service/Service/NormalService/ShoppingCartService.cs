using Core;
using Core.Model;
using DataBase;
using Service.ServiceInterface;
using System.Data;
using System.Data.SqlClient;

namespace Service
{
    public class ShoppingCartService : IBaseNormalService<ShoppingCartModel>
    {
        public static List<string> MainColumns = new List<string>()
        {
            Dictionary.ShoppingCart.SID.FullDBName,
            Dictionary.ShoppingCart.USID.FullDBName,
            Dictionary.ShoppingCart.IsActive.FullDBName,
            Dictionary.ShoppingCart.Confirmed.FullDBName
        };
        public static List<string> DefaultColumns = new List<string>()
        {
            Dictionary.ShoppingCart.Blocked.FullDBName,
            Dictionary.ShoppingCart.Deleted.FullDBName,

            Dictionary.ShoppingCart.CreationUSID.FullDBName,
            Dictionary.ShoppingCart.CreationDate.FullDBName,
            Dictionary.ShoppingCart.CreationTime.FullDBName,

            Dictionary.ShoppingCart.LastModifiedUSID.FullDBName,
            Dictionary.ShoppingCart.LastModifiedDate.FullDBName,
            Dictionary.ShoppingCart.LastModifiedTime.FullDBName
        };
        //------------------------------------------
        public async Task<bool> AddAsync(ShoppingCartModel entity)
        {
            entity.SaveAddParameters();
            bool Added = await DBConnection.ExecProcedureAsync("[ShoppingCart.Add]", entity.Parameters);
            return Added;
        }
        public async Task<bool> EditAsync(ShoppingCartModel entity)
        {
            entity.SaveEditParameters();
            bool Edited = await DBConnection.ExecProcedureAsync("[ShoppingCart.Edit]", entity.Parameters);
            return Edited;
        }

        public async Task<bool> BlockAsync(long SID)
        {
            ShoppingCartModel ShoppingCart = new ShoppingCartModel();
            ShoppingCart.Blocked = true;
            ShoppingCart.SaveBlockedParameter(SID);
            bool Ans = await DBConnection.ExecProcedureAsync("[ShoppingCart.Block]", ShoppingCart.Parameters);
            return Ans;
        }
        public async Task<bool> UnBlockAsync(long SID)
        {
            ShoppingCartModel ShoppingCart = new ShoppingCartModel();
            ShoppingCart.Blocked = false;
            ShoppingCart.SaveBlockedParameter(SID);
            bool Ans = await DBConnection.ExecProcedureAsync("[ShoppingCart.Block]", ShoppingCart.Parameters);
            return Ans;
        }

        public async Task<bool> DeleteAsync(long SID)
        {
            ShoppingCartModel ShoppingCart = new ShoppingCartModel();
            ShoppingCart.Deleted = true;
            ShoppingCart.SaveBlockedParameter(SID);
            bool Ans = await DBConnection.ExecProcedureAsync("[ShoppingCart.Delete]", ShoppingCart.Parameters);
            return Ans;
        }
        public async Task<bool> UnDeleteAsync(long SID)
        {
            ShoppingCartModel ShoppingCart = new ShoppingCartModel();
            ShoppingCart.Deleted = false;
            ShoppingCart.SaveBlockedParameter(SID);
            bool Ans = await DBConnection.ExecProcedureAsync("[ShoppingCart.Delete]", ShoppingCart.Parameters);
            return Ans;
        }
        //------------------------------------------
        public async Task<ShoppingCartModel> GetModelSimpleAsync(long SID)
        {
            QueryBuilder qb = GetSimple();
            qb.AddEqualCondition(Dictionary.ShoppingCart.SID.FullDBName, SID);

            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            ShoppingCartModel ShoppingCart = new ShoppingCartModel(dr);

            return ShoppingCart;
        }
        public async Task<ShoppingCartModel> GetModelWithRelatedEntitiesAsync(long SID)
        {
            QueryBuilder qb = GetWithRelatedEntities();
            qb.AddEqualCondition(Dictionary.ShoppingCart.SID.FullDBName, SID);

            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            ShoppingCartModel ShoppingCart = new ShoppingCartModel(dr);
            ShoppingCart.User = new UserModel(dr);

            return ShoppingCart;
        }

        public QueryBuilder GetSimple()
        {
            QueryBuilder qb = new QueryBuilder();
            qb.AddColumns(MainColumns);
            qb.AddColumns(DefaultColumns);
            qb.SetTable(Dictionary.ShoppingCart.TableName);
            return qb;
        }
        public QueryBuilder GetWithRelatedEntities()
        {
            QueryBuilder qb = GetSimple();
            qb.AddColumns(UserService.MainColumns);
            qb.AddLeftJoin(Dictionary.User.TableName, qb => { qb.AddEqualCondition(Dictionary.User.SID.FullDBName, Dictionary.ShoppingCart.USID.FullDBName); });
            return qb;
        }
        //------------------------------------------
        public List<ShoppingCartModel> MapDTToModel(DataTable dt)
        {
            List<ShoppingCartModel> list = new List<ShoppingCartModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ShoppingCartModel ShoppingCart = new ShoppingCartModel(dr);
                list.Add(ShoppingCart);
            }
            return list;
        }
        //------------------------------------------
        
    }
}
