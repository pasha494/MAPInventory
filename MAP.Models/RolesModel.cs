using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.Model
{ 
    public class RolesModel
    { 
        public int RoleId{ get; set; }

        public string RoleName { get; set; }

        public string RoleData { get; set; }

        public bool IsSystemRole { get; set; }
    }
}
