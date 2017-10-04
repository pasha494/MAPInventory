using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Logging
{
    //-------------Creating a custom static class for storing logs------------------//
    public static class PLog
    {
        static PLog()
        {
            // stuff
        }

        public readonly static log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Info(string error, int userId = 0, string userName = "")
        {
            error += " ========UserName:'" + userName + "' and UserId=" + userId;
            log4net.GlobalContext.Properties["UserId"] = userId;
            logger.Info(error);

        }

        public static void Info(string error, Exception ex, int userId = 0, string userName = "")
        {
            error += " ========UserName:'" + userName + "' and UserId=" + userId;
            log4net.GlobalContext.Properties["UserId"] = userId;
            logger.Info(error, ex);
        }

        public static void Error(string error, int userId = 0, string userName = "")
        {
            error += " ========UserName:'" + userName + "' and UserId=" + userId;
            log4net.GlobalContext.Properties["UserId"] = userId;
            logger.Error(error);

        }

        public static void Error(string error, Exception ex, int userId = 0, string userName = "")
        {
            error += " ========UserName:'" + userName + "' and UserId=" + userId;
            log4net.GlobalContext.Properties["UserId"] = userId;
            logger.Error(error, ex);
        }

    }
}
