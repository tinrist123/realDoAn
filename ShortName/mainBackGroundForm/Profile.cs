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
    public partial class Profile : UserControl
    {
        public Profile()
        {
            InitializeComponent();
        }

        private void eraserClick(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            txt.Text = "";
        }

        public void ketnoicsdl()
        {
            try
            {
                SqlConnection cnn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\nhanvien.mdf;Integrated Security=True;Connect Timeout=30");
                cnn.Open();
                string sql = "select * from Nhanvien ";
                SqlCommand cmd = new SqlCommand(sql, cnn);
                SqlDataAdapter com = new SqlDataAdapter(cmd);
                //DataTable table = new DataTable();
                //com.Fill(table);
                //dataGridView1.DataSource = table;
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

       
        string them;
        private void btnThem_Click(object sender, EventArgs e)
        {

            Nhanvien nv = new Nhanvien();

            if (rbNam.Checked) nv.Gioitinh = "Nam";
            else nv.Gioitinh = "Nữ";

            try
            {
                SqlConnection cnn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\nhanvien.mdf;Integrated Security=True;Connect Timeout=30");
                cnn.Open();
                if (btnThem.Text == "Lưu")
                {
                    if (Checkdata() == true)
                    {
                        txtGiolam.Text = "0";
                        txtHeluong.Text = "0";
                        txtLuong.Text = "0";

                        them = "INSERT INTO Nhanvien VALUES(N'" + txtHoTen.Text + "',N'" + txtTenLV.Text + "',N'" + nv.Gioitinh + "',N'" +
                                txtDiachi.Text + "','" + txtSĐT.Text + "','" + dtNgaysinh.Text + "','" + txtHeluong.Text + "','" + txtGiolam.Text + "','" + txtLuong.Text + "' )";
                        SqlCommand cmdthem = new SqlCommand(them, cnn);
                        cmdthem.ExecuteNonQuery();
                        Management.Instance.ketnoicsdl();

                        btnThem.Text = "Thêm";
                        txtTenLV.Clear();
                        txtHoTen.Clear();
                        rbNam.Checked = false;
                        rbNu.Checked = false;
                        txtDiachi.Clear();
                        txtSĐT.Clear();
                        txtHeluong.Clear();
                        txtGiolam.Clear();
                        txtLuong.Clear();

                        txtTenLV.Enabled = false;
                        txtHoTen.Enabled = false;
                        rbNam.Enabled = false;
                        rbNu.Enabled = false;
                        txtDiachi.Enabled = false;
                        txtSĐT.Enabled = false;
                        dtNgaysinh.Enabled = false;
                        txtHeluong.Enabled = false;
                        txtGiolam.Enabled = false;
                        txtLuong.Enabled = false;

                        MessageBox.Show("Lưu thành công!");
                    }
                }
                else
                {
                    btnThem.Text = "Lưu";

                    txtTenLV.Enabled = true;
                    txtHoTen.Enabled = true;
                    rbNam.Enabled = true;
                    rbNu.Enabled = true;
                    txtDiachi.Enabled = true;
                    txtSĐT.Enabled = true;
                    dtNgaysinh.Enabled = true;
                    txtHeluong.Enabled = true;
                    txtGiolam.Enabled = true;
                    txtLuong.Enabled = true;

                    txtTenLV.Clear();
                    txtHoTen.Clear();
                    rbNam.Checked = true;
                    rbNu.Checked = false;
                    txtDiachi.Clear();
                    txtSĐT.Clear();
                    txtHeluong.Clear();
                    txtGiolam.Clear();
                    txtLuong.Clear();
                    txtHoTen.Focus();
                }
            }
            catch
            {
                MessageBox.Show("Lỗi, Không thêm được");
            }
            finally
            {
                SqlConnection cnn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\nhanvien.mdf;Integrated Security=True;Connect Timeout=30");
                cnn.Close();
            }
        }


        string sua;
        private void btnSua_Click(object sender, EventArgs e)
        {

            Nhanvien nv = new Nhanvien();

            if (rbNam.Checked) nv.Gioitinh = "Nam";
            else nv.Gioitinh = "Nữ";
            try
            {
                if (btnSua.Text == "Lưu")
                {
                    Tinhluong();
                    SqlConnection cnn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\nhanvien.mdf;Integrated Security=True;Connect Timeout=30");
                    cnn.Open();
                    sua = "update Nhanvien set [Tên làm việc] ='" + txtTenLV.Text + "',[Giới tính] =N'" + nv.Gioitinh + "',[Địa chỉ] ='" + txtDiachi.Text + "',[SĐT] ='" + txtSĐT.Text + "',[Ngày sinh]='" + dtNgaysinh.Text + "',[Giờ làm]='" + txtGiolam.Text + "',[Hệ lương]='" + txtHeluong.Text + "', [Lương] = '" + txtLuong.Text + "' where ([Họ tên] = N'" + txtHoTen.Text + "' )";
                    SqlCommand cmdSua = new SqlCommand(sua, cnn);
                    cmdSua.ExecuteNonQuery();

                    Management.Instance.ketnoicsdl();

                    btnThem.Text = "Thêm";
                    btnSua.Text = "Sửa";
                    txtTenLV.Clear();
                    txtHoTen.Clear();
                    rbNam.Checked = false;
                    rbNu.Checked = false;
                    txtDiachi.Clear();
                    txtSĐT.Clear();
                    txtHeluong.Clear();
                    txtGiolam.Clear();
                    txtLuong.Clear();

                    txtTenLV.Enabled = false;
                    txtHoTen.Enabled = false;
                    rbNam.Enabled = false;
                    rbNu.Enabled = false;
                    txtDiachi.Enabled = false;
                    txtSĐT.Enabled = false;
                    txtHeluong.Enabled = false;
                    txtGiolam.Enabled = false;
                    txtLuong.Enabled = false;
                    btnThem.Enabled = true;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;

                    MessageBox.Show("Lưu thành công!");
                }
                else
                {
                    btnSua.Text = "Lưu";

                    txtTenLV.Enabled = true;
                    txtHoTen.Enabled = true;
                    rbNam.Enabled = true;
                    rbNu.Enabled = true;
                    txtDiachi.Enabled = true;
                    txtSĐT.Enabled = true;
                    txtHeluong.Enabled = true;
                    txtGiolam.Enabled = true;
                    txtLuong.Enabled = true;
                    
                    
                
                    txtTenLV.Clear();
                    txtHoTen.Clear();
                    rbNam.Checked = false;
                    rbNu.Checked = false;
                    txtDiachi.Clear();
                    txtSĐT.Clear();
                    txtHeluong.Clear();
                    txtGiolam.Clear();
                    txtLuong.Clear();
                    
                }
            }
            catch
            {
                MessageBox.Show("Lỗi, Không thêm được");
            }
            finally
            {
                SqlConnection cnn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\nhanvien.mdf;Integrated Security=True;Connect Timeout=30");
                cnn.Close();
            }
        }

        string xoa;
        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection cnn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\nhanvien.mdf;Integrated Security=True;Connect Timeout=30");
                cnn.Open();
                if (MessageBox.Show("Bạn có muốn xóa hay không?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    xoa = "delete from Nhanvien where [Họ tên] =N'" + txtHoTen.Text + "' ";
                    SqlCommand cmdxoa = new SqlCommand(xoa, cnn);
                    cmdxoa.ExecuteNonQuery();

                    Management.Instance.ketnoicsdl();

                    txtTenLV.Clear();
                    txtHoTen.Clear();
                    rbNam.Checked = false;
                    rbNu.Checked = false;
                    txtDiachi.Clear();
                    txtSĐT.Clear();
                    txtHeluong.Clear();
                    txtGiolam.Clear();
                    txtLuong.Clear();

                    btnThem.Text = "Thêm";
                    txtTenLV.Enabled = false;
                    txtHoTen.Enabled = false;
                    rbNam.Enabled = false;
                    rbNu.Enabled = false;
                    txtDiachi.Enabled = false;
                    txtSĐT.Enabled = false;
                    txtHeluong.Enabled = false;
                    txtGiolam.Enabled = false;
                    txtLuong.Enabled = false;
                    btnThem.Enabled = true;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                }
            }
            catch
            {
                MessageBox.Show("Lỗi! Xóa Không Được!");
            }
            finally
            {
                SqlConnection cnn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\nhanvien.mdf;Integrated Security=True;Connect Timeout=30");
                cnn.Close();
            }
        }
        public void Tinhluong()
        {
            string a = txtGiolam.Text;
            string b = txtHeluong.Text;

            int kq = (Convert.ToInt32(a) * Convert.ToInt32(b));
            txtLuong.Text = kq.ToString();

        }

        private void Profile_Load(object sender, EventArgs e)
        {
            ketnoicsdl();
        }

        public static string VietHoa(string s)
        {
            if (String.IsNullOrEmpty(s))
                return s;

            string result = "";

            //lấy danh sách các từ  

            string[] words = s.Split(' ');

            foreach (string word in words)
            {
                // từ nào là các khoảng trắng thừa thì bỏ  
                if (word.Trim() != "")
                {
                    if (word.Length > 1)
                        result += word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower() + " ";
                    else
                        result += word.ToUpper() + " ";
                }
            }
            return result.Trim();
        }

        private void txtTenLV_TextChanged(object sender, EventArgs e)
        {
            txtHoTen.Text.Trim();
            txtHoTen.Text = VietHoa(txtHoTen.Text);
        }

        private void txtTenLV_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenLV.Text))
            {
                txtTenLV.Focus();
                errorLV.SetError(txtTenLV, "Please enter your user name!");
            }
            else
            {
                errorLV.SetError(txtTenLV, null);
            }
        }
        private void txtHoTen_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtHoTen.Text))
            {
                txtHoTen.Focus();
                errorHoTen.SetError(txtHoTen, "Please enter your full name!");
            }
            else
            {
                errorHoTen.SetError(txtHoTen, null);
            }
        }
        private void txtDiachi_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDiachi.Text))
            {
                txtDiachi.Focus();
                errorDiachi.SetError(txtDiachi, "Please enter your address!");
            }
            else
            {
                errorDiachi.SetError(txtDiachi, null);
            }
        }

        public bool Checkdata()
        {
            Nhanvien sv = new Nhanvien();

            if (rbNam.Checked) sv.Gioitinh = "Nam";
            else if (rbNu.Checked) sv.Gioitinh = "Nữ";

            if (String.IsNullOrEmpty(txtHoTen.Text) || String.IsNullOrEmpty(txtTenLV.Text) || String.IsNullOrEmpty(txtDiachi.Text) || String.IsNullOrEmpty(txtSĐT.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK);
                return false;
            }
            else
                return true;
        }
        
    }
}
