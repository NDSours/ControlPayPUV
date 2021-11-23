using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DALControlPayPUV.MySQL;
using DALControlPayPUV.Entities;

namespace DALControlPayPUV.Repository
{
    public static class Repository
    {
        public static List<Person> AllListPerson = new List<Person>();
        public static List<Statement> AllListStatement = new List<Statement>();
        public static List<PayFromSEDBANK> ListPaysFromSEDBANK;

        //Справочники
        public static Dictionary<int, string> TypesPay = new Dictionary<int, string>();
        public static Dictionary<int, string> SubTypesPay = new Dictionary<int, string>();
        public static Dictionary<int, string> DeliveryOrgs = new Dictionary<int, string>();
        public static Dictionary<int, string> CodeCredited = new Dictionary<int, string>();

        public static Statement GetInformationWithChildren(string RegNumber)
        {
            //подгрузить информацию о детях
            using (var context = new MyDBContext())
            {
                var Statement = context.Statements.Where(o => o.RegNumber == RegNumber).Include(d => d.DocumentsPay.Select(s => s.Childs)).FirstOrDefault();
                return Statement;
            }
        }

        public static void LoadPaysFromSEDBANK()
        {

            //получить список СНИЛСов по которым требуется получить выплаты из базы SED BANK
            List<string> listsnils = new List<string>();
            using (var context = new MyDBContext())
            {
                var Collection = context.Persons.DistinctBy(o => o.SNILS);
                foreach (var item in Collection)
                    listsnils.Add(item.GetSNILS());
            }

            if(listsnils.Count > 0)
               ListPaysFromSEDBANK = LoaderSEDBANKInformation.GetListPaySEDBANK(listsnils);

        }

        public static void LoadDictionaries()
        {
            string query = "Select ID,Name from Types_pay";
            foreach(var item in SQLCommander.GetRowsFromDB(query))
            {
                TypesPay.Add(int.Parse(item[0]), item[1]);
            }

            query = "Select ID,Name from subtypes_pay";
            foreach(var item in SQLCommander.GetRowsFromDB(query))
            {
                SubTypesPay.Add(int.Parse(item[0]), item[1]);
            }

            query = "Select ID, Name from delivery_orgs";
            foreach(var item in SQLCommander.GetRowsFromDB(query))
            {
                DeliveryOrgs.Add(int.Parse(item[0]), item[1]);
            }

            query = "select ID, Description from code_credited";
            foreach(var item in SQLCommander.GetRowsFromDB(query))
            {
                CodeCredited.Add(int.Parse(item[0]), item[1]);
            }
            
        }

        public static void LoadAllListPerson()
        {
            AllListPerson.Clear();
            using (var context = new MyDBContext())
            {
                var Collection = context.Persons.Include(o => o.Statements.Select(s => s.DocumentsPay.Select(p => p.Childs))).OrderBy(o => o.Surname);
                foreach (var item in Collection)
                    AllListPerson.Add(item);
            }
        }

        
        public static void LoadAllListStatement()
        {
            AllListStatement.Clear();
            using (var context = new MyDBContext())
            {
                var Collection = context.Statements;
                foreach (var item in Collection)
                {
                    AllListStatement.Add(item);
                }
            }
        }
        

        public static List<Person> FilteredListPerson(Filter filter)
        {
            var Collection = new List<Person>();
            switch (filter.HavingFiles)
            {
                case Files.All:
                    return AllListPerson;
                case Files.HaveInformationOnPay:
                    foreach(var person in AllListPerson)
                    {
                        foreach(var st in person.Statements)
                        {
                            if (st.DocumentsPay.Count() > 0)
                            {
                                Collection.Add(person);
                                break;
                            }
                        }
                    }
                    return Collection;
                case Files.NotHaveInformationOnPay:
                    foreach (var person in AllListPerson)
                    {
                        bool flag = false;
                        foreach (var st in person.Statements)
                        {
                            if (st.DocumentsPay.Count() > 0)
                            {
                                flag = true;
                                break;
                            }
                        }

                        if (!flag)
                            Collection.Add(person);
                    }
                    return Collection;

            }
            return null;
        }
    }
}
