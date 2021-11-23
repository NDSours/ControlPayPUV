using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlPayPUV.Forms
{
    public partial class frmWaitForm : Form
    {
        public Action Worker {get; set;}
        public frmWaitForm(Action worker)
        {
            InitializeComponent();
            progressBar1.MarqueeAnimationSpeed = 100;
            if (worker == null)
                throw new ArgumentNullException();
            else
                Worker = worker;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Task.Factory.StartNew(Worker).ContinueWith(t => { this.Close(); }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void frmWaitForm_Load(object sender, EventArgs e)
        {

        }
    }
}
