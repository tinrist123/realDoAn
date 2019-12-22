using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mainBackGroundForm
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        
        static void Main()
        {
           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FormDangNhap LoginForm = new FormDangNhap();
            LoginForm.ShowDialog();
            if (LoginForm.checkTrue == true)
            {
                Form1 ImportantForm = new Form1();
                ImportantForm.txtHello.Text += " " + LoginForm.tk;
                Application.Run(ImportantForm);
            }
        }
    }
}
