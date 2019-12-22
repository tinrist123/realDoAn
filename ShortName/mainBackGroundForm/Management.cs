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

namespace mainBackGroundForm
{
    public partial class Management : UserControl
    {
        public Profile Mprofile = new Profile();
        Form n = new Form();
        public bool CheckCellClick = false;
        public int soLuongNhanVien = 0;
        public static Management Instance; 

        public Management()
        {
            Instance = this; 
            InitializeComponent();
            
        }
    
        public void ketnoicsdl()
        {
            try
            {
                dataGridView1.Refresh(); 
                SqlConnection cnn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\nhanvien.mdf;Integrated Security=True;Connect Timeout=30");
                cnn.Open();
                string sql = "select * from Nhanvien ";
                SqlCommand cmd = new SqlCommand(sql, cnn);
                SqlDataAdapter com = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                com.Fill(table);
                dataGridView1.DataSource = table;
                soLuongNhanVien = dataGridView1.RowCount - 1 ;
            }
            catch
            {
                MessageBox.Show("Lỗi kết nối!!!");
            }
            finally
            {
                SqlConnection cnn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\nhanvien.mdf;Integrated Security=True;Connect Timeout=30");
                cnn.Close();
            }
        }

        private void Management_Load(object sender, EventArgs e)
        {
            ketnoicsdl();
        }
        public void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            Mprofile.txtTenLV.Enabled = true;
            Mprofile.txtHoTen.Enabled = true;
            Mprofile.rbNam.Enabled = true;
            Mprofile.rbNu.Enabled = true;
            Mprofile.txtDiachi.Enabled = true;
            Mprofile.txtSĐT.Enabled = true;
            Mprofile.txtHeluong.Enabled = true;
            Mprofile.txtGiolam.Enabled = true;
            Mprofile.txtLuong.Enabled = true;
            Mprofile.dtNgaysinh.Enabled = true;
            Mprofile.btnThem.Enabled = false;
            Mprofile.btnSua.Enabled = true;
            Mprofile.btnXoa.Enabled = true;
            Mprofile.txtGiolam.Visible = true;
            Mprofile.txtHeluong.Visible = true;
            Mprofile.txtLuong.Visible = true;
            Mprofile.btnSua.Text = "Lưu";

            int index = e.RowIndex;
            //dataGridView1.Rows[index].Selected = true;

            Mprofile.txtHoTen.Text = dataGridView1.Rows[index].Cells[0].Value.ToString();
            Mprofile.txtTenLV.Text = dataGridView1.Rows[index].Cells[1].Value.ToString();
            if (dataGridView1.Rows[index].Cells[2].Value.ToString() == "Nam")
                Mprofile.rbNam.Checked = true;
            else Mprofile.rbNu.Checked = true;
            Mprofile.txtDiachi.Text = dataGridView1.Rows[index].Cells[3].Value.ToString();
            Mprofile.txtSĐT.Text = dataGridView1.Rows[index].Cells[4].Value.ToString();
            Mprofile.dtNgaysinh.Text = dataGridView1.Rows[index].Cells[5].Value.ToString();
            Mprofile.txtGiolam.Text = dataGridView1.Rows[index].Cells[6].Value.ToString();
            Mprofile.txtHeluong.Text = dataGridView1.Rows[index].Cells[7].Value.ToString();
            Mprofile.txtLuong.Text = dataGridView1.Rows[index].Cells[8].Value.ToString();
            
            CheckCellClick = true;
            //soLuongNhanVien = dataGridView1.RowCount;
            //abc.MoveIndicator(abc.panel6);
            //abc.pnlMain.Controls.Add(Mprofile);
            //Mprofile.BringToFront();
            //Formattext();

        }
    }
}
