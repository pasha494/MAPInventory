using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Flortex.Inventory.Models
{
    //-------------Creating a custom static class for storing logs------------------//
    public static class PLog
    {
        readonly static log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                 
        public static void Info(string error)
        {
            if (HttpContext.Current.Session["SessionManager"] != null)
            {
                SessionManager objSession = (SessionManager)HttpContext.Current.Session["SessionManager"];
                error += " ========UserName:'" + objSession.UserName + "' and UserId=" + objSession.UserID;
                log4net.GlobalContext.Properties["UserId"] = objSession.UserID;
                logger.Info(error);
            }
            else
            {
                log4net.GlobalContext.Properties["UserId"] = 0;
                logger.Info(error);
            }
        }

        public static void Info(string error, Exception ex)
        {
            if (HttpContext.Current.Session["SessionManager"] != null)
            {
                SessionManager objSession = (SessionManager)HttpContext.Current.Session["SessionManager"];
                error += " ========UserName:'" + objSession.UserName + "' and UserId=" + objSession.UserID;
                log4net.GlobalContext.Properties["UserId"] = objSession.UserID;
                logger.Info(error, ex);
            }
            else
            {
                log4net.GlobalContext.Properties["UserId"] = 0;
                logger.Info(error, ex);
            }
        }

        public static void Error(string error)
        {
            if (HttpContext.Current.Session["SessionManager"] != null)
            {
                SessionManager objSession = (SessionManager)HttpContext.Current.Session["SessionManager"];
                error += " ========UserName:'" + objSession.UserName + "' and UserId=" + objSession.UserID;
                log4net.GlobalContext.Properties["UserId"] = objSession.UserID;
                logger.Error(error);
            }
            else
            {
                log4net.GlobalContext.Properties["UserId"] = 0;
                logger.Error(error);
            }
        }

        public static void Error(string error, Exception ex)
        {
            if (HttpContext.Current.Session["SessionManager"] != null)
            {
                SessionManager objSession = (SessionManager)HttpContext.Current.Session["SessionManager"];
                error += " ========UserName:'" + objSession.UserName + "' and UserId=" + objSession.UserID;
                log4net.GlobalContext.Properties["UserId"] = objSession.UserID;
                logger.Error(error, ex);
            }
            else
            {
                log4net.GlobalContext.Properties["UserId"] = 0;
                logger.Error(error, ex);
            }
        }

    }
}