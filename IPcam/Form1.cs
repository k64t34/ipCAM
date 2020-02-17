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
        //https://docs.microsoft.com/ru-ru/dotnet/framework/data/adonet/filtering-with-dataview-linq-to-dataset
        string fCSVcam = @"\\fs1-oduyu\СПАК\Эксплуатация\IPcam\cam.csv";
        DataSet dsCam = new DataSet("Cam");
        DataTable tCam;// = new DataTable("Cam");
        SqlDataAdapter  sqlCam = new SqlDataAdapter();
        DataView  vCam;
        //public System.Data.OleDb.OleDbConnection Conn;
        public int selectCnt = 0;
        public String FileDB = "cam.csv";
        public String ProviderDB = "System.Data.Odbc";
        public String PathDB;

        public Form1()
        {
            InitializeComponent();
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            PathDB = @"\\fs1-oduyu\СПАК\Эксплуатация\IPcam";
            PathDB = @"D:\Users\Andrew\Documents\Projects\ipCAM\IPcam";
            PathDB = @"\\t90\tmp";
            string PathTMP= System.IO.Path.GetTempPath().ToString();
            PathTMP = PathTMP.Substring(0,PathTMP.Length - 1);
            /*#if DEBUG
                        PathDB = @"\\fs1 -oduyu\СПАК\Эксплуатация\IPcam";
                        PathDB = "c:\\";
            #else
                        PathDB = Path.GetDirectoryName(Application.ExecutablePath);                  
            #endif*/

            System.Data.OleDb.OleDbConnection Conn = new System.Data.OleDb.OleDbConnection();
            Conn.ConnectionString = @"Provider = Microsoft.Jet.OLEDB.4.0;" +
                @"Data source = "+ PathDB + ";" + //@"Data source = \\fs1-oduyu\СПАК\Эксплуатация\IPcam;"+
                @"Extended Properties = ""text;HDR=YES;FMT=Delimited"";";
            try
            {
                Conn.Open();                
                String strSql;
                strSql = @"select id,title from cam.csv";                
                OleDbDataAdapter adapter;
                adapter = new OleDbDataAdapter(strSql, Conn);
                adapter.Fill(dataSet1);
                int iTitle = dataSet1.Tables[0].Columns.IndexOf("title");
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
            
            return;

            dsCam.Tables.Add("Cam");
            tCam = dsCam.Tables["Cam"];
            using (StreamReader sr = new StreamReader(fCSVcam, Encoding.Default))
            {
                // Create table header
                string[] headers = sr.ReadLine().Split(';');
                foreach (string header in headers)
                {
                    tCam.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(';');
                    DataRow dr = tCam.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    tCam.Rows.Add(dr);                    
                    //var query = from tCam in tCam.AsEnumerable()  select title;
                    //EnumerableRowCollection<DataRow> query = from Cam in tCam select title;
                    //EnumerableRowCollection<DataRow> query = from Cam in tCam select title;
                    //vCam = query.AsDataView(); 
                    
                    //dataGridView1.DataSource = vCam;


                }
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
            Close();
        }
    }
}


