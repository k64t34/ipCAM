using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;


namespace IPcam
{
    
    public partial class Form1 : Form
    {
        public string PathDB = Path.GetDirectoryName(Application.ExecutablePath);
        public System.Data.OleDb.OleDbConnection Conn = new System.Data.OleDb.OleDbConnection();
        public OleDbDataAdapter adapter;
        public Form1()
        {
            InitializeComponent();
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //PathDB = @"\\fs1-oduyu\СПАК\Эксплуатация\IPcam";
            //PathDB = @"D:\Users\Andrew\Documents\Projects\ipCAM\IPcam";
            //PathDB = @"\\t90\tmp";
#if DEBUG
          
            PathDB = System.IO.Directory.GetParent(PathDB).ToString();
            PathDB = System.IO.Directory.GetParent(PathDB).ToString();
            PathDB = System.IO.Directory.GetParent(PathDB).ToString();
            PathDB += @"\DB";
#endif
            
            Conn.ConnectionString = @"Provider = Microsoft.Jet.OLEDB.4.0;" +
                @"Data source = "+ PathDB + ";" + 
                @"Extended Properties = ""text;HDR=YES;FMT=Delimited"";";
            try
            {
                Conn.Open();                
                String strSql;
                strSql = @"select id,title from cam.csv";                
                
                adapter = new OleDbDataAdapter(strSql, Conn);
                adapter.Fill(dataSet1);
                int iTitle = dataSet1.Tables[0].Columns.IndexOf("title");
                //Conver codepage UTF8 to ANSI
                foreach (DataRow row in dataSet1.Tables[0].Rows)
                    {
                    var bytes = Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding(1251), Encoding.Unicode.GetBytes(row[iTitle].ToString()));
                    row[iTitle] = Encoding.UTF8.GetString(bytes);
                }

                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.BackgroundColor = this.BackColor;
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.ColumnHeadersVisible = false;
                dataGridView1.RowHeadersWidthSizeMode =DataGridViewRowHeadersWidthSizeMode.DisableResizing;
                dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dataGridView1.AllowUserToResizeRows = false;
                dataGridView1.BorderStyle = BorderStyle.None;
                dataGridView1.DataSource = dataSet1.Tables[0];                
                dataGridView1.Columns[1].DataPropertyName = "title";
            }
		        catch (Exception ex)
		    {
		        MessageBox.Show("Ошибка подключения к базе данных\n"+ex.Message+
                    "\n  "+ Conn.ConnectionString

                    , "Установка ПО",MessageBoxButtons.OK,MessageBoxIcon.Error);		        
		        Close();
            }
		    finally
		    {
		        Conn.Close();
		    }
                                 
        }

        private void bindingSource1_CurrentChanged_1(object sender, EventArgs e)
        {

        }

        private void camcsvBindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            //TODO: RUN VLC
            Close();
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //label1.Text = dataSet1.Tables[0].Rows[dataSet1.Tables[0].Columns.IndexOf("title")].ToString();
            label1.Text = "as";// Convert.ToString(dataGridView1.CurrentRow.Cells[0].Size);
        }
    }
}


