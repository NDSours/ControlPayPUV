using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NPOI.SS.UserModel;
using ControlPayPUV.Utils;
using System.Diagnostics;

namespace ControlPayPUV.Protocols
{
    public class ProtocolLoadDocumentXML
    {
        public string GUID { get; set; }
        public string RegNumber { get; set; }
        public string SNILS { get; set; }
        public string Status { get; set; }
        public string TypeResolved {get; set;}
        public string Information { get; set; } = "Файл перемещен в дирректорию ExceptionFiles";

        public static void SaveProtocolLoadDocumentXML(List<ProtocolLoadDocumentXML> list)
        {
            string filename = Environment.CurrentDirectory + "\\Templates\\Template3.xlsx";
            if (!File.Exists(filename))
                throw new Exception("Не найден шаблон протокола: " + filename);
            IWorkbook book = NPOIUtils.GetOpenFile(filename);
            if (book != null)
            {
                ISheet sheet = book.GetSheetAt(0);
                int i = 2;
                foreach (var item in list)
                {
                    var currentrow = sheet.CreateRow(i);
                    currentrow.CreateCell(0).SetCellValue((i - 1).ToString());
                    currentrow.CreateCell(1).SetCellValue(item.GUID);
                    currentrow.CreateCell(2).SetCellValue(item.RegNumber);
                    currentrow.CreateCell(3).SetCellValue(item.SNILS);
                    currentrow.CreateCell(4).SetCellValue(item.Status);
                    currentrow.CreateCell(5).SetCellValue(item.TypeResolved);
                    currentrow.CreateCell(6).SetCellValue(item.Information);
                    i++;
                }

                string savefile = Environment.CurrentDirectory + "\\Protocols";
                if (!Directory.Exists(savefile))
                    Directory.CreateDirectory(savefile);
                savefile += "\\" + DateTime.Now.ToString("dd.MM.yyyy_HH.mm.ss") + "_Протокол загрузки файлов XML.xlsx";
                using (FileStream fs = new FileStream(savefile, FileMode.Create, FileAccess.Write))
                {
                    book.Write(fs);
                }

                Process.Start(savefile);
            }
        }
    }
}
