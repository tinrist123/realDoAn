using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using GemBox.Spreadsheet;
using GemBox.Spreadsheet.WinFormsUtilities;


namespace mainBackGroundForm
{
    public partial class WorkSchedule : UserControl
    {
        public WorkSchedule()
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            InitializeComponent();
        }
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Demo Read Google SpeadSheet";
        Dictionary<string, int> DataNV = new Dictionary<string, int>();

        //Hàm check fileWrite Empty
        string fileRead = Application.StartupPath + @"\Book3.xlsx";
        string fileWrite = Application.StartupPath + @"\Book2.xlsx";
        public bool checkfileIsEmpty()
        {
            Excel.Application x1App = new Excel.Application();
            Excel.Workbook x1workbook = x1App.Workbooks.Open(fileWrite);
            Excel._Worksheet x1Worksheet = x1workbook.Sheets[1];
            Excel.Range x1Range = x1Worksheet.UsedRange;

            if (x1Worksheet.Cells[1,2].Value == null)
            {
               
                return true;
            }


            GC.Collect();
            GC.WaitForPendingFinalizers();
            Marshal.ReleaseComObject(x1Range);
            Marshal.ReleaseComObject(x1Worksheet);
            x1workbook.Close();
            Marshal.ReleaseComObject(x1workbook);
            x1App.Quit();
            Marshal.ReleaseComObject(x1App);
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            String spreadsheetId = "12OLgTc_9gMbiPh2H0LsQuZHCi7heKRViZiZMJf8G0kk";
            String range = "A1:H19";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            // Prints the names and majors of students in a sample spreadsheet:

            // https://docs.google.com/spreadsheets/d/12OLgTc_9gMbiPh2H0LsQuZHCi7heKRViZiZMJf8G0kk/edit#gid=0
            // Chèn Dữ liệu vào Listview
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            Excel.Application x1App = new Excel.Application();
            Excel.Workbook x1workbook = x1App.Workbooks.Open(fileRead);
            Excel._Worksheet x1Worksheet = x1workbook.Sheets[1];
            Excel.Range x1Range = x1Worksheet.UsedRange;

            for (int i = 1; i <= 8; i++)// xoa du lieu excel cu
            {
                for (int k = 1; k <= 30; k++)
                {
                    x1Worksheet.Cells[k, i].Value = null;
                }
            }

            if (values != null && values.Count > 0)
            {
                ListViewItem item = new ListViewItem();
                int ViTrirow = 1;
                foreach (var row in values)
                {
                    for (int index = 0; index < row.Count; index++)
                    {
                        if (row[index] == null)
                        {
                            row[index] = "";
                        }
                    }// Fill Null, ( Null will error)

                    if (row.Count == 0)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            x1Worksheet.Cells[ViTrirow, i + 2] = "";
                        }
                    }
                    else
                    {
                        if (row.Count < 8)
                        {
                            for (int i = row.Count; i < 8; i++)
                            {
                                row.Add(""); // Đồng bộ hóa length
                            }
                        }
                        for (int i = 1; i <= 8; i++)
                        {
                            x1Worksheet.Cells[ViTrirow, i] = row[i - 1].ToString();
                        }

                    }
                    ViTrirow++;

                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Marshal.ReleaseComObject(x1Range);
                Marshal.ReleaseComObject(x1Worksheet);
                x1workbook.Close();
                Marshal.ReleaseComObject(x1workbook);
                x1App.Quit();
                Marshal.ReleaseComObject(x1App);


            }

            else
            {
                Console.WriteLine("No data found.");
            }
            ReadExcel(fileRead);
            Arrange();
            WriteExcel(fileWrite, fileRead);
            Arrange1();
            fillTheRest(fileWrite, fileRead);// Xử Lý Ca Còn Thiếu

