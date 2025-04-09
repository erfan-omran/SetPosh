using System.Data;
using System.Data.SqlClient;

namespace Core.Model
{
    public class CommentModel : BaseEntityModel
    {
        public long USID { get; set; } = default;
        public long PSID { get; set; } = default;
        public short CRate { get; set; } = default;
        public string CDescription { get; set; } = string.Empty;

        public UserModel User { get; set; } = new UserModel();
        public ProductModel Product { get; set; } = new ProductModel();

        public CommentModel() { }
        public CommentModel(DataRow dr)
        {
            USID = dr.GetValueOfLongColumn(Dictionary.Comment.USID.EngName);
            PSID = dr.GetValueOfLongColumn(Dictionary.Comment.PSID.EngName);
            CRate = dr.GetValueOfShortColumn(Dictionary.Comment.CRate.EngName);
            CDescription = dr.GetValueOfStringColumn(Dictionary.Comment.CDescription.EngName);
            base.InitBaseEntityModel(dr);
        }
        //-------------
        public void SaveAddParameters()
        {
            SaveMainParameters(true);
            SaveBlockedParameter();
            SaveDeletedParameter();
            SaveCreationParameters();
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
            if (!IsAdd)
                Parameters.Add(new SqlParameter("@" + Dictionary.Comment.SID.EngName, SID));
            Parameters.Add(new SqlParameter("@" + Dictionary.Comment.USID.EngName, USID));
            Parameters.Add(new SqlParameter("@" + Dictionary.Comment.PSID.EngName, PSID));
            Parameters.Add(new SqlParameter("@" + Dictionary.Comment.CRate.EngName, CRate));
            Parameters.Add(new SqlParameter("@" + Dictionary.Comment.CDescription.EngName, CDescription));
        }
    }
}
