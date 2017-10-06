using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.Interface
{
    public interface ILoginImple
    {
        int CheckLogin(string UserEmail, string Password);
        void CreateUserSession(DataTable dt);
    }
}
