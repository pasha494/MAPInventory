using MAP.Inventory.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.ModelImple
{
    public  class HealthCheckImple
    {
        General _General = new General();

        public string GetSQLConnectionStatus()
        {
            return _General.HealthCheckConnection(); 
        }


    }
}
