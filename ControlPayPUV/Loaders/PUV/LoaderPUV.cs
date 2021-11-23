using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using DALControlPayPUV.Entities;
using ControlPayPUV.Protocols;
using ControlPayPUV.Delegates;
using System.ComponentModel;
using System.Data.Entity;

namespace ControlPayPUV.Loaders.PUV
{
    public static class LoaderPUV
    {
        public static CloserForm closer;
        
        private static string GetStringValueFromRow(ICell cell)
        {
            string temprow = default;

            switch (cell.CellType)
            {
                case CellType.String:
                    temprow = cell.StringCellValue;
                    break;
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        DateTime date = cell.DateCellValue;
                        ICellStyle style = cell.CellStyle;
                        string format = style.GetDataFormatString().Replace('m', 'M');
                        temprow = date.ToString(format);
                    }
                    else
                        temprow = cell.NumericCellValue.ToString();
                    break;
                case CellType.Boolean:
                    temprow = cell.BooleanCellValue ? "TRUE" : "FALSE";
                    break;
                default:
                    temprow = "";
                    break;
            }

            return temprow;
        }

        private static object GetValueFromCvs<T>(string[] column,int pos)
        {
            //Console.WriteLine(pos.ToString());
            if (pos <= column.Length - 1)
            {

                if (typeof(T) == typeof(string))
                {
                    string temp = column[pos];
                    return (object)temp;
                }
                else if (typeof(T) == typeof(long))
                {
                    long temp = default;
                    if (long.TryParse(column[pos], out temp))
                    {
                        return (object)temp;
                    }
                    else
                        return null;
                }
                else if (typeof(T) == typeof(DateTime?))
                {
                    DateTime temp = default;
                    if (DateTime.TryParse(column[pos], out temp))
                    {
                        return (object)temp;
                    }
                    else
                        return null;

                }
                else
                    return null;
            }
            else
                return null;
        }

