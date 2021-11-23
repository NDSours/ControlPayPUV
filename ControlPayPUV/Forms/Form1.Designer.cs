
namespace ControlPayPUV
{
    partial class Main
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.менюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.операцииToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьФайлPUVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьФайлыПоВыплатеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cb_HavingFiles = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tb_Surname = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tb_SNILS = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.listViewRecipient = new System.Windows.Forms.ListView();
            this.listViewZayavleniya = new System.Windows.Forms.ListView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.менюToolStripMenuItem,
            this.оПрограммеToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1175, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // менюToolStripMenuItem
            // 
            this.менюToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.операцииToolStripMenuItem});
            this.менюToolStripMenuItem.Name = "менюToolStripMenuItem";
            this.менюToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.менюToolStripMenuItem.Text = "Меню";
            // 
            // операцииToolStripMenuItem
            // 
            this.операцииToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.загрузитьФайлPUVToolStripMenuItem,
            this.загрузитьФайлыПоВыплатеToolStripMenuItem});
            this.операцииToolStripMenuItem.Name = "операцииToolStripMenuItem";
            this.операцииToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.операцииToolStripMenuItem.Text = "Операции";
            // 
            // загрузитьФайлPUVToolStripMenuItem
            // 
            this.загрузитьФайлPUVToolStripMenuItem.Name = "загрузитьФайлPUVToolStripMenuItem";
            this.загрузитьФайлPUVToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.загрузитьФайлPUVToolStripMenuItem.Text = "Загрузить файл PUV";
            this.загрузитьФайлPUVToolStripMenuItem.Click += new System.EventHandler(this.загрузитьФайлPUVToolStripMenuItem_Click);
            // 
            // загрузитьФайлыПоВыплатеToolStripMenuItem
            // 
            this.загрузитьФайлыПоВыплатеToolStripMenuItem.Name = "загрузитьФайлыПоВыплатеToolStripMenuItem";
            this.загрузитьФайлыПоВыплатеToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.загрузитьФайлыПоВыплатеToolStripMenuItem.Text = "Загрузить файлы по выплате";
            this.загрузитьФайлыПоВыплатеToolStripMenuItem.Click += new System.EventHandler(this.загрузитьФайлыПоВыплатеToolStripMenuItem_Click);
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.оПрограммеToolStripMenuItem.Text = "О программе";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1175, 82);
            this.panel1.TabIndex = 1;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Location = new System.Drawing.Point(168, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(554, 79);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Фильтры";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cb_HavingFiles);
            this.groupBox5.Location = new System.Drawing.Point(11, 13);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(172, 57);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Режим отображения";
            // 
            // cb_HavingFiles
            // 
            this.cb_HavingFiles.FormattingEnabled = true;
            this.cb_HavingFiles.Location = new System.Drawing.Point(11, 19);
            this.cb_HavingFiles.Name = "cb_HavingFiles";
            this.cb_HavingFiles.Size = new System.Drawing.Size(151, 21);
            this.cb_HavingFiles.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox2.Location = new System.Drawing.Point(728, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(447, 82);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Поиск";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tb_Surname);
            this.groupBox3.Location = new System.Drawing.Point(164, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(152, 38);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Фамилия";
            // 
            // tb_Surname
            // 
            this.tb_Surname.Location = new System.Drawing.Point(6, 12);
            this.tb_Surname.Name = "tb_Surname";
            this.tb_Surname.Size = new System.Drawing.Size(132, 20);
            this.tb_Surname.TabIndex = 0;
            this.tb_Surname.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tb_SNILS);
            this.groupBox1.Location = new System.Drawing.Point(6, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(152, 38);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "СНИЛС";
            // 
            // tb_SNILS
            // 
            this.tb_SNILS.Location = new System.Drawing.Point(6, 12);
            this.tb_SNILS.Name = "tb_SNILS";
            this.tb_SNILS.Size = new System.Drawing.Size(132, 20);
            this.tb_SNILS.TabIndex = 0;
            this.tb_SNILS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(333, 29);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "найти";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(134, 43);
            this.button1.TabIndex = 0;
            this.button1.Text = "Загрузить журнал";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listViewRecipient
            // 
            this.listViewRecipient.Dock = System.Windows.Forms.DockStyle.Left;
            this.listViewRecipient.FullRowSelect = true;
            this.listViewRecipient.GridLines = true;
            this.listViewRecipient.HideSelection = false;
            this.listViewRecipient.Location = new System.Drawing.Point(0, 106);
            this.listViewRecipient.Name = "listViewRecipient";
            this.listViewRecipient.Size = new System.Drawing.Size(457, 467);
            this.listViewRecipient.TabIndex = 5;
            this.listViewRecipient.UseCompatibleStateImageBehavior = false;
            this.listViewRecipient.View = System.Windows.Forms.View.Details;
            this.listViewRecipient.VirtualMode = true;
            this.listViewRecipient.SelectedIndexChanged += new System.EventHandler(this.listViewRecipient_SelectedIndexChanged);
            this.listViewRecipient.DoubleClick += new System.EventHandler(this.listViewRecipient_DoubleClick_1);
            // 
            // listViewZayavleniya
            // 
            this.listViewZayavleniya.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewZayavleniya.FullRowSelect = true;
            this.listViewZayavleniya.GridLines = true;
            this.listViewZayavleniya.HideSelection = false;
            this.listViewZayavleniya.Location = new System.Drawing.Point(467, 106);
            this.listViewZayavleniya.Name = "listViewZayavleniya";
            this.listViewZayavleniya.Size = new System.Drawing.Size(708, 467);
            this.listViewZayavleniya.TabIndex = 6;
            this.listViewZayavleniya.UseCompatibleStateImageBehavior = false;
            this.listViewZayavleniya.View = System.Windows.Forms.View.Details;
            this.listViewZayavleniya.DoubleClick += new System.EventHandler(this.listViewZayavleniya_DoubleClick);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(457, 106);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(10, 467);
            this.splitter1.TabIndex = 7;
            this.splitter1.TabStop = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1175, 573);
            this.Controls.Add(this.listViewZayavleniya);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.listViewRecipient);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Main";
            this.Text = "БД Ежемесячных пособий";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem менюToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem операцииToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьФайлPUVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьФайлыПоВыплатеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tb_Surname;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tb_SNILS;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listViewRecipient;
        private System.Windows.Forms.ListView listViewZayavleniya;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cb_HavingFiles;
    }
}

