using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace ControlPayPUV.Utils
{
    public static class NPOIUtils
    {
        public static IWorkbook GetOpenFile(string FileName)
        {
            string extension = Path.GetExtension(FileName);
            IWorkbook book;

            using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                if (extension == ".xlsx")
                {
                    try
                    {
                        book = new XSSFWorkbook(fs);
                    }
                    catch
                    {
                        book = null;
                    }

                }
                else if (extension == ".xls")
                {
                    try
                    {
                        book = new HSSFWorkbook(fs);
                    }
                    catch
                    {
                        book = null;
                    }
                }
                else
                    throw new Exception($"Фаил с расширением {extension} не поддерживается.");

                return book;
            }
        }
    }
}
