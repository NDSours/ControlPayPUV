using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ControlPayPUV.Utils
{
    public static class StringUtils
    {
        public static string GetStringFromBool(this bool b)
        {
            if (b)
                return "Есть";
            else
                return "Нет";
        }

    }
}
