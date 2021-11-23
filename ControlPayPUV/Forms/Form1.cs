using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ControlPayPUV.Forms;
using ControlPayPUV.Loaders.PUV;
using DALControlPayPUV.Entities;
using DALControlPayPUV;
using DALControlPayPUV.Repository;


namespace ControlPayPUV
{
    public partial class Main : Form
    {

        public static SettingPUV SettingPUV = new SettingPUV();
        public List<ListViewItem> Jornal;
        ProgressForm form = new ProgressForm();
        private static long SelectedPersonID;
        public static User ActiveUser;
        public Main(User user)
        {
            InitializeComponent();
            ActiveUser = user;
            //загрузка файла настроек
            if (File.Exists(Environment.CurrentDirectory + "\\setting.xml"))
                SettingPUV.LoadFromXmlSerializer();
            else
                SettingPUV.SaveToXmlSerializer();
        }

        private void загрузитьФайлPUVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(ActiveUser.Role != 1)
            {
                MessageBox.Show("Не достаточно прав для выполнения операции", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.CurrentDirectory;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                int maxi = File.ReadAllLines(ofd.FileName).Length;
                form.RunWorkerPUV(ofd.FileName,maxi);
                form.ShowDialog();

                //после загрузки обновить данные журнала
                using (frmWaitForm frm = new frmWaitForm(Repository.LoadPaysFromSEDBANK))
                {
                    frm.ShowDialog(this);
                }
                Repository.LoadAllListPerson();
                Repository.LoadAllListStatement();
                LoadJournalRecipients();
            }
        }

        private void загрузитьФайлыПоВыплатеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveUser.Role != 1)
            {
                MessageBox.Show("Не достаточно прав для выполнения операции", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //определить максимальное колличество заявлений, которые будут обработаны
            int maxi = 0;
            using(var context = new MyDBContext())
            {
                maxi = context.Statements.Count();
            }

            form.RunWorkerXML(maxi);
            form.ShowDialog();
            Repository.LoadAllListPerson();
            Repository.LoadAllListStatement();
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadJournalRecipients();
        }

      
        private void LoadJournalRecipients()
        {

            
            //требуется отфильтровать значения, потом вызвать Shower
            Filter filter = new Filter();
            switch (cb_HavingFiles.SelectedIndex)
            {
                case 0:
                    filter.HavingFiles = Files.All;
                    break;
                case 1:
                    filter.HavingFiles = Files.HaveInformationOnPay;
                    break;
                case 2:
                    filter.HavingFiles = Files.NotHaveInformationOnPay;
                    break;
                default: 
                    filter.HavingFiles = Files.All;
                    break;
            }
           
            Shower.ShowRecipients(Repository.FilteredListPerson(filter), listViewRecipient); 
        }

       

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
                   
            //загружаем данные из БД
            Repository.LoadAllListPerson();
            LoadJournalRecipients();


            cb_HavingFiles.Items.Clear();
            cb_HavingFiles.Items.Add("Все");
            cb_HavingFiles.Items.Add("Есть выплатная инф.");
            cb_HavingFiles.Items.Add("Нет выплатной инф.");
            cb_HavingFiles.SelectedIndex = 0;

            //Загрузка справочников из БД SED BANK
            try
            {
                Repository.LoadDictionaries();
            }
            catch
            {
                MessageBox.Show("Справочники не были загружены из БД SED BANK","Внимание!",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

            //запуск в потоке с ожиданием загрузку информации из бд
            using(frmWaitForm frm = new frmWaitForm(Repository.LoadPaysFromSEDBANK))
            {
                frm.ShowDialog(this);
            }

            
        }
        private void listViewRecipient_DoubleClick_1(object sender, EventArgs e)
        {
            //Загрузить все заявления по этому СНИЛС
            for (int i = 0; i < listViewRecipient.SelectedIndices.Count; i++)
            {
                int Index = listViewRecipient.SelectedIndices[i];
                var p = Repository.AllListPerson.Where(o => o.PersonId == int.Parse(Shower.StaticJornal[Index].Text)).FirstOrDefault();
                if(p != null)
                {
                    SelectedPersonID = p.PersonId;
                    Shower.LoadJornalZayavlenii(p, listViewZayavleniya);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Shower.Search(tb_SNILS.Text, tb_Surname.Text, listViewRecipient);
        }

        private void сформироватьПротоколВсеЗаявителиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filetemplate = Environment.CurrentDirectory + "\\Templates\\Template1.xlsx";
            if (File.Exists(filetemplate))
            {
                //Report1.GetReport(filetemplate,Files.AllFiles);
                MessageBox.Show("Протокол сформирован");
            }
            else
                MessageBox.Show("Отсутствует шаблон: " + filetemplate);
        }

        private void сформироватьПротоколЗаявителиСИмеющимсяФайломOZPEVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string filetemplate = Environment.CurrentDirectory + "\\Templates\\Template1.xlsx";
            //Filter filter = new Filter();
            //filter.HavingFiles = Files.HaveInformationOnPay;
            //if (File.Exists(filetemplate))
            //{
            //    Report1.GetReport(filetemplate, filter);
            //    MessageBox.Show("Протокол сформирован");
            //}
            //else
            //    MessageBox.Show("Отсутствует шаблон: " + filetemplate);
        }

        private void сформироватьПротоколЗаявителиНаКоторыхНетФайлаOZPEVToolStripMenuItem_Click(object sender, EventArgs e)
        {
           //string filetemplate = Environment.CurrentDirectory + "\\Templates\\Template1.xlsx";
           //if (File.Exists(filetemplate))
           // {
           //     //Report1.GetReport(filetemplate, Files.OZPEVIsNotHave);
           //     MessageBox.Show("Протокол сформирован");
           // }
           // else
           //     MessageBox.Show("Отсутствует шаблон: " + filetemplate);
        }

        private void listViewRecipient_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listViewZayavleniya_DoubleClick(object sender, EventArgs e)
        {
            //Загрузить все заявления по этому СНИЛС
            foreach(ListViewItem item in listViewZayavleniya.SelectedItems)
            {
                string StatementID = item.Text;
                var p = Repository.AllListPerson.Where(o => o.PersonId == SelectedPersonID).FirstOrDefault();
                var s = p.Statements.Where(o => o.StatementId == int.Parse(StatementID)).FirstOrDefault();
                if(p != null && s != null)
                {
                    FormPayInformation form = new FormPayInformation(p, s.RegNumber);
                    form.Show();
                }

            }
        }
    }
}
