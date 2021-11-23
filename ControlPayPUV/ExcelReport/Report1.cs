using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlPayPUV;
using NPOI.SS.UserModel;
using ControlPayPUV.Utils;
using DALControlPayPUV;
using DALControlPayPUV.Repository;

namespace ControlPayPUV.ExcelReport
{
    public static class Report1
    {
        
        public static void GetReport(string filename,Filter filter)
        {
            IWorkbook book = NPOIUtils.GetOpenFile(filename);
            if(book != null)
            {
                ISheet sheet = book.GetSheetAt(0);
                sheet.CreateRow(2).CreateCell(0).SetCellValue("Дата формирования списка: " + DateTime.Now.ToString("dd.MM.yyyy"));
                int row = 3;
                foreach(var item in Repository.FilteredListPerson(filter))
                {
                    foreach(var statement in item.Statements)
                    {
                        var currentrow = sheet.CreateRow(row);
                        currentrow.CreateCell(0).SetCellValue(item.SNILS);
                        currentrow.CreateCell(1).SetCellValue(item.Surname + " " + item.Name + " " + item.Patr);
                        currentrow.CreateCell(2).SetCellValue(DateUtils.GetDate(item.DateBirthday));
                        currentrow.CreateCell(3).SetCellValue(statement.RegNumber);
                        currentrow.CreateCell(4).SetCellValue(DateUtils.GetDate(statement.DateInnings));
                        currentrow.CreateCell(5).SetCellValue(DateUtils.GetDate(statement.DateRegistration));
                        currentrow.CreateCell(6).SetCellValue(statement.Status);
                        currentrow.CreateCell(7).SetCellValue(DateUtils.GetDate(statement.DateResolved));
                        currentrow.CreateCell(8).SetCellValue(statement.Vid_Pay);
                        if(statement.DocumentsPay.Count() == 0)
                            row++;
                        else
                        {
                            foreach (var document in statement.DocumentsPay)
                            {
                                currentrow.CreateCell(9).SetCellValue(document.TypePay);
                                currentrow.CreateCell(10).SetCellValue(document.NameDeliveryOrg);
                                currentrow.CreateCell(11).SetCellValue(document.KCSchetDeliveryOrg);
                                currentrow.CreateCell(12).SetCellValue(document.BIKDeliveryOrg);
                                currentrow.CreateCell(13).SetCellValue(document.AmountPay.ToString("c"));
                                if (document.Childs.Count() == 0)
                                    row++;
                                else
                                {
                                    foreach(var child in document.Childs)
                                    {
                                        currentrow.CreateCell(14).SetCellValue(child.Surname + " " + child.Name + " " + child.Surname);
                                        currentrow.CreateCell(15).SetCellValue(child.SNILS);
                                        currentrow.CreateCell(16).SetCellValue(DateUtils.GetDate(child.DateBirthday));
                                        currentrow.CreateCell(17).SetCellValue(DateUtils.GetDate(child.DateStartPay));
                                        currentrow.CreateCell(18).SetCellValue(DateUtils.GetDate(child.DateEndPay));
                                        row++;
                                    }
                                }
                            }
                        }
                    }
                    //row++;
                }

                string path = Environment.CurrentDirectory;
                string savefile = default;
                switch (filter.HavingFiles)
                {
                    case Files.All:
                        savefile = path + "\\Все заявители.xlsx";
                        break;
                    case Files.HaveInformationOnPay:
                        savefile = path + "\\Заявители на которых выгружен фаил OZPEV_BZPEV.xlsx";
                        break;
                    case Files.NotHaveInformationOnPay:
                        savefile = path + "\\Заявители на которых нет фаила OZPEV_BZPEV.xlsx";
                        break;
                }
                
                if (File.Exists(savefile))
                    File.Delete(savefile);
                using(FileStream fs = new FileStream(savefile,FileMode.OpenOrCreate, FileAccess.Write))
                {
                    book.Write(fs);
                }

                book.Close();
                GC.Collect();
            }
        }

    }
}
