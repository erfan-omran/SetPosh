using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ServiceInterface
{
    public interface IBaseService
    {
        public static DataTable GetDataTable();
    }
}
