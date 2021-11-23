using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DALControlPayPUV.Repository;
using DALControlPayPUV.Entities;

namespace ControlPayPUV.Forms
{
    public partial class Autorization : Form
    {
        private UserRepository urepos = new UserRepository();
        public User ActiveUser;
        public Autorization()
        {
            InitializeComponent();
        }

        private void Autorization_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            foreach(var u in urepos.Users)
            {
                comboBox1.Items.Add(u.Login);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void Autorization_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.DialogResult = DialogResult.Cancel;
                e.Cancel = false;
            }
            else
            {
                string Pwd = this.textBox1.Text;
                var selectedUser = urepos.Users.Where(u => u.Login == this.comboBox1.Text).FirstOrDefault();
                if (selectedUser != null)
                {
                    ActiveUser = selectedUser;
                    if (Pwd == selectedUser.Password)
                    {
                        this.DialogResult = DialogResult.OK;
                        e.Cancel = false;
                    }
                    else
                    {
                        this.label1.Visible = true;
                        e.Cancel = true;
                    }

                }
                else
                {
                    this.label1.Visible = true;
                    e.Cancel = true;
                }
            }
        }
    }
}
