using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Procgame
{
    public partial class Form1 : Form
    {
        public string[] filePaths;
        public string procgameExe;

        public Form1()
        {
            InitializeComponent();
        }

        #region Buttons

                    #region Browse for procgame
                    private void button1_Click(object sender, EventArgs e)
                    {
                        FolderBrowserDialog fb = new FolderBrowserDialog();

                        if (fb.ShowDialog() == DialogResult.OK)
                        {
                            textBox1.Text = fb.SelectedPath + "\\";

                        }
                    }

                    #endregion

                    #region Browse for first frame image
                    private void button2_Click(object sender, EventArgs e)
                    {

                        OpenFileDialog fd = new OpenFileDialog();
                        fd.Filter = "Images All Files (*.*)|*.*";
                        fd.FilterIndex = 1;

                        if (fd.ShowDialog() == DialogResult.OK)
                        {
                            textBox2.Text = fd.FileName.ToString();
                            string text = Path.GetFileNameWithoutExtension(fd.ToString());
                            text = Regex.Replace(text, @"(/*[0-9].*)", "");
                            textBox4.Text = text;
                        }
                    }

                    #endregion

                    #region Browse to .dmd destination - Add option to convert to same path
                    private void button3_Click(object sender, EventArgs e)
                    {
                        FolderBrowserDialog fb = new FolderBrowserDialog();

                        if (fb.ShowDialog() == DialogResult.OK)
                        {
                            textBox3.Text = fb.SelectedPath;
                        }
                    }
                    #endregion

                    #region Convert to DMD
                    private void button4_Click(object sender, EventArgs e)
                    {
                        //try
                        //{
                            string destination;                            
                            string source = textBox2.Text;
                            string path = Path.GetDirectoryName(source);
                            string file = Path.GetFileName(source);
                            string ext = Path.GetExtension(source);
                            string replace = @"%0" + numericUpDown4.Value + "d" + ext;
                            string filename = textBox4.Text;
                            string fullFilename = textBox4.Text + @".dmd";
                            procgameExe = textBox1.Text + "\\";

                            file = Regex.Replace(file, @"(/*[0-9].*)", replace);
                           source =  Path.Combine(path, file);

                            if (checkBox1.Checked)
                            {
                                destination = path;
                            }
                            else
                            {
                                destination = textBox3.Text;
                            }

                            ConvertDMD(destination, source, fullFilename);
                           
                        //}
                        //catch (Exception)
                        //{
                            
                        //}
                        
                    }
                    #endregion

        #endregion


                    #region DMD Player
                    
        //Dmd Source Selection
                    private void button7_Click(object sender, EventArgs e)
                    {
                        OpenFileDialog fd = new OpenFileDialog();
                        fd.Filter = "Images All Files (*.dmd)|*.dmd";
                        fd.FilterIndex = 1;

                        if (fd.ShowDialog() == DialogResult.OK)
                        {
                            textBox5.Text = fd.FileName.ToString();
                        }
                    }

                    #endregion


        public void ConvertDMD(string dest, string source, string filename)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WorkingDirectory = @"C:\Python26\Scripts";
            startInfo.FileName = @"procgame.exe";
            startInfo.Arguments = " dmdconvert " + "\"" + source + "\"" + " " + "\"" + dest + "\\" + filename + "\"";
            startInfo.UseShellExecute = false;
            Process.Start(startInfo).WaitForExit();

            if (checkBox2.Checked)
            {
                DMDPlayer(dest + "\\" + filename);
            }
        }

        public void DMDPlayer(string sourceDmd)
        {
            string loop;
            if (checkBox5.Checked)
                loop = " -r";
            else
                loop = "";

            if (checkBox4.Checked)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WorkingDirectory = @"C:\Python26\Scripts";
                startInfo.FileName = @"procgame.exe";
                startInfo.Arguments = " dmdplayer " + "\"" + sourceDmd + "\"" +
                        @" -s " + numericUpDown1.Value + " " + numericUpDown2.Value + @" -f " + numericUpDown3.Value + " " + loop;
                Process.Start(startInfo).WaitForExit();
            }
            else
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WorkingDirectory = @"C:\Python26\Scripts";
                startInfo.FileName = @"procgame.exe";
                startInfo.Arguments = " dmdplayer " + "\"" + sourceDmd + "\"" + loop;
                Process.Start(startInfo).WaitForExit();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                DMDPlayer(textBox5.Text);
            }
            catch (Exception)
            {
                
               
            }
           
        }

        private void button8_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();

            if (fb.ShowDialog() == DialogResult.OK)
            {
                textBox6.Text = fb.SelectedPath;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkBox3.Checked)
                {
                    filePaths = Directory.GetFiles(textBox6.Text, "*.dmd", SearchOption.AllDirectories);
                }
                else
                {
                    filePaths = Directory.GetFiles(textBox6.Text, "*.dmd");
                }


                listView1.Items.Clear();
                foreach (string dmd in filePaths)
                {
                    if (checkBox3.Checked)
                    {

                        listView1.Items.Add(dmd, 1);
                    }
                    else
                    {
                        string dmdNew = Path.GetFileName(dmd);
                        listView1.Items.Add(dmdNew, 1);
                    }
                }
            }
            catch (Exception)
            {
                
               
            }

         
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(listView1.SelectedItems[0].Text);
            if (checkBox3.Checked)
                DMDPlayer(listView1.SelectedItems[0].Text);
            else
                DMDPlayer(textBox6.Text + "\\" + listView1.SelectedItems[0].Text);
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Images All Files (*.exe)|*.exe";
            fd.FilterIndex = 1;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                textBox5.Text = fd.FileName.ToString();
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }
       


    }
}
