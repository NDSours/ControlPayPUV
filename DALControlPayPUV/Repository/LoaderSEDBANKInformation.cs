using System;
using System.Collections.Generic;
using System.IO;
using DALControlPayPUV.Entities;
using DALControlPayPUV.MySQL;

namespace DALControlPayPUV.Repository
{
    public static class LoaderSEDBANKInformation
    {
        public static string GetQuery(string filename)
        {
            string temp = default;
            string path = Environment.CurrentDirectory + "\\SQL";
            if (!Directory.Exists(path))
                throw new Exception("Отсутствует папка: " + path);

            string file = path + "\\" + filename;
            if (File.Exists(file))
            {
                temp = File.ReadAllText(file);
            }
            else
                throw new Exception("Не найден фаил: " + file);

            return temp;
        }
        public static List<PayFromSEDBANK> GetListPaySEDBANK(List<string> ListSNILS)
        {
            List<PayFromSEDBANK> List = new List<PayFromSEDBANK>();
            string partquery = "(";
            foreach (var s in ListSNILS)
                partquery += '"'+s+'"' + ',';

            partquery = partquery.Remove(partquery.Length - 1);
            partquery += ")";

            string query = GetQuery("GetSEDBankPays.sql");
            string listSnils = " recipients.SNILS in " + partquery + " ";
            query = query.Replace("<listSnils>", listSnils);            

            List<string[]> resultlist = SQLCommander.GetRowsFromDB(query);
            foreach (var item in resultlist)
            {
                DateTime temp = default;
                DateTime.TryParse(item[7], out temp);

                string temptypepay = default;
                Repository.TypesPay.TryGetValue(int.Parse(item[0]), out temptypepay);

                string tempsubtypepay = default;
                if(item[1] != "")
                   Repository.SubTypesPay.TryGetValue(int.Parse(item[1]), out tempsubtypepay);

                string tempnamedeliveryorg = default;
                if (item[9] != "")
                    Repository.DeliveryOrgs.TryGetValue(int.Parse(item[9]), out tempnamedeliveryorg);

                bool temphaveozac = false;
                string tempdescrcredited = default;
                if (item[11] != "")
                {
                    temphaveozac = true;
                    Repository.CodeCredited.TryGetValue(int.Parse(item[12]), out tempdescrcredited);
                }
                    

                //формируем объекты PayFromSEDBANK
                PayFromSEDBANK pay = new PayFromSEDBANK()
                {
                    TypePay = temptypepay,
                    SubTypePay = tempsubtypepay,
                    MonthPay = int.Parse(item[2]),
                    YearPay = int.Parse(item[3]),
                    AmountPay = decimal.Parse(item[4]),
                    AccountNumber = item[5],
                    NumberPP = item[6],
                    DatePP = temp,
                    SNILS = item[8],
                    NameDeliveryOrg = tempnamedeliveryorg,
                    Reester = int.Parse(item[10]),
                    haveOZAC = temphaveozac,
                    DescriptionCredited = tempdescrcredited
                };
                List.Add(pay);
            }

            return List;
            
        } 
    }
}
