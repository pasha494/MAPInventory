using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using MAP.Inventory.Common.Controls;

namespace MAPInventory.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult ListViweCustomization()
        {
            MapListView list = new MapListView();
            list.columns.Add(new MapListviewCoumns());



            return View();
        }

    }
}
