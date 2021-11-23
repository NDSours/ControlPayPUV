using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DALControlPayPUV.Entities;
using ControlPayPUV.Utils;
using DALControlPayPUV.Repository;
using System.Diagnostics;

namespace ControlPayPUV.Forms
{
    public partial class FormPayInformation : Form
    {
        private Statement _Statement;

        public FormPayInformation(Person p, string RegNumber)
        {
            InitializeComponent();

            this.Text = p.Surname + " " + p.Name + " " + p.Patr + " СНИЛС: " + p.GetSNILS() + " Заявление: " + RegNumber;


            //подгрузить информацию о детях
            _Statement = Repository.GetInformationWithChildren(RegNumber);
            if (_Statement == null)
                throw new Exception("Не загрузилась информация по заявлению:" + RegNumber);

            ShowInformationFromStatement();
            ShowInformationFromPaySEDBANK(p.GetSNILS());
        }

        private void ShowInformationFromPaySEDBANK(string SNILS)
        {
            InitialListView3();
            listView3.Items.Clear();

            var Collection = Repository.ListPaysFromSEDBANK.Where(o => o.SNILS == SNILS).OrderBy(o => o.MonthPay);
            int counter = 1;
            foreach(var item in Collection)
            {
                ListViewItem lvi = new ListViewItem();
                if (counter % 2 == 0)
                    lvi.BackColor = Color.Aquamarine;
                else
                    lvi.BackColor = Color.GhostWhite;
                lvi.Text = counter.ToString();
                lvi.SubItems.Add(item.Reester.ToString());
                lvi.SubItems.Add(item.TypePay);
                lvi.SubItems.Add(item.SubTypePay);
                lvi.SubItems.Add(item.MonthPay.ToString());
                lvi.SubItems.Add(item.YearPay.ToString());
                lvi.SubItems.Add(item.AmountPay.ToString("c"));
                lvi.SubItems.Add(item.AccountNumber);
                lvi.SubItems.Add(item.DatePP.ToString("dd.MM.yyyy"));
                lvi.SubItems.Add(item.NumberPP);
                lvi.SubItems.Add(item.NameDeliveryOrg);
                if (item.haveOZAC)
                    lvi.SubItems.Add(item.DescriptionCredited);
                else
                    lvi.SubItems.Add("Нет информации о зачислении/незачислении");

                listView3.Items.Add(lvi);
                counter++;
            }
        }

        private void FormPayInformation_Load(object sender, EventArgs e)
        {

        }

        private void ShowInformationFromStatement()
        {
            //загрузка общей информации по заявлению (listview1)
            InitialListView1();

            foreach(var doc in _Statement.DocumentsPay)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = doc.DocumentPayId.ToString();
                lvi.SubItems.Add(doc.TypePay);
                lvi.SubItems.Add(doc.z_Surname + " " + doc.z_Name + " " + doc.z_Patr);
                lvi.SubItems.Add(doc.Phone);
                lvi.SubItems.Add(doc.NameDeliveryOrg);
                lvi.SubItems.Add(doc.BIKDeliveryOrg);
                lvi.SubItems.Add(doc.KCSchetDeliveryOrg);
                lvi.SubItems.Add(DateUtils.GetPeriodDate(doc.DateStartPay, doc.DateEndPay));
                lvi.SubItems.Add(doc.Operation);

                listView1.Items.Add(lvi);
            }

       
            //загрузка информации по детям(по умолчанию для первого документа)
            InitialListView2();
            if(_Statement.DocumentsPay.Count() > 0)
            {
                var doc = _Statement.DocumentsPay.FirstOrDefault();
                if(doc != null)
                {
                    if (doc.Childs.Count() > 0)
                        ShowInformationOnListView2(doc.Childs);
                }
            }
           
        }

