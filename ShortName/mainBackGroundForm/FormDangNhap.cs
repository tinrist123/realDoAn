using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace mainBackGroundForm
{
    public partial class FormDangNhap : Form
    {
        public string tk = "";
        public bool checkTrue = false;
        public FormDangNhap()
        {
            InitializeComponent();
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //SqlConnection cnn = new SqlConnection(@"Data Source=DESKTOP-GD2DBV7\SQLEXPRESS;Initial Catalog=Login;Integrated Security=True");
            
            try
            {
                SqlConnection cnn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\nhanvien.mdf;Integrated Security=True;Connect Timeout=30");
                cnn.Open();
                tk = txtUser.Text;
                
                string mk = txtPass.Text;
                string sql = "select *from LoginData where Username='"+ tk +"' and Password='"+mk+"'"; 
                SqlCommand cmd = new SqlCommand(sql, cnn);
                SqlDataReader data = cmd.ExecuteReader();
                if (data.Read() == true)
                {
                    MessageBox.Show("Đăng Nhập Thành Công");
                    checkTrue = true;
                    


                }
                else
                {
                    MessageBox.Show("Yor Username and Password are incorrect! ",
                        "DANGER",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
                
            }
            catch
            {
                MessageBox.Show("Lỗi kết nối!!!");
            }
            finally
            {

                SqlConnection cnn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\nhanvien.mdf;Integrated Security=True;Connect Timeout=30");
                cnn.Close();
                this.Close();
            }





        }

        private void btnExit_Click(object sender, EventArgs e)
        {
           DialogResult rs =  MessageBox.Show("Bạn có muốn thoát", 
                "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
