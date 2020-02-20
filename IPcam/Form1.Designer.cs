namespace IPcam
{
    partial class Form1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.checkBox_AddToDesktop = new System.Windows.Forms.CheckBox();
            this.checkBox_AutoStart = new System.Windows.Forms.CheckBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataSet1 = new System.Data.DataSet();
            this.button_OK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox_LOG = new System.Windows.Forms.ListBox();
            this.checkBox_Log = new System.Windows.Forms.CheckBox();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBox_AddToDesktop
            // 
            this.checkBox_AddToDesktop.AutoSize = true;
            this.checkBox_AddToDesktop.Location = new System.Drawing.Point(313, 20);
            this.checkBox_AddToDesktop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBox_AddToDesktop.Name = "checkBox_AddToDesktop";
            this.checkBox_AddToDesktop.Size = new System.Drawing.Size(318, 23);
            this.checkBox_AddToDesktop.TabIndex = 1;
            this.checkBox_AddToDesktop.Text = "Добавить ярлык камеры  на рабочий стол";
            this.checkBox_AddToDesktop.UseVisualStyleBackColor = true;
            // 
            // checkBox_AutoStart
            // 
            this.checkBox_AutoStart.AutoSize = true;
            this.checkBox_AutoStart.Location = new System.Drawing.Point(313, 52);
            this.checkBox_AutoStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBox_AutoStart.Name = "checkBox_AutoStart";
            this.checkBox_AutoStart.Size = new System.Drawing.Size(350, 23);
            this.checkBox_AutoStart.TabIndex = 2;
            this.checkBox_AutoStart.Text = "Автозагрузка отображения камеры при старте ";
            this.checkBox_AutoStart.UseVisualStyleBackColor = true;
            this.checkBox_AutoStart.CheckedChanged += new System.EventHandler(this.checkBox_AutoStart_CheckedChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.Title});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ButtonShadow;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Location = new System.Drawing.Point(18, 21);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(9, 11, 9, 11);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 32;
            this.dataGridView1.Size = new System.Drawing.Size(283, 327);
            this.dataGridView1.TabIndex = 3;
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "NewDataSet";
            // 
            // button_OK
            // 
            this.button_OK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button_OK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_OK.Location = new System.Drawing.Point(18, 506);
            this.button_OK.Margin = new System.Windows.Forms.Padding(20);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(744, 30);
            this.button_OK.TabIndex = 4;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = false;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label1.Font = new System.Drawing.Font("Lucida Console", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(18, 356);
            this.label1.Margin = new System.Windows.Forms.Padding(20);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(10);
            this.label1.Size = new System.Drawing.Size(744, 150);
            this.label1.TabIndex = 5;
            this.label1.Text = "Дополнительная информация";
            // 
            // listBox_LOG
            // 
            this.listBox_LOG.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBox_LOG.FormattingEnabled = true;
            this.listBox_LOG.HorizontalScrollbar = true;
            this.listBox_LOG.ItemHeight = 18;
            this.listBox_LOG.Location = new System.Drawing.Point(313, 110);
            this.listBox_LOG.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.listBox_LOG.Name = "listBox_LOG";
            this.listBox_LOG.Size = new System.Drawing.Size(449, 238);
            this.listBox_LOG.TabIndex = 7;
            this.listBox_LOG.Visible = false;
            // 
            // checkBox_Log
            // 
            this.checkBox_Log.AutoSize = true;
            this.checkBox_Log.Location = new System.Drawing.Point(313, 83);
            this.checkBox_Log.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBox_Log.Name = "checkBox_Log";
            this.checkBox_Log.Size = new System.Drawing.Size(81, 23);
            this.checkBox_Log.TabIndex = 8;
            this.checkBox_Log.Text = "Журнал";
            this.checkBox_Log.UseVisualStyleBackColor = true;
            this.checkBox_Log.CheckedChanged += new System.EventHandler(this.checkBox_Log_CheckedChanged);
            // 
            // id
            // 
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // Title
            // 
            this.Title.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Title.HeaderText = "Title";
            this.Title.Name = "Title";
            this.Title.ReadOnly = true;
            // 
            // Form1
            // 
            this.AcceptButton = this.button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 557);
            this.Controls.Add(this.checkBox_Log);
            this.Controls.Add(this.listBox_LOG);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.checkBox_AutoStart);
            this.Controls.Add(this.checkBox_AddToDesktop);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(18, 21, 18, 21);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IP cam";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_AddToDesktop;
        private System.Windows.Forms.CheckBox checkBox_AutoStart;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Data.DataSet dataSet1;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox_LOG;
        private System.Windows.Forms.CheckBox checkBox_Log;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
    }
}

