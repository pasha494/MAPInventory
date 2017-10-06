using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.Model
{
    public class UsersModel
    {
        public UsersModel()
        {

        }

        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public string efDate { get; set; }
        public int RoleID { get; set; }

    }
}
