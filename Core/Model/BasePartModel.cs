using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class BasePartModel : BaseModel
    {
        public long ID { get; set; } = default;
        public void InitBasePartModel(DataRow dr)
        {
            ID = dr[nameof(ID)].ConvertToLong();
            base.InitBaseModel(dr);
        }
    }
}
