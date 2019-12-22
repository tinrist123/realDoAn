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
    public partial class Form1 : Form
    {
        Management manage = new Management();
        Home home = new Home();
        Profile profile = new Profile();
        WorkSchedule workschedule = new WorkSchedule();
        FormDangNhap DangNhap = new FormDangNhap();

        public static Form1 Instance; 

        public Form1()
        {
            Instance = this; 
            InitializeComponent();
        }
        public void MoveIndicator(Control control)
        {
            indicator.Top = control.Top;
            indicator.Height = control.Height;
            
        }
        private void btnHome_Click(object sender, EventArgs e)
        {          
            MoveIndicator(panel5);
            
            //pnlMain.Controls.Add(home);
            home.BringToFront();
            home.lblSll.Text = manage.soLuongNhanVien + "";
        }
        
        public void btnProfile_Click(object sender, EventArgs e)
        {         
            MoveIndicator(panel6);
            
            pnlMain.Controls.Add(profile);

            profile.BringToFront();
            profile.Dock = DockStyle.Fill;         
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            
            MoveIndicator(panel7);
            //pnlMain.Controls.Add(manage);
            manage.BringToFront();
            manage.Dock = DockStyle.Fill;
            Button btn = new Button();
            DataGridViewCellEventArgs z = new DataGridViewCellEventArgs(1,1);

            Timer catch_call_trigger = new Timer();
            catch_call_trigger.Interval = 1;
            catch_call_trigger.Tick += new EventHandler(trigger_cell_click);

            catch_call_trigger.Start(); 
        }

        private void trigger_cell_click(object sender, EventArgs e)
        {
            //manage.Update();
            if (manage.CheckCellClick == true)
            {
                //manage.dataGridView1_CellClick(btn, z);
                MoveIndicator(panel6);
                profile.BringToFront();
                profile = manage.Mprofile;
                
                
                manage.CheckCellClick = false;
                btnProfile_Click(null, null);

                (sender as Timer).Dispose(); 
            }
        }

        private void btnschedule_Click(object sender, EventArgs e)
        {
            
            MoveIndicator(panel8);
            //pnlMain.Controls.Add(workschedule);
            workschedule.Dock = DockStyle.Fill;
            workschedule.BringToFront();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          

            pnlMain.Controls.Add(manage);            
            pnlMain.Controls.Add(home);
            
            pnlMain.Controls.Add(workschedule);
            
            pnlMain.Controls.Add(profile);
   
            
            home.lblSll.Text = manage.soLuongNhanVien + "";
            home.lblTongSoGio.Text = workschedule.TongSoGioLam + "";
            home.VietTxt();
            workschedule.Tinhsogiolam();
            AddSoGioLam();
            
           
            home.BringToFront();
           
        }
        string add;
        void AddSoGioLam()
        {
            try
            { 
            SqlConnection cnn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\nhanvien.mdf;Integrated Security=True;Connect Timeout=30");
            cnn.Open();
                for (int i = 0; i < workschedule.Luuten.Count; i++)
                {
                    add = "update Nhanvien set [Giờ làm] = '" + workschedule.SogiolamNV[workschedule.Luuten[i]] + "' where [Tên làm việc] ='" + workschedule.Luuten[i] + "'";
                    SqlCommand cmdAdd = new SqlCommand(add, cnn);
                    cmdAdd.ExecuteNonQuery();

                    Management.Instance.ketnoicsdl();

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
    }
   
}
