using MAP.Inventory.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.Model
{
    public class GridViewCustomization
    {
        public GridViewCustomization()
        {

        }
         
        public int FeatureId { get; set; }
 
        public string Name { get; set; }

        public string Columns { get; set; }

        public int TotalRows { get; set; }

        public double Height { get; set; }

    }
}
