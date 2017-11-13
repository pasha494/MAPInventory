using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.Model
{
    public class SessionManager
    {
        public SessionManager()
        {
            RoleFeatures = new Dictionary<string, bool>();
        }

        public int UserID { set; get; }
        public string UserName { set; get; }
        public string Email { set; get; }
        public bool IsAdmin { get; set; }
        public int RoleID { get; set; }

        public Dictionary<string, bool> RoleFeatures { get; set; }
    }
}
