using MAP.Inventory.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.Interface
{
    public interface IWareHouseImple
    {
        long SaveWareHouse(WareHouseModel _WareHouseModel);
        WareHouseModel EditWareHouse(int ID);
        DataTable GetGridData(int WareHouseID);
        long DeleteWareHouse(int ProductID);
    }
}
