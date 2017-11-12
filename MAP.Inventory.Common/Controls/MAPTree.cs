using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.Common.Controls
{
    public class MapTree
    {
        public MapTree()
        {
            children = new List<MapTree>();
        }

        public int id { get; set; }

        public string field { get; set; }

        public string text { get; set; }

        public bool @checked { get; set; }

        public bool indeterminate { get; set; }

        public bool value
        { 
            get {return (indeterminate || @checked); } 
        }


        public List<MapTree> children { get; set; }
    }
}
