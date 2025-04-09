using System.Data;

namespace Core.Model
{
    public class BasePartModel
    {
        public long ID { get; set; } = default;
        public void InitBasePartModel(DataRow dr)
        {
            ID = dr.GetValueOfLongColumn(nameof(ID));
        }
    }
}
