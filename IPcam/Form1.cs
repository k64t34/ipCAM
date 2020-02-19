﻿using System;
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
using IWshRuntimeLibrary;
using Microsoft.Win32;

namespace IPcam
{
    
    public partial class Form1 : Form
    {
        public string PathDB = Path.GetDirectoryName(Application.ExecutablePath);
        public System.Data.OleDb.OleDbConnection Conn = new System.Data.OleDb.OleDbConnection();
        public OleDbDataAdapter adapter;
        public string[] CamStream = new string[3];
        const string vlc_cmd_line = " --no-repeat ";
        const string vlc_exe = "vlc.exe";
        string FolderVLC= @"c:\Program Files\VideoLAN\VLC\";
        public Form1()
        {
            InitializeComponent();
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;
            this.Text = this.Text + " " + version.ToString();

			
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
            listBox_LOG.Items.Add("Поиск в реестре записей об установленном плеере VLC");
            //Read registry to find  path to VLC player
            //Компьютер\HKEY_LOCAL_MACHINE\SOFTWARE\VideoLAN\VLC            
            //https://docs.microsoft.com/ru-ru/dotnet/api/microsoft.win32.registrykey?view=netframework-4.8            
            bool needInstalVLC = false;

            needInstalVLC = true;
            RegistryKey VLCKey = Registry.LocalMachine.OpenSubKey(@"software\videolan\vlc");
            if (VLCKey != null) 
            { 
                RegistryValueKind rvkInstallDir = VLCKey.GetValueKind("InstallDir");
                if (rvkInstallDir == RegistryValueKind.String)
                {
                    string vInstallDir = VLCKey.GetValue("InstallDir").ToString();
                    if (vInstallDir != null)
                    {
                        if (Directory.Exists(vInstallDir))
                        {
                            if (System.IO.File.Exists(vInstallDir + "\\" + vlc_exe))
                            { 
                            FolderVLC = vInstallDir;
                            needInstalVLC = false;
                            }
                        }
                    }
                }
            }
            if (needInstalVLC)//Компьютер\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\VideoLAN\VLC
            {
                VLCKey = Registry.LocalMachine.OpenSubKey(@"software\WOW6432Node\videolan\vlc");
                if (VLCKey != null)
                {
                    RegistryValueKind rvkInstallDir = VLCKey.GetValueKind("InstallDir");
                    if (rvkInstallDir == RegistryValueKind.String)
                    {
                        string vInstallDir = VLCKey.GetValue("InstallDir").ToString();
                        if (vInstallDir != null)
                        {
                            if (Directory.Exists(vInstallDir))
                            {
                                if (System.IO.File.Exists(vInstallDir + "\\" + vlc_exe))
                                {
                                    FolderVLC = vInstallDir;
                                    needInstalVLC = false;
                                }
                            }
                        }
                    }
                }
            }
            ProcessStartInfo ProcessInfo;
            Process Process;
            if (needInstalVLC) {
                listBox_LOG.Items.Add("Установка плеера VLC");//Instal VLC player            
                ProcessInfo = new ProcessStartInfo();
            ProcessInfo.Arguments = " /L=1033 /s";
#if DEBUG
                PathDB = @"\\fs1-oduyu\СПАК\Эксплуатация\IPcam";
#endif

                ProcessInfo.WorkingDirectory = PathDB + "\\distrib\\";
            ProcessInfo.FileName = "vlc-3.0.7.1-win32.exe";
                listBox_LOG.Items.Add("\t"+ ProcessInfo.WorkingDirectory+ ProcessInfo.FileName+" "+ ProcessInfo.Arguments);//Instal VLC player            
                try
                {
                    Process = Process.Start(ProcessInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка установки VLC player\n" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }



            ProcessInfo = new ProcessStartInfo();
            ProcessInfo.Arguments = vlc_cmd_line + CamStream[0].ToString()+" "+ CamStream[1].ToString() + " "+ CamStream[2].ToString();
            ProcessInfo.WorkingDirectory = FolderVLC;
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
            if (checkBox_AddToDesktop.Checked)
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = shell.CreateShortcut(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory)+"\\"+ dataGridView1.CurrentRow.Cells[1].Value.ToString()+".lnk");
                shortcut.Description = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                shortcut.IconLocation = @"C:\Windows\system32\shell32.dll,117";
                shortcut.TargetPath = FolderVLC+"\\vlc.exe";
                shortcut.Arguments = vlc_cmd_line + CamStream[0].ToString() + " " + CamStream[1].ToString() + " " + CamStream[2].ToString();
                shortcut.Save();                
            }
            if (checkBox_AutoStart.Checked)
            {
                
                WshShell shell = new WshShell();
                IWshShortcut shortcut = shell.CreateShortcut(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup) + "\\" + dataGridView1.CurrentRow.Cells[1].Value.ToString() + ".lnk");
                shortcut.Description = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                shortcut.IconLocation = @"C:\Windows\system32\shell32.dll,117";
                shortcut.TargetPath = FolderVLC + "\\vlc.exe";
                shortcut.Arguments = vlc_cmd_line + CamStream[0].ToString() + " " + CamStream[1].ToString() + " " + CamStream[2].ToString();
                shortcut.Save();



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

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}


