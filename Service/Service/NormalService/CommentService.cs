using Core;
using Core.Model;
using DataBase;
using Service.ServiceInterface;
using System.Data;

namespace Service
{
    public class CommentService : IBaseNormalService<CommentModel>
    {
        public static List<string> MainColumns = new List<string>()
        {
            Dictionary.Comment.SID.FullDBName,
            Dictionary.Comment.USID.FullDBName,
            Dictionary.Comment.PSID.FullDBName,
            Dictionary.Comment.CRate.FullDBName,
            Dictionary.Comment.CDescription.FullDBName
        };
        public static List<string> DefaultColumns = new List<string>()
        {
            Dictionary.Comment.Blocked.FullDBName,
            Dictionary.Comment.Deleted.FullDBName,

            Dictionary.Comment.CreationUSID.FullDBName,
            Dictionary.Comment.CreationDate.FullDBName,
            Dictionary.Comment.CreationTime.FullDBName,

            Dictionary.Comment.LastModifiedUSID.FullDBName,
            Dictionary.Comment.LastModifiedDate.FullDBName,
            Dictionary.Comment.LastModifiedTime.FullDBName
        };
        //------------------------------------------
        public async Task<bool> AddAsync(CommentModel entity)
        {
            entity.SaveAddParameters();
            bool Added = await DBConnection.ExecProcedureAsync("[Comment.Add]", entity.Parameters);
            return Added;
        }
        public async Task<bool> EditAsync(CommentModel entity)
        {
            entity.SaveEditParameters();
            bool Edited = await DBConnection.ExecProcedureAsync("[Comment.Edit]", entity.Parameters);
            return Edited;
        }

        public async Task<bool> BlockAsync(long SID)
        {
            CommentModel Comment = new CommentModel();
            Comment.Blocked = true;
            Comment.SaveBlockedParameter(SID);
            bool Ans = await DBConnection.ExecProcedureAsync("[Comment.Block]", Comment.Parameters);
            return Ans;
        }
        public async Task<bool> UnBlockAsync(long SID)
        {
            CommentModel Comment = new CommentModel();
            Comment.Blocked = false;
            Comment.SaveBlockedParameter(SID);
            bool Ans = await DBConnection.ExecProcedureAsync("[Comment.Block]", Comment.Parameters);
            return Ans;
        }

        public async Task<bool> DeleteAsync(long SID)
        {
            CommentModel Comment = new CommentModel();
            Comment.Deleted = true;
            Comment.SaveBlockedParameter(SID);
            bool Ans = await DBConnection.ExecProcedureAsync("[Comment.Delete]", Comment.Parameters);
            return Ans;
        }
        public async Task<bool> UnDeleteAsync(long SID)
        {
            CommentModel Comment = new CommentModel();
            Comment.Deleted = false;
            Comment.SaveBlockedParameter(SID);
            bool Ans = await DBConnection.ExecProcedureAsync("[Comment.Delete]", Comment.Parameters);
            return Ans;
        }
        //------------------------------------------
        public async Task<CommentModel> GetModelSimpleAsync(long SID)
        {
            QueryBuilder qb = GetSimple();
            qb.AddEqualCondition(Dictionary.Comment.SID.FullDBName, SID);

            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());
            CommentModel Comment = new CommentModel(dr);

            return Comment;
        }
        public async Task<CommentModel> GetModelWithRelatedEntitiesAsync(long SID)
        {
            QueryBuilder qb = GetWithRelatedEntities();
            qb.AddEqualCondition(Dictionary.Comment.SID.FullDBName, SID);
            DataRow dr = await DBConnection.GetDataRowAsync(qb.CreateQuery());

            CommentModel Comment = new CommentModel(dr);
            Comment.User = new UserModel(dr);
            Comment.Product = new ProductModel(dr);

            return Comment;
        }
        public async Task<List<CommentModel>> GetProductCommentsAsync(long SID)
        {
            QueryBuilder qb = GetWithRelatedEntities();
            qb.AddEqualCondition(Dictionary.Comment.PSID.FullDBName, SID);
            qb.AddEqualCondition(Dictionary.Comment.Blocked.FullDBName, 0);
            qb.AddEqualCondition(Dictionary.Comment.Deleted.FullDBName, 0);

            qb.AddOrderBy(Dictionary.Comment.CreationDate.FullDBName, false);
            qb.AddOrderBy(Dictionary.Comment.CreationTime.FullDBName, false);

            DataTable dt = await DBConnection.GetDataTableAsync(qb.CreateQuery());
            List<CommentModel> Comments = MapDTToModel(dt);
            return Comments;
        }

        public QueryBuilder GetSimple()
        {
            QueryBuilder qb = new QueryBuilder();
            qb.AddColumns(MainColumns);
            qb.AddColumns(DefaultColumns);
            qb.SetTable(Dictionary.Comment.TableName);
            return qb;
        }
        public QueryBuilder GetWithRelatedEntities()
        {
            QueryBuilder qb = GetSimple();
            qb.AddColumns(UserService.MainColumns);
            qb.AddColumns(ProductService.MainColumns);

            qb.AddLeftJoin(Dictionary.User.TableName, qb => { qb.AddEqualCondition(Dictionary.User.SID.FullDBName, Dictionary.Comment.USID.FullDBName); });
            qb.AddLeftJoin(Dictionary.Product.TableName, qb => { qb.AddEqualCondition(Dictionary.Product.SID.FullDBName, Dictionary.Comment.PSID.FullDBName); });
            return qb;
        }
        //------------------------------------------
        public List<CommentModel> MapDTToModel(DataTable dt)
        {
            List<CommentModel> list = new List<CommentModel>();
            foreach (DataRow dr in dt.Rows)
            {
                CommentModel Comment = new CommentModel(dr);
                Comment.User = new UserModel(dr);
                Comment.Product = new ProductModel(dr);
                list.Add(Comment);
            }
            return list;
        }
    }
}