        //загрузка файла PUV формата *.cvs
        public static void LOAD_PUV_CVS_(object sender, DoWorkEventArgs e)
        {
            SummaryArgsForLoadPUV SummaryArgs = (SummaryArgsForLoadPUV)e.Argument;
            HeaderParser parser = new HeaderParser();
            parser.LoadFromXmlSerializer();
            //создаем лист новых получателей, которые будут добавляться в БД одним запросом
            List<Person> NewPersonList = new List<Person>();
            using (MyDBContext context = new MyDBContext())
            {
                StreamReader sr = new StreamReader(SummaryArgs.filename, Encoding.Default);
                //определим заголовки документа
                string header = sr.ReadLine();
                header = header.Replace('"', ' ');
                header = header.Replace('=', ' ');
                header = header.Trim();
                SettingPUV setting = parser.ParsePUV(header.Split(';'));

                List<ProtocolLoadDocumentPUV> ProtocolLoadingPUV = new List<ProtocolLoadDocumentPUV>();
                int i = 1;
                while (!sr.EndOfStream)
                {
                    //флаг, определяющий, требуется рабоать с БД или со списком в памяти
                    bool flagWorkWithBase;
                    
                    string line = sr.ReadLine();
                    line = line.Replace('"', ' ');
                    line = line.Replace('=', ' ');
                    line = line.Trim();

                    //получили строку из файла PUV
                    string[] columns = line.Split(';');

                    //Формируем объект Person
                    string tempSNILS = (string)GetValueFromCvs<string>(columns, setting.Column__SNILS);
                    string tempSurname = (string)GetValueFromCvs<string>(columns, setting.Column_Surname);
                    string tempName = (string)GetValueFromCvs<string>(columns, setting.Column_Name);
                    string tempPatr = (string)GetValueFromCvs<string>(columns, setting.Column_Patr);
                    DateTime? tempDateBirthday = (DateTime?)GetValueFromCvs<DateTime?>(columns, setting.Column_DateBirthday);
                    string tempPhoneNumber = (string)GetValueFromCvs<string>(columns, setting.Column_PhoneNumber);

                    Person p = new Person()
                    {
                        SNILS = tempSNILS,
                        Surname = tempSurname,
                        Name = tempName,
                        Patr = tempPatr,
                        DateBirthday = tempDateBirthday,
                        PhoneNumber = tempPhoneNumber
                    };

                    //формируем объект Statement
                    string tempGUID = (string)GetValueFromCvs<string>(columns, setting.Column_GUID);
                    string tempRegNumber = (string)GetValueFromCvs<string>(columns, setting.Column_RegNumber);
                    string tempVid_Pay = (string)GetValueFromCvs<string>(columns, setting.Column_Vid_Pay);
                    string tempOsn_Pay = (string)GetValueFromCvs<string>(columns, setting.Column_Osnovanie_Pay);
                    DateTime? tempDateInnings = (DateTime?)GetValueFromCvs<DateTime?>(columns, setting.Column_DateInnings);
                    DateTime? tempDateRegistration = (DateTime?)GetValueFromCvs<DateTime?>(columns, setting.Column_DateRegistration);
                    string tempStatus = (string)GetValueFromCvs<string>(columns, setting.Column_Status);
                    DateTime? tempDateResolved = (DateTime?)GetValueFromCvs<DateTime?>(columns, setting.Column_DateResolved);
                    string tempTypeResolved = (string)GetValueFromCvs<string>(columns, setting.Column_TypeResolved);
                    string tempRejectionReason = (string)GetValueFromCvs<string>(columns, setting.Column_RejectionReason);

                    Statement statement = new Statement()
                    {
                        GUID = tempGUID,
                        RegNumber = tempRegNumber,
                        Vid_Pay = tempVid_Pay,
                        Osn_Pay = tempOsn_Pay,
                        DateInnings = tempDateInnings,
                        DateRegistration = tempDateRegistration,
                        Status = tempStatus,
                        DateResolved = tempDateResolved,
                        TypeResolved = tempTypeResolved,
                        RejectionReason = tempRejectionReason
                    };

                    // формируем объект DocumentPUV
                    DocumentPUV documentpuv = new DocumentPUV()
                    {
                        DocumentPUVName = Path.GetFileName(SummaryArgs.filename),
                        DateLoaded = DateTime.Now
                    };

                    //проверяем, данный заявитель присутствует в БД или нет
                    var DBPerson = context.Persons.Where(c => c.SNILS == p.SNILS).Include(o => o.Statements).FirstOrDefault();
                    if (DBPerson == null)
                        flagWorkWithBase = false;
                    else
                        flagWorkWithBase = true;

                    if (flagWorkWithBase) // если заявитель уже присутствует в БД, все манипуляции будут производится с базой данных
                                          // добавление новых заявлений, перезапись статусов и т.д.
                    {
                        var FindStatement = DBPerson.Statements.Where(s => s.RegNumber == statement.RegNumber).FirstOrDefault();
                        if (FindStatement == null) //если заявления с таким рег.номером не найдено, записать его в базу данных
                        {
                            statement.DocumentsPUV.Add(documentpuv);
                            DBPerson.Statements.Add(statement);
                            context.SaveChanges();
                        }
                        else // если найдено, то проверить, нужно ли обновлять статус или нет
                        {
                            string OldStatus = FindStatement.Status;
                            string OldTypeResolved = FindStatement.TypeResolved;

                            string NewStatus = statement.Status;
                            string NewTypeResolved = statement.TypeResolved;

                            // 1-я проверка если статусы совпадают, но различаются виды решения(положительное-отрицательное и наоборот) помещаем в протокол
                            // заявлению будет выставлен статус "Требует дополнительного решения"
                            if (OldStatus == NewStatus && OldTypeResolved != NewTypeResolved)
                            {
                                ProtocolLoadingPUV.Add(new ProtocolLoadDocumentPUV
                                {
                                    RegNumber = FindStatement.RegNumber,
                                    SNILS = DBPerson.SNILS,
                                    OldStatus = OldStatus,
                                    OldTypeResolved = OldTypeResolved,
                                    NewStatus = NewStatus,
                                    NewTypeResolved = NewTypeResolved
                                });
                                NewStatus = "Требует дополнительного решения";
                            }else if   //2-я проверка, если статус заявления в БД - Решение принято, а в документе отличное от него, то выводим в протокол
                                   // заявлению будет выставлен статус "Требует дополнительного решения"
                            (OldStatus == "Решение принято" && OldStatus != NewStatus)
                            {
                                ProtocolLoadingPUV.Add(new ProtocolLoadDocumentPUV
                                {
                                    RegNumber = FindStatement.RegNumber,
                                    SNILS = DBPerson.SNILS,
                                    OldStatus = OldStatus,
                                    OldTypeResolved = OldTypeResolved,
                                    NewStatus = NewStatus,
                                    NewTypeResolved = NewTypeResolved
                                });
                                NewStatus = "Требует дополнительного решения";
                            }

                            if(OldStatus != NewStatus) //обновляем бд
                            {
                                FindStatement.Status = statement.Status;
                                FindStatement.TypeResolved = statement.TypeResolved;
                                FindStatement.DocumentsPUV.Add(documentpuv);
                                context.SaveChanges();
                            }
                        }
                    }
                    else //если заявителя нет в базе данных, требуется работать с памятью приложения
                         //после чего данные в БД добавятся одним запросом
                    {
                        var MSPerson = NewPersonList.Where(o => o.SNILS == p.SNILS).FirstOrDefault();
                        if(MSPerson == null) //если заявителя нет в списке, то добавить c заявлением
                        {
                            statement.DocumentsPUV.Add(documentpuv);
                            p.Statements.Add(statement);

                            NewPersonList.Add(p);
                        }
                        else
                        {
                            var FindStatement = MSPerson.Statements.Where(o => o.RegNumber == statement.RegNumber).FirstOrDefault();
                            if(FindStatement == null)
                            {
                                statement.DocumentsPUV.Add(documentpuv);
                                MSPerson.Statements.Add(statement);
                            }
                            else
                            {
                                string OldStatus = FindStatement.Status;
                                string OldTypeResolved = FindStatement.TypeResolved;

                                string NewStatus = statement.Status;
                                string NewTypeResolved = statement.TypeResolved;

                                // 1-я проверка если статусы совпадают, но различаются виды решения(положительное-отрицательное и наоборот) помещаем в протокол
                                // заявлению будет выставлен статус "Требует дополнительного решения"
                                if (OldStatus == NewStatus && OldTypeResolved != NewTypeResolved)
                                {
                                    ProtocolLoadingPUV.Add(new ProtocolLoadDocumentPUV
                                    {
                                        RegNumber = FindStatement.RegNumber,
                                        SNILS = MSPerson.SNILS,
                                        OldStatus = OldStatus,
                                        OldTypeResolved = OldTypeResolved,
                                        NewStatus = NewStatus,
                                        NewTypeResolved = NewTypeResolved
                                    });
                                    NewStatus = "Требует дополнительного решения";
                                }
                                else if   //2-я проверка, если статус заявления в БД - Решение принято, а в документе отличное от него, то выводим в протокол
                                          // заявлению будет выставлен статус "Требует дополнительного решения"
                               (OldStatus == "Решение принято" && OldStatus != NewStatus)
                                {
                                    ProtocolLoadingPUV.Add(new ProtocolLoadDocumentPUV
                                    {
                                        RegNumber = FindStatement.RegNumber,
                                        SNILS = MSPerson.SNILS,
                                        OldStatus = OldStatus,
                                        OldTypeResolved = OldTypeResolved,
                                        NewStatus = NewStatus,
                                        NewTypeResolved = NewTypeResolved
                                    });
                                    NewStatus = "Требует дополнительного решения";
                                }

                                if (OldStatus != NewStatus) //обновляем заявление
                                {
                                    FindStatement.Status = statement.Status;
                                    FindStatement.TypeResolved = statement.TypeResolved;
                                    FindStatement.DocumentsPUV.Add(documentpuv);
                                }
                            }
                        }
                    }

                    SummaryArgs.bw.ReportProgress(i);
                    i++;

                }

                context.Persons.AddRange(NewPersonList.ToArray());
                context.SaveChanges();

                //переместить в архив фаил
                GoToArhive(SummaryArgs.filename);

                if (ProtocolLoadingPUV.Count > 0)
                {
                    ProtocolCreater.SaveProtocolLoadDocumentPUV(ProtocolLoadingPUV);
                }
            }

           
        }