        private void ShowInformationOnListView2(IEnumerable<Childs_OZPEV> list)
        {
            InitialListView2();

            foreach (var item in list)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = item.Surname + " " + item.Name + " " + item.Patr;
                lvi.SubItems.Add(DateUtils.GetDate(item.DateBirthday));
                lvi.SubItems.Add(item.SNILS);
                lvi.SubItems.Add(DateUtils.GetPeriodDate(item.DateStartPay, item.DateEndPay));

                listView2.Items.Add(lvi);
            }
  
        }

        private void InitialListView1()
        {
            listView1.Items.Clear();
            if (listView1.Columns.Count > 0)
                return;

            listView1.Columns.Add("", 0, HorizontalAlignment.Left);
            listView1.Columns.Add("Тип документа", 70, HorizontalAlignment.Center);
            listView1.Columns.Add("Ф.И.О. заявителя", 150, HorizontalAlignment.Center);
            listView1.Columns.Add("Телефон",100,HorizontalAlignment.Center);
            listView1.Columns.Add("Наименование доставочной организации", 250, HorizontalAlignment.Center);
            listView1.Columns.Add("БИК доставочной организации", 100, HorizontalAlignment.Center);
            listView1.Columns.Add("КС счёт доставочной организации", 150, HorizontalAlignment.Center);
            listView1.Columns.Add("Период выплаты", 100, HorizontalAlignment.Center);
            listView1.Columns.Add("Тип операции", 60, HorizontalAlignment.Center);
            

            ImageList imgList = new ImageList();
            imgList.ImageSize = new System.Drawing.Size(1, 50);
            listView1.SmallImageList = imgList;
        }

        private void InitialListView2()
        {
            listView2.Items.Clear();
            if (listView2.Columns.Count > 0)
                return;

            listView2.Columns.Add("Ф.И.О. ребёнка", 150, HorizontalAlignment.Center);
            listView2.Columns.Add("Дата рождения", 100, HorizontalAlignment.Center);
            listView2.Columns.Add("СНИЛС ребёнка", 100, HorizontalAlignment.Center);
            listView2.Columns.Add("Период выплаты", 100, HorizontalAlignment.Center);
            

            ImageList imgList = new ImageList();
            imgList.ImageSize = new System.Drawing.Size(1, 50);
            listView2.SmallImageList = imgList;
        }

        private void InitialListView3()
        {
            if (listView3.Columns.Count > 0)
                return;

            listView3.Columns.Add("#", 50, HorizontalAlignment.Center);
            listView3.Columns.Add("Номер реестра", 100, HorizontalAlignment.Center);
            listView3.Columns.Add("Тип выплаты", 150, HorizontalAlignment.Center);
            listView3.Columns.Add("Подтип выплаты", 150, HorizontalAlignment.Center);
            listView3.Columns.Add("Месяц", 50, HorizontalAlignment.Center);
            listView3.Columns.Add("Год", 50, HorizontalAlignment.Center);
            listView3.Columns.Add("Сумма выплаты", 100, HorizontalAlignment.Center);
            listView3.Columns.Add("Номер счета", 100, HorizontalAlignment.Center);
            listView3.Columns.Add("Дата П/П", 100, HorizontalAlignment.Center);
            listView3.Columns.Add("Номер П/П", 100, HorizontalAlignment.Center);
            listView3.Columns.Add("Банк", 300, HorizontalAlignment.Center);
            listView3.Columns.Add("Зачисление/Незачисление", 300, HorizontalAlignment.Center);

            ImageList imgList = new ImageList();
            imgList.ImageSize = new System.Drawing.Size(1, 50);
            listView3.SmallImageList = imgList;
        }

        private void открытьФаилToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Main.ActiveUser.Role != 1)
            {
                MessageBox.Show("Не достаточно прав для выполнения операции", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //получить выделенный элемент из _Statement.DocumentsPay
            var selectedItem = listView1.SelectedItems;
            if(selectedItem != null)
            {
                int selectedID = int.Parse(selectedItem[0].Text);
                var obj = _Statement.DocumentsPay.Where(d => d.DocumentPayId == selectedID).FirstOrDefault();
                if(obj != null)
                {
                    Process.Start(obj.ArhivefullPath);
                }
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            var selectedItem = listView1.SelectedItems;
            if (selectedItem != null)
            {
                int selectedID = int.Parse(selectedItem[0].Text);
                var obj = _Statement.DocumentsPay.Where(d => d.DocumentPayId == selectedID).FirstOrDefault();
                if(obj != null)
                {
                    ShowInformationOnListView2(obj.Childs);
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void удалитьЗаявлениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Main.ActiveUser.Role != 1)
            {
                MessageBox.Show("Не достаточно прав для выполнения операции", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //получить выделенный элемент из _Statement.DocumentsPay
            var selectedItem = listView1.SelectedItems;
            if (selectedItem != null)
            {
                int selectedID = int.Parse(selectedItem[0].Text);
                var obj = _Statement.DocumentsPay.Where(d => d.DocumentPayId == selectedID).FirstOrDefault();
                if (obj != null)
                {
                    RepositoryDocumentPay rep = new RepositoryDocumentPay();
                    rep.Delete(obj.DocumentPayId);

                    //удалить заявление из коллекции
                    _Statement.DocumentsPay.Remove(obj);
                    //обновить отображение заявлений
                    ShowInformationFromStatement();
                }
            }
        }
    }
}
