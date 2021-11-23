using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ControlPayPUV.Delegates;
using ControlPayPUV.Loaders.PUV;
using ControlPayPUV.Loaders.XML;


namespace ControlPayPUV.Forms
{
    public partial class ProgressForm : Form
    {
        public static BackgroundWorker bw_loaderpuv;
        public static BackgroundWorker bw_loaderxml;
  
        public ProgressForm(bool NotViewResult)
        {
            InitializeComponent();
            this.label1.Visible = false;
            this.progressBar1.Visible = false;
        }
        public ProgressForm()
        {
            InitializeComponent();
            pictureBox1.Image = Image.FromFile(Environment.CurrentDirectory + "\\images\\Water.gif");
            
            bw_loaderpuv = new BackgroundWorker();
            bw_loaderpuv.WorkerReportsProgress = true;
            bw_loaderpuv.WorkerSupportsCancellation = true;
            //bw_loaderpuv.DoWork += LoaderPUV.LOAD_PUV_CVS;
            bw_loaderpuv.DoWork += LoaderPUV.LOAD_PUV_CVS_;
            bw_loaderpuv.ProgressChanged += Bw_loaderpuv_ProgressChanged;
            bw_loaderpuv.RunWorkerCompleted += Bw_loaderpuv_RunWorkerCompleted;

            bw_loaderxml = new BackgroundWorker();
            bw_loaderxml.WorkerReportsProgress = true;
            bw_loaderxml.WorkerSupportsCancellation = true;
            bw_loaderxml.DoWork += LoaderDocumentXML.LOAD_XML_Documents;
            bw_loaderxml.ProgressChanged += Bw_loaderXML_ProgressChanged;
            bw_loaderxml.RunWorkerCompleted += Bw_loaderpuv_RunWorkerCompleted;
        }

        public void RunWorkerPUV(string filename, int maxvalueprogressbar)
        {
            progressBar1.Minimum = 1;
            progressBar1.Maximum = maxvalueprogressbar;
            bw_loaderpuv.RunWorkerAsync(new SummaryArgsForLoadPUV { filename = filename, bw = bw_loaderpuv });
        }

        public void RunWorkerXML( int maxvalueprogressbar)
        {
            progressBar1.Minimum = 1;
            progressBar1.Maximum = maxvalueprogressbar;
            bw_loaderxml.RunWorkerAsync(new SummaryArgsForLoadXML { bw = bw_loaderxml });
        }

        private void Bw_loaderpuv_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                Console.WriteLine(
                  "Работа BackgroundWorker была прервана пользователем!");
            else if (e.Error != null)
                Console.WriteLine("Worker exception: " + e.Error);
            else
                Console.WriteLine("Работа закончена успешно. Результат - "
                  + e.Result + ". ");

            this.Close();
        }

        private void Bw_loaderpuv_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = "Обработано строк: " + e.ProgressPercentage.ToString();
        }

        private void Bw_loaderXML_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = "Обработано заявлений: " + e.ProgressPercentage.ToString();
        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {

        }
    }
}
