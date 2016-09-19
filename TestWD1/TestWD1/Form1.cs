using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestWD1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void UploadBt_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string ImgPath = openFileDialog1.FileName;
                string uploadPath = System.Environment.CurrentDirectory + "/Origin/";
                string savePath = System.Environment.CurrentDirectory + "/Output/";
                try 
                {
                    if (Directory.Exists(uploadPath) == false)  Directory.CreateDirectory(uploadPath);
                    if(Directory.Exists(savePath) == false)     Directory.CreateDirectory(savePath);
                    
                    Bitmap input = new Bitmap(ImgPath);
                    input.Save(uploadPath + "origin.png");
                    input.Save(savePath + "output.png");

                }
                catch( Exception ex )
                {
                    MessageBox.Show (ex.Message);
                }
                originImg.Load(uploadPath + "origin.png");
                procImg.Load(savePath + "output.png");
            }
        }
    }
}

