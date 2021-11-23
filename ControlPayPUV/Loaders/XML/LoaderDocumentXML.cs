using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DALControlPayPUV.Entities;
using System.IO;
using System.ComponentModel;
using ControlPayPUV.Protocols;
using DALControlPayPUV.Repository;

namespace ControlPayPUV.Loaders.XML
{
    public static class LoaderDocumentXML
    {
        
        public static void LOAD_XML_Documents(object sender, DoWorkEventArgs e)
        {
            //получить список всех GUID - Тип решения
            //далее ищется фаил по GUID в определенном каталоге
            //если тип решения положительное, то загружаем его в БД, если решение отрицательное - выводим данное заявление в протокол, 
            //переносим фаил BZPEV/OZPEV в отдельную дирректорию для таких файлов, так же если решение требует доработки, переносим в отдельную 
            //дирректорию и в протокол

            SummaryArgsForLoadXML myargs = (SummaryArgsForLoadXML)e.Argument;
            
            string pathExceptionFiles = Environment.CurrentDirectory + "\\ExceptionFiles";
            if (!Directory.Exists(pathExceptionFiles))
                Directory.CreateDirectory(pathExceptionFiles);

            string pathSearchingFiles = Environment.CurrentDirectory + "\\PathForLOAD";
            if(!Directory.Exists(pathSearchingFiles))
                throw new Exception("Не найдена дирректория с файлами для загрузки: " + pathSearchingFiles);

            //получить список всех GUID загруженных в БД
            Dictionary<string,bool> DictionaryGUID= new Dictionary<string,bool>();

            Repository.LoadAllListStatement();
            foreach(var statement in Repository.AllListStatement)
            {
                if (statement.Status == "Решение принято" && statement.TypeResolved == "Назначено")
                    DictionaryGUID.Add(statement.GUID, true);
                else if (statement.Status == "Решение принято" && statement.RejectionReason == "")
                    DictionaryGUID.Add(statement.GUID, true);
                else if (statement.Status == "Решение принято" && statement.TypeResolved == "Отказано в назначении")
                    DictionaryGUID.Add(statement.GUID, false);
                else if (statement.Status != "Решение принято")
                    DictionaryGUID.Add(statement.GUID, false);
                else if (statement.Status == "Решение принято" && statement.RejectionReason != "")
                    DictionaryGUID.Add(statement.GUID, false);
                else
                    throw new Exception("Не возможно определить положительное или отрицательное решение принято по заявлению: " + statement.ToString());
            }

            //Получить список всех файлов в папке для загрузки
            string[] files = Directory.GetFiles(pathSearchingFiles, "*.XML");

            //протокол загрузки
            List<ProtocolLoadDocumentXML> Protocol = new List<ProtocolLoadDocumentXML>();

            //Проход по всем файлам и загрузка информации
            int i = 1;
            foreach(var itemdictionary in DictionaryGUID)
            {
                string GUIDWithoutDef = itemdictionary.Key.Replace("-", "");
                var findfile = files.FirstOrDefault(o => o.Contains(GUIDWithoutDef));
                if(findfile != null)
                {
                    if(itemdictionary.Value == true) // если положительное решение
                    {
                        //загрузить найденный документ
                        ParseAndSaveDocumentXML(findfile);
                    }
                    else
                    {
                        //переместить фаил в папку для файлов с ошибками и добавить информацию в протокол
                        File.Move(findfile, pathExceptionFiles + Path.GetFileName(findfile));

                        //поместить информацию в протокол
                        string tempRegNumber = default;
                        string tempSNILS = default;
                        string tempStatus = default;
                        string tempTypeResolved = default;
                    
                        var findstatement = Repository.AllListStatement.Find(delegate (Statement d) { return d.GUID == itemdictionary.Key; });
                        if(findstatement != null)
                        {
                            tempRegNumber = findstatement.RegNumber;
                            tempStatus = findstatement.Status;
                            tempTypeResolved = findstatement.TypeResolved;

                            var findobj = Repository.AllListPerson.Find(delegate (Person d) { return d.PersonId == findstatement.PersonId; });
                            if(findobj != null)
                            {
                                tempSNILS = findobj.SNILS;
                            }
                        }

                        Protocol.Add(new ProtocolLoadDocumentXML { GUID = itemdictionary.Key, RegNumber = tempRegNumber, SNILS = tempSNILS, Status = tempStatus, TypeResolved = tempTypeResolved });

                    }
                }

                myargs.bw.ReportProgress(i);
                i++;
            }

            //сохраняем протокол
            if (Protocol.Count > 0)
                ProtocolLoadDocumentXML.SaveProtocolLoadDocumentXML(Protocol);
             
        }

