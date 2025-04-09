using Core;
using Core.Model;
using DataBase;
using Service.ServiceInterface;
using System.Data;

namespace Service
{
    public class ShoppingCartDetailService : IBasePartService<ShoppingCartDetailModel>
    {
        public static List<string> MainColumns = new List<string>()
        {
            Dictionary.ShoppingCartDetail.ID.FullDBName,
            Dictionary.ShoppingCartDetail.SCSID.FullDBName,
            Dictionary.ShoppingCartDetail.PSID.FullDBName,
            Dictionary.ShoppingCartDetail.SCDCount.FullDBName
        };
        public static List<string> DefaultColumns = new List<string>()
        {
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
            entity.SaveAddParameters(entity.ShoppingCart.User.SID);
            bool Added = await DBConnection.ExecTransactionProcedureAsync("[ShoppingCartDetail.Add]", entity.Parameters);
            return Added;
        }
        public async Task<bool> EditAsync(ShoppingCartDetailModel entity)
        {
            entity.SaveEditParameters(entity.ShoppingCart.USID);
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
        public async Task<ShoppingCartDetailModel> GetModelSimpleAsync(long SID)
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
            ShoppingCartDetail.ShoppingCart = new ShoppingCartModel(dr);
            ShoppingCartDetail.Product = new ProductModel(dr);

            return ShoppingCartDetail;
        }
        public async Task<long> GetUserProductsCountAsync(long USID, long PSID)
        {
            QueryBuilder WithQB = new QueryBuilder();
            WithQB.AddColumn(Dictionary.ShoppingCart.SID.FullDBName);
            WithQB.SetTable(Dictionary.ShoppingCart.TableName);
            WithQB.AddEqualCondition(Dictionary.ShoppingCart.USID.FullDBName, USID);
            WithQB.AddEqualCondition(Dictionary.ShoppingCart.IsActive.FullDBName, 1);
            WithQB.AddEqualCondition(Dictionary.ShoppingCart.Blocked.FullDBName, 0);
            WithQB.AddEqualCondition(Dictionary.ShoppingCart.Deleted.FullDBName, 0);

            QueryBuilder MainQB = new QueryBuilder();
            MainQB.AddWith(WithQB, nameof(WithQB));

            MainQB.AddColumn(SqlFunction.IsNull(SqlFunction.Sum(Dictionary.ShoppingCartDetail.SCDCount.FullDBName),0));
            MainQB.SetTable(Dictionary.ShoppingCartDetail.TableName);
            MainQB.AddLeftJoin(nameof(WithQB), qb =>
            {
                qb.AddEqualCondition($"{nameof(WithQB)}.SID", Dictionary.ShoppingCartDetail.SCSID.FullDBName);
            });

            MainQB.AddEqualCondition(Dictionary.ShoppingCartDetail.SCSID.FullDBName, $"{nameof(WithQB)}.SID");
            MainQB.AddEqualCondition(Dictionary.ShoppingCartDetail.PSID.FullDBName, PSID);

            string query = MainQB.CreateQuery();
            long Count = await DBConnection.GetFirstValueAsync<long>(query);
            return Count;
        }

        public QueryBuilder GetSimple()
        {
            QueryBuilder qb = new QueryBuilder();
            qb.AddColumns(MainColumns);
            qb.AddColumns(DefaultColumns);
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
