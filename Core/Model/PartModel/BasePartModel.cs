using System.Data;

namespace Core.Model
{
    public class BasePartModel : BaseModel
    {
        public long ID { get; set; } = default;
        public void InitBasePartModel(DataRow dr)
        {
            ID = dr[nameof(ID)].ConvertToLong();
            InitBaseModel(dr);
        }
    }
}
