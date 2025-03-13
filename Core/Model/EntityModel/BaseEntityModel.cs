using System.Data;
using System.Data.SqlClient;

namespace Core.Model
{
    public class BaseEntityModel : BaseModel
    {
        public long SID { get; set; } = default;
        public void InitBaseEntityModel(DataRow dr)
        {
            SID = dr[nameof(SID)].ConvertToLong();
            InitBaseModel(dr);
        }
    }
}
