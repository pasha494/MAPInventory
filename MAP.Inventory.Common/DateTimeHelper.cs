using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.Common
{
    public static class DateTimeHelper
    {

        public static string ConvertDateToString(DateTime dt)
        {

            if (!string.IsNullOrEmpty(AppPreferences.DateFormat))
            {
                switch (AppPreferences.DateFormat)
                {
                    case "dd-MM-yyyy":
                        return dt.Date.ToString("dd-MM-yyyy");
                    case "dd/MM/yyyy":
                        return dt.Date.ToString("dd/MM/yyyy");

                    case "MM/dd/yyyy":
                        return dt.Date.ToString("MM/dd/yyyy");
                    case "MM-dd-yyyy":
                        return dt.Date.ToString("MM-dd-yyyy");

                    //case "dd/MMM/yyyy":
                    //    return dt.Date.ToString("dd/MMM/yyyy");
                    //case "dd-MMM-yyyy":
                    //    return dt.Date.ToString("dd-MMM-yyyy");

                    default:
                      return  dt.Date.ToString();
                }
            }
            return dt.Date.ToString();
        }


        public static DateTime ConvertStringToDate(string date)
        {
            try
            {
                if (!string.IsNullOrEmpty(AppPreferences.DateFormat) && !string.IsNullOrEmpty(date))
                {
                    switch (AppPreferences.DateFormat)
                    {
                        case "dd-MM-yyyy":
                            return DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        case "dd/MM/yyyy":
                            return DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        case "MM/dd/yyyy":
                            return DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        case "MM-dd-yyyy":
                            return DateTime.ParseExact(date, "MM-dd-yyyy", CultureInfo.InvariantCulture);

                        //case "dd/MMM/yyyy":
                        //    return DateTime.ParseExact(date, "dd/MMM/yyyy", CultureInfo.InvariantCulture);
                        //case "dd-MMM-yyyy":
                        //    return DateTime.ParseExact(date, "dd-MMM-yyyy", CultureInfo.InvariantCulture);

                        default:
                            return DateTime.ParseExact(DateTime.Now.Date.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                }
            }
            catch (Exception ex)
            {
                 
            }
            return DateTime.ParseExact(DateTime.Now.Date.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

    }



    public static class AppPreferences
    { 
        public static string DateFormat { get; set; }

    }
}
