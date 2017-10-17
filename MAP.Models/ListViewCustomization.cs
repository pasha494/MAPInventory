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
        public ListViewCustomization()
        {

        }

        public ListViewCustomization(string _ActionMethodName, string _ControllerName, int _FeatureId, string _Name, string _Options, int _TotalRows)
        {
            ActionMethodName = _ActionMethodName;
            ControllerName = _ControllerName;
            FeatureId = _FeatureId;
            Name = _Name;
            Options = _Options;
            TotalRows = _TotalRows; 
        }



        public string ActionMethodName { get; set; }
        public string ControllerName { get; set; }

        public int FeatureId { get; set; }

        //private MapListView _ListViewOptions;

        //public MapListView MapListViewOptions
        //{
        //    get { return _ListViewOptions; } 
        //} 

        public string Name { get; set; }

        public string Options { get; set; }

        public int TotalRows { get; set; }


        //public void SetMapListViewOptions(MapListView _MapListViewOptions)
        //{
        //    _ListViewOptions = _MapListViewOptions;
        //}
    }
}
