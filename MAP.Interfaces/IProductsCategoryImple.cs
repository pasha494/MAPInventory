using MAP.Inventory.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.Interface
{
    public interface IProductsCategoryImple
    {
        long SaveProductCategory(ProductsCategoryModel _ProductsCategoryModel);
        ProductsCategoryModel EditProdcutCategory(int ID);
        DataTable GetGridData(int ProductCategoryID);
        long DeleteProductsCategory(int ProductCategoryID);
    }
}
