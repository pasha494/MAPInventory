using MAP.Inventory.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.Interface
{
    public interface IUsersImple
    {
        long SaveUsers(UsersModel User);

        UsersModel EditUser(int ID);

        DataTable GetGridData(int UserID);

        long DeleteUser(int UserID);

        long ChangePassword(string OldPassword, string NewPassword, int UserID);

    }
}
