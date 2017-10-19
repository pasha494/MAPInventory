using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using MAP.Inventory.Common.Controls;
using MAP.Inventory.ModelImple;
using MAP.Inventory.Model;

namespace MAP.Inventory.Web.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult ListViewCustomization()
        {
            // Pass the feature id as zero in the below constructor
            MapListViewImple objMapListViewImple = new MapListViewImple(0);
            List<ListViewCustomization> objListViewsInfo = objMapListViewImple.GetListViewsCustomizationInfo();
            string x = JsonConvert.SerializeObject(objListViewsInfo[0]);
            return View(objListViewsInfo);
        }


        public ActionResult SaveListViewCutomization(string model)
        {
            long result = 0;

            try
            {
                ListViewCustomization objModel = JsonConvert.DeserializeObject<ListViewCustomization>(model);
                MapListViewImple objImple = new MapListViewImple(objModel.FeatureId);
                result = objImple.SaveListViewCustomizationInfo(objModel);
            }
            catch (Exception ex)
            {
                result = -494;
            }

            return Content(result.ToString());
        }




    }
}
