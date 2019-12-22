using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace mainBackGroundForm
{
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
        }
        string txtPath = Application.StartupPath + @"\textfile.txt";
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
        public void DocTxt()
        {

            
            using (StreamWriter sw = new StreamWriter(txtPath))
            {
                sw.WriteLine(rchtxt.Text);
            }


              
        }
        public void VietTxt()
        {
            // doc va hien thi du lieu trong textfile.txt
            string line = "";
            using (StreamReader sr = new StreamReader(txtPath))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    rchtxt.Text += line;
                }
            }
        }

        private void btnLuuTxt_Click(object sender, EventArgs e)
        {
            DocTxt();
            MessageBox.Show("Lưu Thành Công", "Congratulation", MessageBoxButtons.OK, MessageBoxIcon.Information);
           
        }
    }
}
