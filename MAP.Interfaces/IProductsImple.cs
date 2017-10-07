using MAP.Inventory.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.Interface
{
    public interface IProductsImple
    {
         long SaveProducts(ProductsModel _ProductsModel);

         ProductsModel EditProdcut(int ID);

         DataTable GetGridData(int ProductID);

         long DeleteProduct(int ProductID);
    }
}
