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
using System.Diagnostics;

namespace IPcam
{
    
    public partial class Form1 : Form
    {
        public string PathDB = Path.GetDirectoryName(Application.ExecutablePath);
        public System.Data.OleDb.OleDbConnection Conn = new System.Data.OleDb.OleDbConnection();
        public OleDbDataAdapter adapter;
        public string[] CamStream = new string[3];
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
            this.Text = this.Text + " " + PathDB;
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
                dataGridView1.Columns[0].DataPropertyName = "id";                
                dataGridView1.CurrentCellChanged += new System.EventHandler(DataGridView1CurrentCellChanged);
                DataGridView1CurrentCellChanged(dataGridView1,null);




            }
            catch (Exception ex)
		    {
		        MessageBox.Show("Ошибка подключения к базе данных\n"+ex.Message+"\n  "+ ex.Source+"\n"+ Conn.ConnectionString, "Установка ПО",MessageBoxButtons.OK,MessageBoxIcon.Error);		        
		        Close();
            }
		    finally
		    {
		        
		    }
                                 
        }

       

        private void button_OK_Click(object sender, EventArgs e)
        {


            ProcessStartInfo ProcessInfo;
            Process Process;
            ProcessInfo = new ProcessStartInfo();
            ProcessInfo.Arguments = "--no-repeat "+ CamStream[0].ToString()+" "+ CamStream[1].ToString() + " "+ CamStream[2].ToString();
            ProcessInfo.WorkingDirectory = @"c:\Program Files\VideoLAN\VLC\vlc.exe";
            ProcessInfo.FileName = "vlc.exe";            
            try
            {
                Process = Process.Start(ProcessInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка запуска\n" + ex.Message,"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

            }

            Conn.Close();
            Close();
        }


        void DataGridView1CurrentCellChanged(object sender, EventArgs e)
        {         
                    OleDbDataReader rs;
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = Conn;
            cmd.CommandText= @"select * from cam.csv where id=" + dataGridView1.CurrentRow.Cells[0].Value ;

            try
            {
                rs = cmd.ExecuteReader(CommandBehavior.SingleResult);
                rs.Read();
                label1.Text = String.Format("{0}\nid={1}\nIP={2}\nhostname={3}\n",
                        dataGridView1.CurrentRow.Cells[1].Value,
                    dataGridView1.CurrentRow.Cells[0].Value,
                    rs["ip"],
                    rs["hostname"]
                        );
                CamStream[1] = ""; CamStream[2] = "";
                CamStream[0] = rs["protocol"].ToString();
                CamStream[0] = EliminateEnclosingQuotes(CamStream[0], '\'');

                if (CamStream[0].Contains(@"[IP]"))
                {
                    
                    CamStream[0]=CamStream[0].Replace(@"[IP]", rs["ip"].ToString());
                    
                }

                     


                rs.Close();
                cmd.Dispose();
                cmd = new OleDbCommand();
                cmd.Connection = Conn;
                cmd.CommandText = @"select u.hostname,protocol from stream.csv as s left join hub.csv as u on s.idHub=u.id where idCam=" + dataGridView1.CurrentRow.Cells[0].Value;
                rs = cmd.ExecuteReader();
                int i = 0;
                while (rs.Read())
                {
                    CamStream[i + 1] = CamStream[i];
                    CamStream[i] = "http://"+ rs["hostname"].ToString()+":8008/"+rs["protocol"].ToString();
                    i++;
                }
                
                label1.Text += CamStream[0] + "\n" + CamStream[1] + "\n" + CamStream[2];
                rs.Close();
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Ошибка\n" + ex.Message + "\n  " + ex.Source + "\n", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }
        public string EliminateEnclosingQuotes(string str,char Quotes='"')
        {
            str = str.TrimStart(Quotes);
            str = str.TrimEnd(Quotes);
            return str;
        }


    }
}


