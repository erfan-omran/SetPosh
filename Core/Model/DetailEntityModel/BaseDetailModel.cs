using System.Data;

namespace Core.Model
{
    public class BaseDetailModel : BaseModel
    {
        public long ID { get; set; } = default;
        public void InitBaseDetailModel(DataRow dr)
        {
            ID = dr.GetValueOfLongColumn(nameof(ID));
            InitBaseModel(dr);
        }
    }
}
