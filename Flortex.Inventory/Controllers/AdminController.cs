using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using MAP.Inventory.Common.Controls;
using MAP.Inventory.ModelImple;
using MAP.Inventory.Model;
using MAP.Inventory.Web.Models;

namespace MAP.Inventory.Web.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        [SessionExpire(FeatureKey = "ListViewCusotmization", RequestType = 1)]
        public ActionResult ListViewCustomization()
        {
            // Pass the feature id as zero in the below constructor
            MapListViewImple objMapListViewImple = new MapListViewImple(0);
            List<ListViewCustomization> objListViewsInfo = objMapListViewImple.GetListViewsCustomizationInfo();
            string x = JsonConvert.SerializeObject(objListViewsInfo[0]);
            return View(objListViewsInfo);
        }
         
        [SessionExpire(FeatureKey = "Listview_Save", RequestType = 2)]
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


        [SessionExpire(FeatureKey = "GridViewCustomization", RequestType = 1)]
        public ActionResult GridViewCustomization()
        {


            return View();
        }
         
        public ActionResult GetGridViewCustomizationData(int FeatureId)
        {
            MapGridViewImple objGridView = new MapGridViewImple(FeatureId);
            GridViewCustomization objModal = objGridView.GetGridViewsCustomizationInfo(0);
            string modelContent = JsonConvert.SerializeObject(objModal);
            return Content(modelContent);

        }

        [SessionExpire(FeatureKey = "Gridview_Save", RequestType = 2)]
        public ActionResult SaveGridViewCustomization(string model)
        {
            long result = 0;

            try
            {
                GridViewCustomization objModel = JsonConvert.DeserializeObject<GridViewCustomization>(model);
                MapGridViewImple objImple = new MapGridViewImple(objModel.FeatureId);
                result = objImple.SaveGridViewCustomizationInfo(objModel);
            }
            catch (Exception ex)
            {
                result = -494;
            }

            return Content(result.ToString());
        }



        #region Roles
        [SessionExpire(FeatureKey = "RolesCustomization", RequestType = 1)]
        public ActionResult Roles()
        {
            List<RolesModel> objModel = null;
            RolesImple _RolesImple = new RolesImple();
            objModel = _RolesImple.GetRolesData();
            ViewBag.MainTreeData = JsonConvert.SerializeObject(_RolesImple.GetRolesFeatureActionData());

            return View(objModel);
        }

        [HttpPost]
        [SessionExpire(FeatureKey = "Roles_SaveRole", RequestType = 2)]
        public ActionResult SaveRoleData(int RoleId, string RoleName, string RoleData)
        {
            long result = 0;

            try
            {
                RolesImple objModel = new RolesImple();
                result = objModel.SaveRoleData(RoleId, RoleName, RoleData);

            }
            catch (Exception ex)
            {
                result = -494;
            }

            return Content(result.ToString());
        }

        [SessionExpire(FeatureKey = "Roles_DeleteRole", RequestType = 2)]
        public ActionResult DeleteRole(int RoleId)
        {
            long result = 0;

            try
            {
                RolesImple objModel = new RolesImple();
                result = objModel.DeleteRole(RoleId);

            }
            catch (Exception ex)
            {
                result = -494;
            }

            return Content(result.ToString());
        }

        [SessionExpire(FeatureKey = "RolesCustomization", RequestType = 2)]
        public ActionResult RefreshRoles()
        {
            List<RolesModel> objModel = null;
            RolesImple _RolesImple = new RolesImple();
            objModel = _RolesImple.GetRolesData();
            return Content(JsonConvert.SerializeObject(objModel));
        }

        [SessionExpire(FeatureKey = "Roles_EditRole", RequestType = 2)]
        public ActionResult EditRole(int id)
        {
            RolesModel _RolesModel = new RolesModel();
            RolesImple _RolesImple = new RolesImple();
            _RolesModel = _RolesImple.GetRolesData(id);

            return Content(JsonConvert.SerializeObject(_RolesModel));
        }

        #endregion

    }
}
