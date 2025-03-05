using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Enum
{
    public enum TableEnum : int
    {
        None = 0,
        Comment,
        Demand,
        DemandDetail,
        Log_Exception,
        Product,
        ProductCategory,
        ShoppingCart,
        ShoppingCartDetail,
        User,
        UserType,
    }
}
