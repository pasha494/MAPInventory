using MAP.Inventory.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.Model
{
    public class ListViewCustomization
    {

        public ListViewCustomization(string _AcitonMethodName, string _ControllerName, int _FeatureId, string _Name, string _Options, int _TotalRows)
        {
            AcitonMethodName = _AcitonMethodName;
            ControllerName = _ControllerName;
            FeatureId = _FeatureId;
            Name = _Name;
            Options = _Options;
            TotalRows = _TotalRows; 
        }

        public string AcitonMethodName { get;   }
        public string ControllerName { get;   }

        public int FeatureId { get;  }

        private MapListView _ListViewOptions;

        public MapListView MapListViewOptions
        {
            get { return _ListViewOptions; } 
        } 

        public string Name { get; }

        public string Options { get;  }

        public int TotalRows { get;   }


        public void SetMapListViewOptions(MapListView _MapListViewOptions)
        {
            _ListViewOptions = _MapListViewOptions;
        }



    }
}
