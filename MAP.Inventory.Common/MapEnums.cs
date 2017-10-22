using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.Common
{

    public enum EnumListViews
    {
        Products = 1,
        WareHouses = 2,
        Customers = 3,
        Vendors = 4
    }

    public enum EnumGridView
    {
        OpeningStock = 1,
        InwardStock = 2,
        OutwardStock = 3,
        StockTransfer = 4,
        OpeningStockView = 5,
        InwardStockView = 6,
        OutwardStockView = 7,
        StockTransferView = 8
    }

}
