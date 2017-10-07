using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.Model
{
    public class ProductsModel
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int ProductCategory { get; set; }
        public int Status { get; set; }
        public string efDate { get; set; }
        public string Spec { get; set; }
    }
}