        private static void ParseAndSaveDocumentXML(string filename)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.Load(filename);
            }
            catch
            {
                return; // переходит к следующему документу
            }
            
            XmlElement xRoot = xDoc.DocumentElement;

            string l_DocumentName = Path.GetFileName(filename);
            
            string l_DocumentType = default;
            try { l_DocumentType = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:ТипВыплаты"].InnerText; }
            catch { }

            string l_ZSurname = default;
            try { l_ZSurname = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:Заявитель"]["УТ2:ФИО"]["УТ2:Фамилия"].InnerText; }
            catch { }

            string l_ZName = default;
            try { l_ZName = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:Заявитель"]["УТ2:ФИО"]["УТ2:Имя"].InnerText; }
            catch { }

            string l_ZPatr = default;
            try { l_ZPatr = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:Заявитель"]["УТ2:ФИО"]["УТ2:Отчество"].InnerText; }
            catch { }

            string l_ZSNILS = default;
            try { l_ZSNILS = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:Заявитель"]["УТ2:СНИЛС"].InnerText; }
            catch { }

            string l_UDTYPE = default;
            try { l_UDTYPE = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:Заявитель"]["УТ2:УдостоверяющийДокумент"]["УТ2:Тип"].InnerText; }
            catch { }

            string l_UDSerialNumber = default;
            try { l_UDSerialNumber = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:Заявитель"]["УТ2:УдостоверяющийДокумент"]["УТ2:Серия"].InnerText; }
            catch { }

            string l_UDNumber = default;
            try { l_UDNumber = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:Заявитель"]["УТ2:УдостоверяющийДокумент"]["УТ2:Номер"].InnerText; }
            catch { }

            DateTime l_UDDateVidachi = default;
            try
            {
                string tempStringDataVidachi = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:Заявитель"]["УТ2:УдостоверяющийДокумент"]["УТ2:ДатаВыдачи"].InnerText;
                DateTime.TryParse(tempStringDataVidachi, out l_UDDateVidachi);
            }
            catch { }

            string l_UDKemVidan = default;
            try { l_UDKemVidan = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:Заявитель"]["УТ2:УдостоверяющийДокумент"]["УТ2:КемВыдан"].InnerText; }
            catch { }

            string l_UDKOD = default;
            try { l_UDKOD = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:Заявитель"]["УТ2:УдостоверяющийДокумент"]["УТ2:КодПодразделения"].InnerText; }
            catch { }

            string l_Adress = default;
            try { l_Adress = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:Заявитель"]["ns0:Адрес"]["УТ2:Неструктурированный"].InnerText; }
            catch { }

            string l_Phone = default;
            try { l_Phone = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:Заявитель"]["УТ2:Телефоны"]["УТ2:Телефон"].InnerText; }
            catch { }

            string l_Email = default;
            try { l_Email = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:Заявитель"]["УТ2:АдресЭлПочты"].InnerText; }
            catch { }

            DateTime l_DateStartPay = default;
            DateTime l_DateEndPay = default;
            if (l_DocumentType == "БЗПЕВ")
            {
                try
                {
                    string tempStringDateStartPay = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:Заявитель"]["ns0:ПроектРешения"]["ns0:ПериодВыплаты"]["ns0:НачалоВыплаты"].InnerText;
                    DateTime.TryParse(tempStringDateStartPay, out l_DateStartPay);
                }
                catch { }

                try
                {
                    string tempStringDateEndPay = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:Заявитель"]["ns0:ПроектРешения"]["ns0:ПериодВыплаты"]["ns0:КонецВыплаты"].InnerText;
                    DateTime.TryParse(tempStringDateEndPay, out l_DateEndPay);
                }
                catch { }
            }
            else if(l_DocumentType == "ОЗПЕВ")
            {
                // периода выплаты у файлов ОЗПЕВ нету
                // поскольку период выплаты сформирован для каждого ребенка
            }


            DateTime l_DateFirstPay = default;
            try
            {
                string tempStringDateFirstPay = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:Заявитель"]["ns0:ПроектРешения"]["ns0:ДатаПервойВыплаты"].InnerText;
                DateTime.TryParse(tempStringDateFirstPay, out l_DateFirstPay);
            }
            catch { }

            DateTime l_DateLastPay = default;
            try
            {
                string tempStringDateLastPay = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:Заявитель"]["ns0:ПроектРешения"]["ns0:ДатаПрекращенияВыплаты"].InnerText;
                DateTime.TryParse(tempStringDateLastPay, out l_DateLastPay);
            }
            catch { }

            string l_Operation = default;
            try { l_Operation = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:Заявитель"]["ns0:ПроектРешения"]["ns0:ОперацияПоЗаявлению"].InnerText; }
            catch { }

            decimal l_AmountPay = default;
            try
            {
                string tempStringAmountPay = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:Заявитель"]["ns0:ПроектРешения"]["ns0:РазмерВыплаты"].InnerText;
                tempStringAmountPay = tempStringAmountPay.Replace('.', ',');
                decimal.TryParse(tempStringAmountPay, out l_AmountPay);
            }
            catch { }

            string l_NameDeliveryOrg = default;
            try { l_NameDeliveryOrg = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:РеквизитыПеречисления"]["ns0:Банк"]["УТ2:Наименование"].InnerText; }
            catch { }

            string l_BIKDeliveryOrg = default;
            try { l_BIKDeliveryOrg = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:РеквизитыПеречисления"]["ns0:Банк"]["УТ2:БИК"].InnerText; }
            catch { }

            string l_KCSchetDeliveryOrg = default;
            try { l_KCSchetDeliveryOrg = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:РеквизитыПеречисления"]["ns0:Банк"]["УТ2:КСчет"].InnerText; }
            catch { }

            string l_PSurname = default;
            try { l_PSurname = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:РеквизитыПеречисления"]["ns0:Получатель"]["УТ2:ФИО"]["УТ2:Фамилия"].InnerText; }
            catch { }

            string l_PName = default;
            try { l_PName = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:РеквизитыПеречисления"]["ns0:Получатель"]["УТ2:ФИО"]["УТ2:Имя"].InnerText; }
            catch { }

            string l_PPatr = default;
            try { l_PPatr = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:РеквизитыПеречисления"]["ns0:Получатель"]["УТ2:ФИО"]["УТ2:Отчество"].InnerText; }
            catch { }

            string l_PAccountNumber = default;
            try { l_PAccountNumber = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:РеквизитыПеречисления"]["ns0:Получатель"]["ns0:НомерБанковскогоСчета"].InnerText; }
            catch { }

            string l_GUID = default;
            try { l_GUID = xDoc["ns0:ЭДПФР"]["ns0:СлужебнаяИнформация"]["АФ4:GUID"].InnerText; }
            catch { }

            string l_GlobalID = default;
            try { l_GlobalID = xDoc["ns0:ЭДПФР"]["ns0:СлужебнаяИнформация"]["ns0:GlobalProcessID"].InnerText; }
            catch { }

            string l_RegNumber = default;
            try { l_RegNumber = xDoc["ns0:ЭДПФР"]["ns0:СлужебнаяИнформация"]["ns0:РегномерУВКиП"].InnerText; }
            catch { }

            List<Childs_OZPEV> listChilds = new List<Childs_OZPEV>();
            if (l_DocumentType == "ОЗПЕВ")
            {
                //загрузить список получателей - детей
                var ElementChild = xDoc["ns0:ЭДПФР"]["ns0:НЗПЕВ"]["ns0:Дети"];
                foreach (XmlNode item in ElementChild.ChildNodes)
                {
                    string temp_child_Surname = default;
                    try { temp_child_Surname = item["УТ2:ФИО"]["УТ2:Фамилия"].InnerText; }
                    catch { }

                    string temp_child_Name = default;
                    try { temp_child_Name = item["УТ2:ФИО"]["УТ2:Имя"].InnerText; }
                    catch { }

                    string temp_child_Patr = default;
                    try { temp_child_Patr = item["УТ2:ФИО"]["УТ2:Отчество"].InnerText; }
                    catch { }

                    DateTime temp_child_actDate = default;
                    try
                    {
                        string temp_childStringactDate = item["ns0:Акт"]["ns0:Реквизиты"]["УТ2:Дата"].InnerText;
                        DateTime.TryParse(temp_childStringactDate, out temp_child_actDate);
                    }
                    catch { }

                    string temp_child_actNumber = default;
                    try { temp_child_actNumber = item["ns0:Акт"]["ns0:Реквизиты"]["УТ2:Номер"].InnerText; }
                    catch { }

                    string temp_child_act_ZaksName = default;
                    try { temp_child_act_ZaksName = item["ns0:Акт"]["ns0:ЗАГС"].InnerText; }
                    catch { }

                    DateTime temp_child_DateBirthday = default;
                    try
                    {
                        string temp_childStringDateBirthday = item["УТ2:ДатаРождения"].InnerText;
                        DateTime.TryParse(temp_childStringDateBirthday, out temp_child_DateBirthday);
                    }
                    catch { }

                    string temp_child_SNILS = default;
                    try { temp_child_SNILS = item["УТ2:СНИЛС"].InnerText; }
                    catch { }

                    DateTime temp_child_DateStartPay = default;
                    try
                    {
                        string temp_childStringDateStartPay = item["ns0:ПроектРешения"]["ns0:ПериодВыплаты"]["ns0:НачалоВыплаты"].InnerText;
                        DateTime.TryParse(temp_childStringDateStartPay, out temp_child_DateStartPay);
                    }
                    catch { }

                    DateTime temp_child_DateEndPay = default;
                    try
                    {
                        string temp_childStringDateEndPay = item["ns0:ПроектРешения"]["ns0:ПериодВыплаты"]["ns0:КонецВыплаты"].InnerText;
                        DateTime.TryParse(temp_childStringDateEndPay, out temp_child_DateEndPay);
                    }
                    catch { }

                    Childs_OZPEV child = new Childs_OZPEV
                    {
                        act_NameZaks = temp_child_act_ZaksName,
                        act_Number = temp_child_actNumber,
                        DateEndPay = temp_child_DateEndPay,
                        DateStartPay = temp_child_DateStartPay,
                        act_Data = temp_child_actDate,
                        DateBirthday = temp_child_DateBirthday,
                        Name = temp_child_Name,
                        SNILS = temp_child_SNILS,
                        Surname = temp_child_Surname,
                        Patr = temp_child_Patr
                    };

                    listChilds.Add(child);
                }
            }

            //создаем объект DocumentXML
            DocumentPay document = new DocumentPay
            {
                Address = l_Adress,
                AmountPay = l_AmountPay,
                BIKDeliveryOrg = l_BIKDeliveryOrg,
                DateEndPay = l_DateEndPay,
                DateFirstPay = l_DateFirstPay,
                DateLastPay = l_DateLastPay,
                DateStartPay = l_DateStartPay,
                DocumentName = l_DocumentName,
                Email = l_Email,
                GlobalID = l_GlobalID,
                GUID = l_GUID,
                KCSchetDeliveryOrg = l_KCSchetDeliveryOrg,
                NameDeliveryOrg = l_NameDeliveryOrg,
                Operation = l_Operation,
                Phone = l_Phone,
                p_AccountNumber = l_PAccountNumber,
                p_Name = l_PName,
                p_Surname = l_PSurname,
                p_Patr = l_PPatr,
                RegNumber = l_RegNumber,
                TypePay = l_DocumentType,
                ud_DataVidachi = l_UDDateVidachi,
                ud_Kod = l_UDKOD,
                ud_Number = l_UDNumber,
                ud_Seriya = l_UDSerialNumber,
                ud_Type = l_UDTYPE,
                ud_Vidan = l_UDKemVidan,
                z_Name = l_ZName,
                z_Patr = l_ZPatr,
                z_SNILS = l_ZSNILS,
                z_Surname = l_ZSurname,
                ArhivefullPath = GetPathArhive() + "\\" + Path.GetFileName(filename)
            };

            //помещаем в документ объекты - ребенок
            foreach (var child in listChilds)
                document.Childs.Add(child);

            //сохраняем в БД полученную информацию
            using (var context = new MyDBContext())
            {
                var findStatement = context.Statements.Where(o => o.RegNumber == document.RegNumber).FirstOrDefault();
                if (findStatement != null)
                {
                    //Проверка есть ли данный документ в бд
                    //Если есть и он такой же, то перезаписать
                    //Если нет, то добавить новый
                    var finddocumentpay = context.DocumentsPay.Where(d => d.RegNumber == document.RegNumber).FirstOrDefault();
                    if(finddocumentpay != null) //если документ  стаким рег номером ранее был загружен
                    {
                        if(document.Equals(finddocumentpay)) // если это заявление с той же операцией и сроком С ПО, обновляем данные о детях
                        {
                            foreach(var item in document.Childs)
                            {
                                var findchild = finddocumentpay.Childs.Where(c => c.SNILS == item.SNILS).FirstOrDefault();
                                if(findchild == null) // если такого ребенка не было в бд, добавляем
                                {
                                    finddocumentpay.Childs.Add(item);
                                }
                            }
                            //обновляем БД
                            context.SaveChangesAsync();
                        }
                        else // если отличается, добавляем как новое заявление
                        {
                            findStatement.DocumentsPay.Add(document);
                            context.SaveChangesAsync();
                        }
                    }
                    else // если документ с таким рег номером ранее не был загружен, добавляем как новое заявление
                    {
                        findStatement.DocumentsPay.Add(document);
                        context.SaveChangesAsync();
                    }  
                }
                else
                {
                    throw new Exception("Не найдено заявления в БД с рег номером: " + document.RegNumber);
                }
            }

            GoToArhive(filename);
        }

        private static string GetPathArhive()
        {
            string path = Environment.CurrentDirectory + "\\Arhive";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path += "\\DocumentsXML";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path += "\\" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString(); 
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }


        private static void GoToArhive(string filename)
        {

            string path = GetPathArhive();
            string filedest = path + "\\" + Path.GetFileName(filename);
            if (File.Exists(filedest))
            {
                filedest = Path.GetDirectoryName(filedest) + "\\" + Path.GetFileNameWithoutExtension(filedest) + "_" + Guid.NewGuid() + ".XML";
            }

            try
            {
                File.Move(filename, filedest);
            }
            catch
            {

            }
        }

        private static string ReadTeg(XmlElement root, string teg)
        {

            string temp = default;
            try
            {
                var elements = root.GetElementsByTagName(teg);
                if (elements != null)
                {
                    temp = elements[0].InnerText;
                }
            }
            catch
            {

            }

            return temp;
        }
     
    }

    public class SummaryArgsForLoadXML
    {
        public BackgroundWorker bw { get; set; }
    }
}