        public static void LOAD_PUV_CVS(object sender, DoWorkEventArgs e) // 1 вариант с последовательным доступом к БД(очень медленно работает)
        {
            HeaderParser parser = new HeaderParser();
            parser.LoadFromXmlSerializer();
            SummaryArgsForLoadPUV SummaryArgs = (SummaryArgsForLoadPUV)e.Argument;
            using (MyDBContext context = new MyDBContext())
            {
                StreamReader sr = new StreamReader(SummaryArgs.filename, Encoding.Default);
                //определим заголовки документа
                string header = sr.ReadLine();
                header = header.Replace('"', ' ');
                header = header.Replace('=', ' ');
                header = header.Trim();
                SettingPUV setting = parser.ParsePUV(header.Split(';'));
                List<ProtocolLoadDocumentPUV> ProtocolLoadingPUV = new List<ProtocolLoadDocumentPUV>();
                int i = 1;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    line = line.Replace('"', ' ');
                    line = line.Replace('=', ' ');
                    line = line.Trim();

                    //получили строку из файла PUV
                    string[] columns = line.Split(';');

                    //Формируем объект Person
                    string tempSNILS = (string)GetValueFromCvs<string>(columns, setting.Column__SNILS);
                    string tempSurname = (string)GetValueFromCvs<string>(columns, setting.Column_Surname);
                    string tempName = (string)GetValueFromCvs<string>(columns, setting.Column_Name);
                    string tempPatr = (string)GetValueFromCvs<string>(columns, setting.Column_Patr);
                    DateTime? tempDateBirthday = (DateTime?)GetValueFromCvs<DateTime?>(columns, setting.Column_DateBirthday);
                    string tempPhoneNumber = (string)GetValueFromCvs<string>(columns, setting.Column_PhoneNumber);

                    Person p = new Person()
                    {
                        SNILS = tempSNILS,
                        Surname = tempSurname,
                        Name = tempName,
                        Patr = tempPatr,
                        DateBirthday = tempDateBirthday,
                        PhoneNumber = tempPhoneNumber
                    };

                    //формируем объект Statement
                    string tempGUID = (string)GetValueFromCvs<string>(columns, setting.Column_GUID);
                    string tempRegNumber = (string)GetValueFromCvs<string>(columns, setting.Column_RegNumber);
                    string tempVid_Pay = (string)GetValueFromCvs<string>(columns, setting.Column_Vid_Pay);
                    string tempOsn_Pay = (string)GetValueFromCvs<string>(columns, setting.Column_Osnovanie_Pay);
                    DateTime? tempDateInnings = (DateTime?)GetValueFromCvs<DateTime?>(columns, setting.Column_DateInnings);
                    DateTime? tempDateRegistration = (DateTime?)GetValueFromCvs<DateTime?>(columns, setting.Column_DateRegistration);
                    string tempStatus = (string)GetValueFromCvs<string>(columns, setting.Column_Status);
                    DateTime? tempDateResolved = (DateTime?)GetValueFromCvs<DateTime?>(columns, setting.Column_DateResolved);
                    string tempTypeResolved = (string)GetValueFromCvs<string>(columns, setting.Column_TypeResolved);
                    string tempRejectionReason = (string)GetValueFromCvs<string>(columns, setting.Column_RejectionReason);

                    Statement statement = new Statement()
                    {
                        GUID = tempGUID,
                        RegNumber = tempRegNumber,
                        Vid_Pay = tempVid_Pay,
                        Osn_Pay = tempOsn_Pay,
                        DateInnings = tempDateInnings,
                        DateRegistration = tempDateRegistration,
                        Status = tempStatus,
                        DateResolved = tempDateResolved,
                        TypeResolved = tempTypeResolved,
                        RejectionReason = tempRejectionReason
                    };

                    // формируем объект DocumentPUV
                    DocumentPUV documentpuv = new DocumentPUV()
                    {
                        DocumentPUVName = Path.GetFileName(SummaryArgs.filename),
                        DateLoaded = DateTime.Now
                    };

                    //Проверка, есть ли такой получатель в базе данных
                    var FindObj = context.Persons.Where(c => c.SNILS == p.SNILS).Include(o => o.Statements).FirstOrDefault();
                    if (FindObj == null) //если такого заявителя нет в базе данных, требуется сохранить его
                    {

                        //так как строка в документе содержит еще информацию о заявлении, создаем заявление к новому заявителю
                        statement.DocumentsPUV.Add(documentpuv);
                        p.Statements.Add(statement);

                        context.Persons.Add(p);
                        context.SaveChanges();
                    }
                    else //если заявитель присутствует, проверяем есть ли у него такое заявление
                    {
                        var FindStatement = FindObj.Statements.Where(s => s.RegNumber == statement.RegNumber).FirstOrDefault();
                        if (FindStatement == null) //если заявления с таким рег.номером не найдено, записать его в базу данных
                        {
                            statement.DocumentsPUV.Add(documentpuv);
                            //context.Persons.Find(FindObj.PersonId).Statements.Add(statement);
                            FindObj.Statements.Add(statement);
                            context.SaveChanges();

                        }
                        else //если заявление  стаким рег. номером найдено, требуется проверить статус, возможно его нужно будет обновить
                        {
                            string OldStatus = FindStatement.Status;
                            string OldTypeResolved = FindStatement.TypeResolved;

                            string NewStatus = statement.Status;
                            string NewTypeResolved = statement.TypeResolved;

                            // 1-я проверка если статусы совпадают, но различаются виды решения(положительное-отрицательное и наоборот) помещаем в протокол
                            // заявлению будет выставлен статус "Требует дополнительного решения"
                            if (OldStatus == NewStatus && OldTypeResolved != NewTypeResolved)
                            {
                                ProtocolLoadingPUV.Add(new ProtocolLoadDocumentPUV
                                {
                                    RegNumber = FindStatement.RegNumber,
                                    SNILS = FindObj.SNILS,
                                    OldStatus = OldStatus,
                                    OldTypeResolved = OldTypeResolved,
                                    NewStatus = NewStatus,
                                    NewTypeResolved = NewTypeResolved
                                });
                                statement.Status = "Требует дополнительного решения";
                            }

                            //2-я проверка, если статус заявления в БД - Решение принято, а в документе отличное от него, то выводим в протокол
                            // заявлению будет выставлен статус "Требует дополнительного решения"
                            if (OldStatus == "Решение принято" && OldStatus != NewStatus)
                            {
                                ProtocolLoadingPUV.Add(new ProtocolLoadDocumentPUV
                                {
                                    RegNumber = FindStatement.RegNumber,
                                    SNILS = FindObj.SNILS,
                                    OldStatus = OldStatus,
                                    OldTypeResolved = OldTypeResolved,
                                    NewStatus = NewStatus,
                                    NewTypeResolved = NewTypeResolved
                                });
                                statement.Status = "Требует дополнительного решения";
                            }

                            //Обновляем заявление в БД
                            //var TargetStatement = context.Statements.Find(FindStatement.StatementId);
                            FindStatement.Status = statement.Status;  
                            FindStatement.TypeResolved = statement.TypeResolved;
                            FindStatement.DocumentsPUV.Add(documentpuv);
                            context.SaveChanges();
                        }
                    }

                    SummaryArgs.bw.ReportProgress(i);
                    i++;
                }

                //переместить в архив фаил
                GoToArhive(SummaryArgs.filename);

                if (ProtocolLoadingPUV.Count > 0)
                {
                    ProtocolCreater.SaveProtocolLoadDocumentPUV(ProtocolLoadingPUV);
                }
            }
        }

        private static void GoToArhive(string filename)
        {
            string path = Environment.CurrentDirectory + "\\Arhive";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path += "\\DocumentsPUV";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string filenamedest = path + "\\" + Path.GetFileNameWithoutExtension(filename) + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_.csv";

            try
            {
                File.Copy(filename,filenamedest);
            }
            catch
            {

            }
            
        }
    }


    public class SummaryArgsForLoadPUV
    {
        public string filename { get; set; }
        public BackgroundWorker bw { get; set; }
    }
    
}
