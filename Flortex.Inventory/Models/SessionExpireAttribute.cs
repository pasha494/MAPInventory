using MAP.Inventory.Logging;
using MAP.Inventory.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MAP.Inventory.Web.Models
{
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public string FeatureKey { get; set; }

        /// <summary>
        /// 1 stands for Page
        /// 2 stands for Ajax call
        /// </summary>
        public int RequestType { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            // check  sessions here
            if ((HttpContext.Current == null || HttpContext.Current.Session == null) || HttpContext.Current.Session["SessionManager"] == null)
            {
                filterContext.Result = new RedirectResult("~/Home/Login");
                return;
            }
            else if (HttpContext.Current.Session != null && HttpContext.Current.Session["SessionManager"] != null)
            {

                SessionManager objSession = (SessionManager)HttpContext.Current.Session["SessionManager"];

                if (FeatureKey != null && (objSession.RoleFeatures == null || !objSession.RoleFeatures.ContainsKey(FeatureKey) || !objSession.RoleFeatures[FeatureKey]) && objSession.RoleID != 1)
                {
                    if (RequestType == 1)// for page calls
                    {
                        filterContext.Result = new RedirectResult("~/Home/UnAuthorizedAction");
                        return;
                    }
                    else // for Ajax calls
                    {
                        filterContext.Result = new RedirectResult("~/Home/UnAuthorizedFeature");
                        return;
                    }
                }

            }


            base.OnActionExecuting(filterContext);
        }
    }
}