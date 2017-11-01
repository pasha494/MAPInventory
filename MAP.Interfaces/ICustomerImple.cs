using MAP.Inventory.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.Interface
{
    public interface ICustomerImple
    {
        long Save(CustomerModel _CustomerModel);

        CustomerModel Edit(int ID);

        DataTable GetGridData(int CustomerID);

        long Delete(int CustomerID);
    }
}
