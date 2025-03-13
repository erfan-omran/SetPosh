using Core;
using Core.Model;
using DataBase;
using Service.ServiceInterface;
using System.Data;

namespace Service.Service
{
    public class CommentService : IBaseNormalService<CommentModel>
    {
        public static List<string> MainColumns = new List<string>()
        {
            Dictionary.Comment.SID.FullDBName,
            Dictionary.Comment.USID.FullDBName,
            Dictionary.Comment.PSID.FullDBName,
            Dictionary.Comment.CRate.FullDBName,
            Dictionary.Comment.CDescription.FullDBName,

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
        public async Task BlockAsync(long SID)
        {
            CommentModel Comment = new CommentModel();
            Comment.SaveBlockedParameter(SID);
            await DBConnection.ExecProcedureAsync("[Comment.Block]", Comment.Parameters);
        }
        public async Task DeleteAsync(long SID)
        {
            CommentModel Comment = new CommentModel();
            Comment.SaveBlockedParameter(SID);
            await DBConnection.ExecProcedureAsync("[Comment.Delete]", Comment.Parameters);
        }
        //------------------------------------------
        public async Task<CommentModel> GetSimpleModelAsync(long SID)
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

            return Comment;
        }

        public QueryBuilder GetSimple()
        {
            QueryBuilder qb = new QueryBuilder();
            qb.AddColumns(MainColumns);
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
                list.Add(Comment);
            }
            return list;
        }
    }
}
