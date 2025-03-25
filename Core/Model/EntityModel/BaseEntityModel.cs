using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace Core.Model
{
    public class BaseEntityModel : BaseModel
    {
        public long SID { get; set; } = default;
        public void InitBaseEntityModel(DataRow dr)
        {
            SID = dr.GetValueOfLongColumn(nameof(SID));
            InitBaseModel(dr);
        }
    }
}
