using System.Data;
using System.Data.SqlClient;

namespace Core.Model
{
    public class CommentModel : BaseEntityModel
    {
        public UserModel User { get; set; } = new UserModel();
        public ProductModel Product { get; set; } = new ProductModel();
        public short CRate { get; set; } = default;
        public string CDescription { get; set; } = string.Empty;

        public CommentModel() { }
        public CommentModel(DataRow dr)
        {
            User = new UserModel(dr);
            Product = new ProductModel(dr);

            CRate = dr[CRate].ConvertToShort();
            CDescription = dr[CDescription].ConvertToString();
            base.InitBaseEntityModel(dr);
        }
        //-------------
        public void SaveAddParameters()
        {
            SaveMainParameters(true);
            SaveBlockedParameter();
            SaveDeletedParameter();
            SaveCreationParameters();
            SaveModificationParameters();
        }
        public void SaveEditParameters()
        {
            SaveMainParameters(false);
            SaveBlockedParameter();
            SaveDeletedParameter();
            SaveModificationParameters();
        }
        public void SaveMainParameters(bool IsAdd)
        {
            SqlParameter SIDParam = new SqlParameter("@" + Dictionary.Comment.SID.EngName, SID);
            if (IsAdd)
                SIDParam.Direction = System.Data.ParameterDirection.Output;
            Parameters.Add(SIDParam);
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.Comment.USID.EngName), User.SID));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.Comment.PSID.EngName), Product.SID));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.Comment.CRate.EngName), CRate));
            Parameters.Add(new SqlParameter("@" + nameof(Dictionary.Comment.CDescription.EngName), CDescription));
        }
    }
}
