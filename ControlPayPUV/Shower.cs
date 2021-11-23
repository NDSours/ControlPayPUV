using DALControlPayPUV.Entities;
using ControlPayPUV.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DALControlPayPUV;
using DALControlPayPUV.Repository;

namespace ControlPayPUV
{
    public static class Shower
    {

        public static List<ListViewItem> StaticJornal;
        private static string _SearchSNILS;
        private static string _SearchSurname;
        
        
        public static void ShowRecipients(List<Person> Collection,ListView list)
        {
            list.Items.Clear();

            //получили новую коллекцию элементов, согласно фильтру
            LoadJornalRecipients(Collection);

            // настройка колонок
            InitialColumnsJornalRecipient(list);

            list.VirtualListSize = StaticJornal.Count();
            list.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(listView1_RetrieveVirtualItem);
            list.SearchForVirtualItem += new SearchForVirtualItemEventHandler(listViewRecipient_SearchForVirtualItem);
        }
        private static void LoadJornalRecipients(List<Person> Collection)
        {

            StaticJornal = new List<ListViewItem>();
            
            // фильтруем коллекцию, если надо (для фильтрации можно использовать LINQ)
            int counter = 1;
            foreach (var item in Collection)
            {
                StaticJornal.Add(new ListViewItem(new string[] { item.PersonId.ToString(), counter.ToString(), item.GetSNILS(), item.Surname + " " + item.Name + " " + item.Patr, DateUtils.GetDate(item.DateBirthday) }));
                counter++;
            }
        }

        private static void InitialColumnsJornalRecipient(ListView list)
        {
            if (list.Columns.Count > 0)
                return;

            list.Columns.Add("", 0, HorizontalAlignment.Center);
            list.Columns.Add("#", 35, HorizontalAlignment.Center);
            list.Columns.Add("СНИЛС", 120, HorizontalAlignment.Center);
            list.Columns.Add("Ф.И.О.", 200, HorizontalAlignment.Center);
            list.Columns.Add("Дата рождения", 100, HorizontalAlignment.Center);

            ImageList imgList = new ImageList();
            imgList.ImageSize = new System.Drawing.Size(1, 50);
            list.SmallImageList = imgList;
        }

        private static void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            e.Item = StaticJornal[e.ItemIndex];
        }

        private static void listViewRecipient_SearchForVirtualItem(object sender, SearchForVirtualItemEventArgs e)
        {
            
            if(_SearchSNILS != "")
            {
                var findItem = StaticJornal.Find(delegate (ListViewItem d) { return d.SubItems[2].Text.Contains(_SearchSNILS); });
                if (findItem != null)
                    e.Index = StaticJornal.IndexOf(findItem);
            }
        
            if(_SearchSurname != "")
            {
                var findItem2 = StaticJornal.Find(delegate (ListViewItem d) { return d.SubItems[3].Text.ToLower().Contains(_SearchSurname.ToLower()); });
                if (findItem2 != null)
                    e.Index = StaticJornal.IndexOf(findItem2);
            }
        }

        public static void Search(string SNILS, string Surname, ListView list)
        {
            _SearchSNILS = SNILS;
            _SearchSurname = Surname;

            var item = list.FindItemWithText("");
            if (item != null)
            {
                list.FocusedItem = item;
                item.Selected = true;
                item.EnsureVisible();
            }
        }

        
        public static void LoadJornalZayavlenii(Person p,ListView list)
        {
            list.Items.Clear();
            //Инициализация заголовков
            InitialColumnsListViewZayavlenii(list);

            //получение и заполнение данными 
            bool HaveInfPay = false;
            //модифицируем снилс в пуве(добавляются дифицы и пробелы)
            string modSNILS = p.GetSNILS();
            var findObj = Repository.ListPaysFromSEDBANK.Where(o => o.SNILS == modSNILS).FirstOrDefault();
            if (findObj != null)
                HaveInfPay = true;

            int counter = 1;
            foreach(var item in p.Statements)
            {
                bool HaveBZPEV = false;
                bool HaveOZPEV = false;

                if(item.DocumentsPay.Count() > 0)
                {
                    foreach(var doc in item.DocumentsPay)
                    {
                        if (doc.TypePay == "ОЗПЕВ")
                            HaveOZPEV = true;
                        else if (doc.TypePay == "БЗПЕВ")
                            HaveBZPEV = true;
                    }
                }

                //вывод первой строки с реквизитами заявления
                ListViewItem lvi = new ListViewItem();
                lvi.Text = item.StatementId.ToString();
                lvi.SubItems.Add(counter.ToString());
                lvi.SubItems.Add(item.GUID);
                lvi.SubItems.Add(item.RegNumber);
                lvi.SubItems.Add(item.Vid_Pay);
                lvi.SubItems.Add(item.Osn_Pay);
                lvi.SubItems.Add(item.Status);
                lvi.SubItems.Add(Utils.DateUtils.GetDate(item.DateInnings));
                lvi.SubItems.Add(Utils.DateUtils.GetDate(item.DateResolved));
                lvi.SubItems.Add(HaveBZPEV.GetStringFromBool());
                lvi.SubItems.Add(HaveOZPEV.GetStringFromBool());
                lvi.SubItems.Add(HaveInfPay.GetStringFromBool());

                list.Items.Add(lvi);
                
                counter++;
            }

        }
        

        private static void InitialColumnsListViewZayavlenii(ListView list)
        {
            if (list.Columns.Count > 0)
                return;

            list.Columns.Add("", 0, HorizontalAlignment.Center);
            list.Columns.Add("#", 35, HorizontalAlignment.Center);
            list.Columns.Add("GUID", 120, HorizontalAlignment.Center);
            list.Columns.Add("Регистрационный номер", 150, HorizontalAlignment.Center);
            list.Columns.Add("Вид выплаты", 200, HorizontalAlignment.Center);
            list.Columns.Add("Основание выплаты", 200, HorizontalAlignment.Center);
            list.Columns.Add("Статус заявления", 120, HorizontalAlignment.Center);
            list.Columns.Add("Дата подачи", 100, HorizontalAlignment.Center);
            list.Columns.Add("Дата решения", 100, HorizontalAlignment.Center);
            list.Columns.Add("BZPEV", 100, HorizontalAlignment.Center);
            list.Columns.Add("OZPEV", 100, HorizontalAlignment.Center);
            list.Columns.Add("Инф. по выплате", 100, HorizontalAlignment.Center);

            ImageList imgList = new ImageList();
            imgList.ImageSize = new System.Drawing.Size(1, 50);
            list.SmallImageList = imgList;
        }
    }
}
