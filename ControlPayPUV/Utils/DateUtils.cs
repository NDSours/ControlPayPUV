using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPayPUV.Utils
{
    public static class DateUtils
    {
        public static string GetDate(DateTime? date)
        {
            if (date != null)
                if (date == DateTime.MinValue)
                    return "";
                else
                    return date.Value.ToString("dd.MM.yyyy");
            else
                return "";
        }

        public static string GetPeriodDate(DateTime? date1, DateTime? date2)
        {
            if (date1 == null && date2 == null)
                return "";
            if (date1 == DateTime.MinValue && date2 == DateTime.MinValue)
                return "";
            else
                return GetDate(date1) + " - " + GetDate(date2);
        }


    }
}
