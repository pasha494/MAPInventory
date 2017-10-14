using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.Common.Controls
{
    public class MapListView
    {
        public MapListView()
        { 
            columns = new List<MapListviewCoumns>();
            idField = string.Empty;
            mode = string.Empty;
            searchField = string.Empty;
            textField = string.Empty; 
            value = string.Empty;
            listViewName = string.Empty;
        }

        public List<MapListviewCoumns> columns { get; set; }
        public int delay { get; set; }
        public bool fitColumns { get; set; }
        public string idField { get; set; }
        public string listViewName { get; set; }
        public string mode { get; set; }
        public int panelWidth { get; set; }
        public string searchField { get; set; }
        public string textField { get; set; }   
        public string value { get; set; }

    }

    public class MapListviewCoumns
    {
        public MapListviewCoumns()
        {
            align = string.Empty;
            field = string.Empty;
            title = string.Empty;
        }

        public string align { get; set; }
        public string field { get; set; }
        public string title { get; set; }        
        public int width { get; set; }
    }
}