           // FillListView();

        } // Lấy dữ liệu bỏ vô excel

         
        public Dictionary<string, int> dupicateDataNV = new Dictionary<string, int>();
         List<KeyValuePair<string, int>> arrList1 = new List<KeyValuePair<string, int>>();
        //static Dictionary<string, int> ordered = new Dictionary<string, int>();
         List<KeyValuePair<string, int>> arrList = new List<KeyValuePair<string, int>>();
         Dictionary<string, int> NhanVienCaDu = new Dictionary<string, int>();
         List<KeyValuePair<string, int>> arrList2 = new List<KeyValuePair<string, int>>();

         Dictionary<string, int> Demo = new Dictionary<string, int>();
         int TongSoCa;

         int[,] matrixMaxWork = new int[3, 7]; // tất cả = 0
        public int TongSoGioLam = 63*5;
        public void CountHours()
        {
            for (int buoi = 0; buoi < 3; buoi++)
            {
                for (int thu = 0; thu < 7; thu++)
                {
                    if (buoi == 2)
                    {
                        if (matrixMaxWork[buoi, thu] < 5)
                        {
                            TongSoGioLam = TongSoGioLam -  (5 - matrixMaxWork[buoi,thu]) * 5;
                        }
                    }
                    else
                    {
                        if (matrixMaxWork[buoi, thu] < 2)
                        {
                            TongSoGioLam = TongSoGioLam - (2 - matrixMaxWork[buoi, thu]) * 5;
                        }
                    }
                }
            }
        }
         void Test()
        {

            // Tạo bản sao để lấy Duplication DataNV nhưng Value nhân giá trị %
            foreach (KeyValuePair<string, int> item in DataNV)
            {
                string TenDangXet = item.Key;
                int ValueNow = item.Value * 63 / TongSoCa;
                dupicateDataNV.Add(TenDangXet, ValueNow);
            }
        }
         void Arrange()
        {  // Sắp xếp tăng dần theo value
            var ordered = dupicateDataNV.OrderBy(x => x.Value);
            arrList = ordered.ToList(); // chuyển sang List cho dễ làm
        }
         void Arrange1()
        {  // Sắp xếp tăng dần theo value
            var ordered = dupicateDataNV.OrderByDescending(x => x.Value);
            arrList1 = ordered.ToList(); // chuyển sang List cho dễ làm
        }
         void Arrange2()
        {  // Sắp xếp tăng dần theo value
            var ordered = NhanVienCaDu.OrderByDescending(x => x.Value);
            arrList2 = ordered.ToList(); // chuyển sang List cho dễ làm
        }

        // Xử Lý Ca Còn Thiếu
        private  void fillTheRest(string fileKQ, string fileRead)
        {
            // File Excel Đọc
            Excel.Application x1App = new Excel.Application();
            Excel.Workbook x1workbook = x1App.Workbooks.Open(fileRead);
            Excel._Worksheet x1Worksheet = x1workbook.Sheets[1];
            Excel.Range x1Range = x1Worksheet.UsedRange;


            // File Excel Ghi
            Excel.Application x1App1 = new Excel.Application();
            Excel.Workbook x1workbook1 = x1App1.Workbooks.Open(fileKQ);
            Excel._Worksheet x1Worksheet1 = x1workbook1.Sheets[1];
            Excel.Range x1Range1 = x1Worksheet1.UsedRange;


            // Lấy Các Bạn Nhân viên đủ ca rồi
            for (int index = 0; index < arrList1.Count; index++)
            {
                if (arrList1[index].Value == 0)
                {
                    NhanVienCaDu.Add(arrList1[index].Key, arrList1[index].Value); // Lấy nhân viên đã xếp đủ ca ở Loop đầu tiên
                }
            }

            for (int buoi = 0; buoi < 3; buoi++) // Duyệt Theo Dòng  buoi = buổi
            {

                int soCaMotBuoi;
                if (buoi == 2) soCaMotBuoi = 5; // Gán số Ca Để So Sánh Số Ca Thiếu
                else soCaMotBuoi = 2;
                for (int thu = 0; thu < 7; thu++) // Duyệt Theo Cột thu = thứ
                {

                    if (matrixMaxWork[buoi, thu] < soCaMotBuoi)
                    {
                        string TenDangXet;
                        for (int i = 0; i < arrList1.Count; i++)
                        {
                            if (arrList1[i].Value != 0)
                            {
                                int addRange = 0;
                                bool checkAddTen = true;
                                TenDangXet = arrList1[i].Key; // Lấy Tên Đứa Thiếu Ca Tỷ lệ
                                if (buoi == 0) addRange = 2;
                                else if (buoi == 1) addRange = 7;
                                else addRange = 12;
                                for (int dong = addRange; dong < soCaMotBuoi + addRange; dong++)
                                {
                                    if (TenDangXet == (string)x1Worksheet1.Cells[dong, thu + 2].Value)
                                    {
                                        checkAddTen = false;
                                        break; // Trong Ca Đã có người này rồi
                                    }
                                }
                                if (checkAddTen == true)
                                {
                                    for (int row = addRange; row < soCaMotBuoi + addRange; row++)
                                    { // Tìm bên File Read có tên ứng cử viên ưu tiên 1 không
                                        if (TenDangXet == (string)x1Worksheet.Cells[row, thu + 2].Value)
                                        {// Có nè
                                            NhanVienCaDu[TenDangXet]++;
                                            x1Worksheet1.Cells[addRange + matrixMaxWork[buoi, thu], thu + 2] = TenDangXet;
                                            matrixMaxWork[buoi, thu]++;
                                            //NhanVienCaDu[TenDangXet]++;

                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }
            //Ưu Tiên 2 :
            string TenDangXet1;

            for (int i = 0; i < arrList.Count; i++)
            {
                TenDangXet1 = arrList[i].Key;
                for (int buoi = 0; buoi < 3; buoi++) // Duyệt Theo Dòng  buoi = buổi
                {
                    int soCaMotBuoi1;
                    if (buoi == 2) soCaMotBuoi1 = 5; // Gán số Ca Để So Sánh Số Ca Thiếu
                    else soCaMotBuoi1 = 2;
                    for (int thu = 0; thu < 7; thu++) // Duyệt Theo Cột thu = thứ
                    {
                        int addRange = 0;
                        bool checkAddTen = true;
                        if (buoi == 0) addRange = 2;
                        else if (buoi == 1) addRange = 7;
                        else addRange = 12;
                        if (matrixMaxWork[buoi, thu] < soCaMotBuoi1) // Còn thiếu ca
                        {
                            for (int dong = addRange; dong < soCaMotBuoi1 + addRange; dong++)
                            {
                                if (TenDangXet1 == (string)x1Worksheet1.Cells[dong, thu + 2].Value)
                                {
                                    checkAddTen = false;
                                    break; // Trong Ca Đã có người này rồi
                                }
                            }
                            if (checkAddTen == true)
                            {
                                for (int row = addRange; row < soCaMotBuoi1 + addRange; row++)
                                { // Tìm bên File Read có tên ứng cử viên ưu tiên 1 không
                                    if (TenDangXet1 == (string)x1Worksheet.Cells[row, thu + 2].Value)
                                    {// Có nè
                                        NhanVienCaDu[TenDangXet1]++;
                                        x1Worksheet1.Cells[addRange + matrixMaxWork[buoi, thu], thu + 2] = TenDangXet1;
                                        matrixMaxWork[buoi, thu]++;
                                        //NhanVienCaDu[TenDangXet1]++;

                                    }//truong hop 2 nguoi bang nhau(them ) co so ca ti le bang nhau
                                }
                            }
                        }
                    }
                }
            }
            Arrange2();
            //Ưu tiên 3
            Demo = arrList1.ToDictionary(k => k.Key, k => k.Value);
            for (int j = 0; j < arrList2.Count; j++)
            {

                string NVduca = arrList2[j].Key;
                for (int i = 0; i < arrList1.Count; i++)//Lấy tên nhân viên bị thiếu ca
                {
                    //foreach (KeyValuePair<string, int> item1 in Demo)
                    //{
                    string NVthieuca = arrList1[i].Key;
                    if (Demo[NVthieuca] > 0) // Nếu nv đó còn thiếu ca thì làm
                    {

                        double TiLe = (double)Demo[NVthieuca] / DataNV[NVthieuca];
                        if (TiLe >= 0.25) // Đứa(TenDangXet) này đang dư ca
                        {
                            for (int buoi = 0; buoi < 3; buoi++) // Duyệt Theo Dòng  buoi = buổi
                            {
                                int soCaMotBuoi;
                                if (buoi == 2) soCaMotBuoi = 5; // Gán số Ca Để So Sánh Số Ca Thiếu
                                else soCaMotBuoi = 2;

                                for (int thu = 0; thu < 7; thu++) // Duyệt Theo Cột thu = thứ
                                {
                                    int addRange = 0;
                                    bool checkTen = true;
                                    if (buoi == 0) addRange = 2;
                                    else if (buoi == 1) addRange = 7;
                                    else addRange = 12;
                                    int VTDong = 0;
                                    bool checkXayra = false;
                                    bool checkThem = false;
                                    for (int dong = addRange; dong < addRange + soCaMotBuoi; dong++)
                                    {
                                        if ((NVduca == (string)x1Worksheet1.Cells[dong, thu + 2].Value))
                                        {
                                            VTDong = dong;
                                            checkXayra = true;
                                            for (int dongfake = addRange; dongfake < addRange + soCaMotBuoi; dongfake++)
                                            {
                                                if (NVthieuca == (string)x1Worksheet1.Cells[dongfake, thu + 2].Value)
                                                {
                                                    checkTen = false;
                                                    break;//Out ra khỏi Loop dongfake
                                                }

                                            }
                                        }
                                        if (checkTen == false)
                                        {
                                            break;//Out ra khỏi Loop dong
                                        }
                                        if ((checkTen == true) && (checkXayra == true))
                                        {
                                            int soCaMotBuoiread;
                                            if (buoi == 2) soCaMotBuoiread = 8; // Gán số Ca Để So Sánh Số Ca Thiếu
                                            else soCaMotBuoiread = 4;
                                            for (int dongread = addRange; dongread < addRange + soCaMotBuoiread; dongread++)
                                            {
                                                if (NVthieuca == (string)x1Worksheet.Cells[dongread, thu + 2].Value)
                                                {
                                                    Demo[NVthieuca]--;// Giảm số ca còn thiếu

                                                    x1Worksheet1.Cells[VTDong, thu + 2].Value = NVthieuca;
                                                    checkThem = true;
                                                    break;
                                                }
                                            }
                                            if (checkThem == true)
                                            {
                                                break;
                                            }
                                        }


                                    }
                                    if (checkTen == false || checkThem == true)
                                    {
                                        break;//Out ra khỏi Loop thứ
                                    }
                                }
                            }
                        }
                    }

                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            Marshal.ReleaseComObject(x1Range);
            Marshal.ReleaseComObject(x1Worksheet);
            x1workbook.Close();
            Marshal.ReleaseComObject(x1workbook);
            x1App.Quit();
            Marshal.ReleaseComObject(x1App);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            Marshal.ReleaseComObject(x1Range1);
            Marshal.ReleaseComObject(x1Worksheet1);
            x1workbook1.Close();
            Marshal.ReleaseComObject(x1workbook1);
            x1App1.Quit();
            Marshal.ReleaseComObject(x1App1);
            CountHours();
        }
         public string[] ReadExcel(string file)
        {
            Microsoft.Office.Interop.Excel.Application x1App = new Excel.Application();
            Excel.Workbook x1workbook = x1App.Workbooks.Open(file);
            Excel._Worksheet x1Worksheet = x1workbook.Sheets[1];
            Excel.Range x1Range = x1Worksheet.UsedRange;

            int rowCount = x1Range.Rows.Count;
            string[] mssv = new string[rowCount];

            for (int cot = 2; cot <= 8; cot++)
            {
                for (int dong = 2; dong <= 20; dong++)
                {
                    string TenDangXet = (string)x1Worksheet.Cells[dong, cot].Value;
                    if (TenDangXet == null) continue;
                    if (DataNV.ContainsKey(TenDangXet) == false)// Tên này chưa xuất hiện
                    {
                        DataNV.Add(TenDangXet, 1);
                        TongSoCa++;
                    }
                    else if (DataNV.ContainsKey(TenDangXet) == true) //Tên Này Đã xuất hiện
                    {
                        // Tăng Value --- Tăng số ca
                        DataNV[TenDangXet]++; // Lấy Số lần ĐKCa
                        TongSoCa++;
                    }

                } // export dữ liệu
            }
            Test();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            Marshal.ReleaseComObject(x1Range);
            Marshal.ReleaseComObject(x1Worksheet);
            x1workbook.Close();
            Marshal.ReleaseComObject(x1workbook);
            x1App.Quit();
            Marshal.ReleaseComObject(x1App);
            return mssv;
        }


         public void WriteExcel(string fileKQ, string fileRead)
        {
            // File Excel Đọc
            Excel.Application x1App = new Excel.Application();
            Excel.Workbook x1workbook = x1App.Workbooks.Open(fileRead);
            Excel._Worksheet x1Worksheet = x1workbook.Sheets[1];
            Excel.Range x1Range = x1Worksheet.UsedRange;


            // File Excel Ghi
            Excel.Application x1App1 = new Excel.Application();
            Excel.Workbook x1workbook1 = x1App1.Workbooks.Open(fileKQ);
            Excel._Worksheet x1Worksheet1 = x1workbook1.Sheets[1];
            Excel.Range x1Range1 = x1Worksheet1.UsedRange;

            for (int i = 1; i <= 8; i++)// xoa du lieu excel cu
            {
                for (int k = 1; k <= 16; k++)
                {
                    x1Worksheet1.Cells[k, i].Value = null;
                }
            }

            for (int i = 1; i <=8; i++)
            {
                x1Worksheet1.Cells[1, i] = x1Worksheet.Cells[1, i];
            }
            x1Worksheet1.Cells[2, 1] = x1Worksheet.Cells[2, 1];
            x1Worksheet1.Cells[7, 1] = x1Worksheet.Cells[7, 1];
            x1Worksheet1.Cells[12, 1] = x1Worksheet.Cells[12, 1];
            int j = x1Range.Rows.Count + 1;
            // Viết Kết Qủa
            for (int i = 0; i < arrList.Count; i++)
            {
                string TenDangXet = arrList[i].Key;
                int SoCaTiLe = arrList[i].Value;
                int demBool = 0;
                int Buoi = 1;
                //int Songuoilamngay = 2;//Số ca sáng + chiều
                //int Songuoilamtoi = 5;//Số ca tối
                //int Demsonguoilam = 0;//Đếm số người làm cho mỗi ca
                bool CheckOutTenDangXet = false; // Out Loop với Tên Đang Xét
               
                for (int cot = 2; cot <= 8; cot++)//1
                {

                    if (Buoi == 1 && CheckOutTenDangXet == false)
                    {

                        for (int dong = 2; dong <= 6; dong++)//2
                        {

                            if (TenDangXet == (string)x1Worksheet.Cells[dong, cot].Value)
                            {
                                if (matrixMaxWork[0, cot - 2] >= 2)// 0 tương ứng với buổi sáng và cot - 2 là thứ 2 
                                                                   // cột ứng với excel lớn hơn 2 so với matrix
                                {
                                    break;
                                }
                                matrixMaxWork[0, cot - 2]++;
                                if (matrixMaxWork[0, cot - 2] == 1) // Chèn và format luôn theo số ca đã đăng kí vì nó liên quan với nhau
                                {
                                    x1Worksheet1.Cells[2, cot].Value = TenDangXet;
                                    dupicateDataNV[TenDangXet]--;
                                    demBool++;
                                }
                                else if (matrixMaxWork[0, cot - 2] == 2) // Chèn và format luôn theo số ca đã đăng kí vì nó liên quan với nhau
                                {
                                    x1Worksheet1.Cells[3, cot].Value = TenDangXet;
                                    dupicateDataNV[TenDangXet]--;
                                    demBool++;
                                }

                                if (demBool >= SoCaTiLe) // Out Tên Đang Xét vì Số Ca Đã quá
                                {
                                    CheckOutTenDangXet = true;
                                    break;
                                }
                                Buoi = 2;//Nhảy xuống buổi tiếp theo khi phát hiện có tên đang xét trong ca
                                break;//Thoát ra khỏi vòng for 2
                            }


                        }
                        Buoi = 2; // THoát khỏi vòng lặp for 1
                    }
                    if (Buoi == 2 && CheckOutTenDangXet == false) // Dấu hiệu Out hàm For Lớn

                    {
                        //Demsonguoilam = 0;
                        for (int dong = 7; dong <= 11; dong++)
                        {

                            if (TenDangXet == (string)x1Worksheet.Cells[dong, cot].Value)
                            {
                                if (matrixMaxWork[1, cot - 2] >= 2)// 0 tương ứng với buổi sáng và cot - 2 là thứ 2 
                                                                   // cột ứng với excel lớn hơn 2 so với matrix
                                {
                                    break;
                                }
                                matrixMaxWork[1, cot - 2]++;
                                if (matrixMaxWork[1, cot - 2] == 1) // Chèn và format luôn theo số ca đã đăng kí vì nó liên quan với nhau
                                {
                                    x1Worksheet1.Cells[7, cot].Value = TenDangXet;
                                    dupicateDataNV[TenDangXet]--;
                                    demBool++;
                                }
                                else if (matrixMaxWork[1, cot - 2] == 2) // Chèn và format luôn theo số ca đã đăng kí vì nó liên quan với nhau
                                {
                                    x1Worksheet1.Cells[8, cot].Value = TenDangXet;
                                    dupicateDataNV[TenDangXet]--;
                                    demBool++;
                                }

                                if (demBool >= SoCaTiLe) // Out Tên Đang Xét vì Số Ca Đã quá
                                {
                                    CheckOutTenDangXet = true;
                                    break;
                                }
                            }


                        }
                        Buoi = 3;
                    }
                    if (Buoi == 3 && CheckOutTenDangXet == false)
                    {
                        //Demsonguoilam = 0;
                        for (int dong = 12; dong <= 20; dong++)
                        {

                            if (TenDangXet == (string)x1Worksheet.Cells[dong, cot].Value)
                            {
                                if (matrixMaxWork[2, cot - 2] >= 5)// 0 tương ứng với buổi sáng và cot - 2 là thứ 2 
                                                                   // cột ứng với excel lớn hơn 2 so với matrix
                                {
                                    break;
                                }
                                matrixMaxWork[2, cot - 2]++; // 11 tương ứng trong excel    
                                x1Worksheet1.Cells[11 + matrixMaxWork[2, cot - 2], cot].Value = TenDangXet; // Thuật toán dự phòng nếu Nhân Viên trong ca nhiều quá
                                dupicateDataNV[TenDangXet]--;                                                // Chèn và format luôn theo số ca đã đăng kí vì nó liên quan với nhau
                                demBool++;

                                if (demBool >= SoCaTiLe) // Out Tên Đang Xét vì Số Ca Đã quá
                                {
                                    CheckOutTenDangXet = true;
                                    break;
                                }
                            }


                        }
                    }
                    if (CheckOutTenDangXet == true)
                    {
                        break;
                    }
                    Buoi = 1;
                }

            }

            

            GC.Collect();
            GC.WaitForPendingFinalizers();
            Marshal.ReleaseComObject(x1Range);
            Marshal.ReleaseComObject(x1Worksheet);
            x1workbook.Close();
            Marshal.ReleaseComObject(x1workbook);
            x1App.Quit();
            Marshal.ReleaseComObject(x1App);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            Marshal.ReleaseComObject(x1Range1);
            Marshal.ReleaseComObject(x1Worksheet1);
            x1workbook1.Close();
            Marshal.ReleaseComObject(x1workbook1);
            x1App1.Quit();
            Marshal.ReleaseComObject(x1App1);
            
        }

        public void btnhint_Click(object sender, EventArgs e)
        {
            //var openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "XLS files (*.xls, *.xlt)|*.xls;*.xlt|XLSX files (*.xlsx, *.xlsm, *.xltx, *.xltm)|*.xlsx;*.xlsm;*.xltx;*.xltm|ODS files (*.ods, *.ots)|*.ods;*.ots|CSV files (*.csv, *.tsv)|*.csv;*.tsv|HTML files (*.html, *.htm)|*.html;*.htm";
            //openFileDialog.FilterIndex = 2;

            //  if (openFileDialog.ShowDialog() == DialogResult.OK)
            //{
            
                var workbook = ExcelFile.Load(fileWrite);

                // From ExcelFile to DataGridView.
                DataGridViewConverter.ExportToDataGridView(workbook.Worksheets.ActiveWorksheet, this.datagrid, new ExportToDataGridViewOptions() { ColumnHeaders = true });
            //}
            //string fileRead = "D:\\Doan\\Book2.xlsx";
            //Excel.Application x1App = new Excel.Application();
            //Excel.Workbook x1workbook = x1App.Workbooks.Open(fileRead);
            //Excel._Worksheet x1Worksheet = x1workbook.Sheets[1];
            //Excel.Range x1Range = x1Worksheet.UsedRange;

            //ListViewItem item = new ListViewItem();
            //for (int buoi = 1; buoi <= 16; buoi++)
            //{
            //    for (int thu = 1; thu <= 7; thu++)
            //    {
            //        if (x1Worksheet.Cells[buoi, thu].Value == null)
            //        {
            //            x1Worksheet.Cells[buoi, thu].Value = "";
            //        }
            //    }
            //}
            //for (int buoi = 2; buoi <= 2; buoi++)
            //{
            //    string s1 = x1Worksheet.Cells[buoi, 1].Value.ToString();
            //    string s2 = x1Worksheet.Cells[buoi, 2].Value.ToString();
            //    //string s3 = x1Worksheet.Cells[buoi, 3].Value.ToString();
            //    //string s4 = x1Worksheet.Cells[buoi, 4].Value.ToString();
            //    //string s5 = x1Worksheet.Cells[buoi, 5].Value.ToString();
            //    //string s6 = x1Worksheet.Cells[buoi, 6].Value.ToString();
            //    //string s7 = x1Worksheet.Cells[buoi, 7].Value.ToString();
            //    //string s8 = x1Worksheet.Cells[buoi, 8].Value.ToString();
            //    item = new ListViewItem(new string[] { x1Worksheet.Cells[buoi,1].Value.ToString(),
            //        x1Worksheet.Cells[buoi,2].Value.ToString()});
            //    //    x1Worksheet.Cells[buoi,3].Value.ToString()
            //    //    , x1Worksheet.Cells[buoi,4].Value.ToString()
            //    //    , x1Worksheet.Cells[buoi,5].Value.ToString(),
            //    //    x1Worksheet.Cells[buoi,6].Value.ToString(),
            //    //    x1Worksheet.Cells[buoi,7].Value.ToString(),
            //    //x1Worksheet.Cells[buoi,8].Value.ToString()});
            //    lst_data.Items.AddRange(new ListViewItem[] { item });
            //}

            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //Marshal.ReleaseComObject(x1Range);
            //Marshal.ReleaseComObject(x1Worksheet);
            //x1workbook.Close();
            //Marshal.ReleaseComObject(x1workbook);
            //x1App.Quit();
            //Marshal.ReleaseComObject(x1App);
        }

        public Dictionary<string, int> SogiolamNV = new Dictionary<string, int>();
        public List<string> Luuten  = new List<string>();
       

        public void Tinhsogiolam()
        {

            for (int buoi = 0; buoi < 2; buoi++)
            {
                for (int thu = 1; thu < 8; thu++)   
                {
                    if (datagrid.Rows[buoi].Cells[thu].Value == null) continue;
                    else
                    {
                        string TenDangXet = datagrid.Rows[buoi].Cells[thu].Value.ToString();
                        if (SogiolamNV.ContainsKey(TenDangXet) == false)
                        {
                            SogiolamNV.Add(TenDangXet, 5);
                            Luuten.Add(TenDangXet);
                        }
                        else if (SogiolamNV.ContainsKey(TenDangXet) == true)
                        {
                            SogiolamNV[TenDangXet] += 5;
                        }
                    }
                }
            }
            for (int buoi = 5; buoi < 7; buoi++)
            {
                for (int thu = 1; thu < 8; thu++)
                {
                    if (datagrid.Rows[buoi].Cells[thu].Value == null) continue;
                    else
                    {
                        string TenDangXet = datagrid.Rows[buoi].Cells[thu].Value.ToString();
                        if (SogiolamNV.ContainsKey(TenDangXet) == false)
                        {
                            SogiolamNV.Add(TenDangXet, 5);
                        }
                        else if (SogiolamNV.ContainsKey(TenDangXet) == true)
                        {
                            SogiolamNV[TenDangXet] += 5;
                        }
                    }
                }
            }
            for (int buoi = 10; buoi < 15; buoi++)
            {
                for (int thu = 1; thu < 8; thu++)
                {
                    if (datagrid.Rows[buoi].Cells[thu].Value == null) continue;
                    else
                    {
                        string TenDangXet = datagrid.Rows[buoi].Cells[thu].Value.ToString();
                        if (SogiolamNV.ContainsKey(TenDangXet) == false)
                        {
                            SogiolamNV.Add(TenDangXet, 5);
                        }
                        else if (SogiolamNV.ContainsKey(TenDangXet) == true)
                        {
                            SogiolamNV[TenDangXet] += 5;
                        }
                    }
                }
            }
        }
        
        private void WorkSchedule_Load(object sender, EventArgs e)
        {
            btnhint_Click(null, null);
            btnHienTHi.BringToFront();
            btnSapXep.BringToFront();
            
        }
    }
    
}
