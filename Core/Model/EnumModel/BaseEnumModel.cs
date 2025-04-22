using System.Data;
using System.Data.SqlClient;

namespace Core.Model
{
    public class BaseEnumModel
    {
        public long ID { get; set; } = default;
        public List<SqlParameter> Parameters = new List<SqlParameter>();

        public void InitBasePartModel(DataRow dr)
        {
            ID = dr.GetValueOfLongColumn(nameof(ID));
        }
    }
}
