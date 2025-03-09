using Core.Model.EntityModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public static class UserService
    {

        public static List<UserModel> MapDTToModels(DataTable dt)
        {
            List<UserModel> list = new List<UserModel>();
            foreach (DataRow dr in dt.Rows)
            {
                UserModel User = new UserModel(dr);
                list.Add(User);
            }
            return list;
        }
    }
}
