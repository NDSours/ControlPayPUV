using ControlPayPUV.Utils;
using System.Collections.Generic;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.IO;
using System.Diagnostics;

namespace ControlPayPUV.Protocols
{
 
    
    public class ProtocolLoadDocumentPUV
    {
        public string RegNumber { get; set; }
        public string SNILS { get; set; }
        public string OldStatus { get; set; }
        public string OldTypeResolved { get; set; }
        public string NewStatus { get; set; }
        public string NewTypeResolved { get; set; }

    }

    public static class ProtocolCreater
    {
        public static void SaveProtocolLoadDocumentPUV(List<ProtocolLoadDocumentPUV> list)
        {
            string filename = Environment.CurrentDirectory + "\\Templates\\Template2.xlsx";
            if (!File.Exists(filename))
                throw new Exception("Не найден шаблон протокола: " + filename);
            IWorkbook book = NPOIUtils.GetOpenFile(filename);
            if(book != null)
            {
                ISheet sheet = book.GetSheetAt(0);
                int i = 2;
                foreach(var item in list)
                {
                    var currentrow = sheet.CreateRow(i);
                    currentrow.CreateCell(0).SetCellValue((i - 1).ToString());
                    currentrow.CreateCell(1).SetCellValue(item.RegNumber);
                    currentrow.CreateCell(2).SetCellValue(item.SNILS);
                    currentrow.CreateCell(3).SetCellValue(item.OldStatus);
                    currentrow.CreateCell(4).SetCellValue(item.OldTypeResolved);
                    currentrow.CreateCell(5).SetCellValue(item.NewStatus);
                    currentrow.CreateCell(6).SetCellValue(item.NewTypeResolved);
                    i++;
                }

                string savefile = Environment.CurrentDirectory + "\\Protocols";
                if (!Directory.Exists(savefile))
                    Directory.CreateDirectory(savefile);
                savefile += "\\" + DateTime.Now.ToString("dd.MM.yyyy_HH.mm.ss") + "_Протокол загрузки файла PUV.xlsx";
                using(FileStream fs = new FileStream(savefile, FileMode.Create, FileAccess.Write))
                {
                    book.Write(fs);
                }

                Process.Start(savefile);
            }
        }
    }
}
