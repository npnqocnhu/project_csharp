using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using COMExcel = Microsoft.Office.Interop.Excel;

namespace QuanLyQuanCafe
{
    public partial class Fhoadonban : Form
    {

        OleDbConnection con = new OleDbConnection();
        OleDbCommand command = new OleDbCommand();
        String chuoi = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\Namba\C_Sharp\Doan\QuanLyQuanCafe.mdb";
        DataTable tblDonHang = new DataTable();
        DataTable tblCTDonHang = new DataTable();
        OleDbDataAdapter adap = new OleDbDataAdapter();
        List<KhachHang> listKhachHang = new List<KhachHang>();
        bool isAdd = false;
        //
        bool isFirst = true; 
        public Fhoadonban()
        {
            InitializeComponent();
        }

        private void Fhoadonban_Load(object sender, EventArgs e)
        {
            con = new OleDbConnection(chuoi);
            if (con.State != ConnectionState.Open)
            {
                con.Open();
                loaddulieu();
            }

            FillComboBox("SELECT MAKH, HOTENKH FROM KHACHHANG", cmbmakh,true);
            listKhachHang = SelectListCustomer("SELECT MAKH, HOTENKH, SODT, DIACHI FROM KHACHHANG");
            cmbmakh.SelectedIndex = -1;
            //txtdiachi.Text = listKhachHang[0].DIACHI;
            //txtdienthoai.Text = listKhachHang[0].SODT;
            //MessageBox.Show(listKhachHang[0].MAKH +
            //    "-" + listKhachHang[0].HOTENKH + "-" + listKhachHang[0].SODT + "-" + listKhachHang[0].DIACHI);
            FillComboBox("SELECT MANV, HOTEN FROM NHANVIEN", cmbNhanVien,true);
            cmbNhanVien.SelectedIndex = -1;
            FillComboBox("SELECT MASP, TENSP FROM SANPHAM", cmbmasp,true);
            cmbmasp.SelectedIndex = -1;
            FillComboBox("SELECT MABAN, TENBAN FROM BAN", cmbmaban,true);
            cmbmaban.SelectedIndex = -1;
            FillComboBox("SELECT MADH FROM DONHANG", cmbmahdtk,false);
            isFirst = false;

            //Hiển thị thông tin của một hoá đơn được gọi từ form tìm kiếm
            if (txtmadh.Text != "")
            {
                LoadInfoHoaDon();
                btnhuy.Enabled = true;
                btnin.Enabled = true;
            }
            LoadDataGridView();

        }
        private List<KhachHang> SelectListCustomer(string query)
        {
            OleDbCommand cmd = new OleDbCommand(query, con);
            OleDbDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                KhachHang khachHang = new KhachHang();
                khachHang.MAKH = dr.GetString(0);
                khachHang.HOTENKH = dr.GetString(1);
                khachHang.SODT = dr.GetString(2);
                khachHang.DIACHI = dr.GetString(3);
                listKhachHang.Add(khachHang);
            }

            dr.Close();
            return listKhachHang;
        }
        private void FillComboBox(string query, ComboBox comboBox,bool isKeyValue)
        {
            OleDbCommand cmd = new OleDbCommand(query, con);
            OleDbDataReader dr = cmd.ExecuteReader();
            List<KeyValuePair<string, string>> results = new List<KeyValuePair<string, string>>();

            if (isKeyValue)
            {
                while (dr.Read())
                {

                    string key = dr.GetString(0);
                    string value = dr.GetString(1);
                    results.Add(new KeyValuePair<string, string>(key, value));
                }
                comboBox.DataSource = results;
                comboBox.DisplayMember = "value";
                comboBox.ValueMember = "key";
            }
            else
            {
                while (dr.Read())
                {

                    string key = dr.GetString(0);
                    comboBox.Items.Add(key);
                }
            }
            dr.Close();
        }

        void loaddulieu()
        {
            //command = con.CreateCommand();
            //command.CommandText = "SELECT a.MASP, b.TENSP, a.SOLUONG, b.DONGIA, a.THANHTIEN " +
            //                      "FROM CHITIETDONHANG AS a " +
            //                      "INNER JOIN SANPHAM AS b ON a.MASP = b.MASP";
            //adap.SelectCommand = command;
            //tblCTDonHang.Clear();
            //adap.Fill(tblCTDonHang);
            //dtgvhoadon.DataSource = tblCTDonHang;

            command = con.CreateCommand();
            command.CommandText = "select * from CHITIETDONHANG";
            adap.SelectCommand = command;
            tblCTDonHang.Clear();
            adap.Fill(tblCTDonHang);
            dtgvhoadon.DataSource = tblCTDonHang;
        }

        public static string ChuyenSoSangChu(string sNumber)
        {
            int mLen, mDigit;
            string mTemp = "";
            string[] mNumText;
            //Xóa các dấu "," nếu có
            sNumber = sNumber.Replace(",", "");
            mNumText = "không;một;hai;ba;bốn;năm;sáu;bảy;tám;chín".Split(';');
            mLen = sNumber.Length - 1; // trừ 1 vì thứ tự đi từ 0
            for (int i = 0; i <= mLen; i++)
            {
                mDigit = Convert.ToInt32(sNumber.Substring(i, 1));
                mTemp = mTemp + " " + mNumText[mDigit];
                if (mLen == i) // Chữ số cuối cùng không cần xét tiếp break; 
                    switch ((mLen - i) % 9)
                    {
                        case 0:
                            mTemp = mTemp + " tỷ";
                            if (sNumber.Substring(i + 1, 3) == "000") i = i + 3;
                            if (sNumber.Substring(i + 1, 3) == "000") i = i + 3;
                            if (sNumber.Substring(i + 1, 3) == "000") i = i + 3;
                            break;
                        case 6:
                            mTemp = mTemp + " triệu";
                            if (sNumber.Substring(i + 1, 3) == "000") i = i + 3;
                            if (sNumber.Substring(i + 1, 3) == "000") i = i + 3;
                            break;
                        case 3:
                            mTemp = mTemp + " nghìn";
                            if (sNumber.Substring(i + 1, 3) == "000") i = i + 3;
                            break;
                        default:
                            switch ((mLen - i) % 3)
                            {
                                case 2:
                                    mTemp = mTemp + " trăm";
                                    break;
                                case 1:
                                    mTemp = mTemp + " mươi";
                                    break;
                            }
                            break;
                    }
            }
            //Loại bỏ trường hợp x00
            mTemp = mTemp.Replace("không mươi không ", "");
            mTemp = mTemp.Replace("không mươi không", ""); //Loại bỏ trường hợp 00x 
            mTemp = mTemp.Replace("không mươi ", "linh "); //Loại bỏ trường hợp x0, x>=2
            mTemp = mTemp.Replace("mươi không", "mươi");
            //Fix trường hợp 10
            mTemp = mTemp.Replace("một mươi", "mười");
            //Fix trường hợp x4, x>=2
            mTemp = mTemp.Replace("mươi bốn", "mươi tư");
            //Fix trường hợp x04
            mTemp = mTemp.Replace("linh bốn", "linh tư");
            //Fix trường hợp x5, x>=2
            mTemp = mTemp.Replace("mươi năm", "mươi lăm");
            //Fix trường hợp x1, x>=2
            mTemp = mTemp.Replace("mươi một", "mươi mốt");
            //Fix trường hợp x15
            mTemp = mTemp.Replace("mười năm", "mười lăm");
            //Bỏ ký tự space
            mTemp = mTemp.Trim();
            //Viết hoa ký tự đầu tiên
            mTemp = mTemp.Substring(0, 1).ToUpper() + mTemp.Substring(1) + " đồng";
            return mTemp;
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            isAdd = true;
            btnhuy.Enabled = false;
            btnluu.Enabled = true;
            btnin.Enabled = false;
            btnthem.Enabled = false;
            ResetValues();
            txtmadh.Text = CreateKey("HDB");
            LoadDataGridView();

            //DataGridViewRow row = (DataGridViewRow)dtgvhoadon.Rows[0].Clone();
            //row.Cells[0].Value = "0";
            //row.Cells[1].Value = "0";
            //row.Cells[2].Value = "0";
            //row.Cells[3].Value = 10;
            //row.Cells[4].Value = 10;
            //row.Cells[5].Value = 10;
            //dtgvhoadon.Rows.Add(temp);
        }

        private void ResetValues()
        {
            dtpkngayban.Value = DateTime.Now;
            cmbNhanVien.Text = "";
            cmbmakh.Text = "";
            cmbmaban.Text = "";
            txttongtien.Text = "";
            lblbangchu.Text = "";
            cmbmasp.Text = "";
            nmsoluong.Value = nmsoluong.Minimum;
            txtthanhtien.Text = "0";
        }

        //Tạo mã đơn hàng
        public static string CreateKey(string tiento)
        {
            //string code = tiento;
            //string datePart = DateTime.Now.ToString("yyyyMMdd");
            //code += datePart;
            //string timePart = DateTime.Now.ToString("HHmmss");
            //code += "_" + timePart;

            //return code;

            string key = tiento;
            string[] partsDay;
            partsDay = DateTime.Now.ToShortDateString().Split('/');
            string d = String.Format("{0}{1}{2}", partsDay[0], partsDay[1], partsDay[2]);
            key = key + d;
            string[] partsTime;
            partsTime = DateTime.Now.ToLongTimeString().Split(':');
            if (partsTime[2].Substring(3, 2) == "PM")
                partsTime[0] = ConvertTimeTo24(partsTime[0]);
            if (partsTime[2].Substring(3, 2) == "AM")
                if (partsTime[0].Length == 1)
                    partsTime[0] = "0" + partsTime[0];
            partsTime[2] = partsTime[2].Remove(2, 3);
            string t;
            t = String.Format("_{0}{1}{2}", partsTime[0], partsTime[1], partsTime[2]);
            key = key + t;
            return key;
        }

        public static string ConvertTimeTo24(string hour)
        {
            string h = "";
            switch (hour)
            {
                case "1":
                    h = "13";
                    break;
                case "2":
                    h = "14";
                    break;
                case "3":
                    h = "15";
                    break;
                case "4":
                    h = "16";
                    break;
                case "5":
                    h = "17";
                    break;
                case "6":
                    h = "18";
                    break;
                case "7":
                    h = "19";
                    break;
                case "8":
                    h = "20";
                    break;
                case "9":
                    h = "21";
                    break;
                case "10":
                    h = "22";
                    break;
                case "11":
                    h = "23";
                    break;
                case "12":
                    h = "0";
                    break;
            }
            return h;
        }

        public static bool CheckKey(string sql)
        {
            using (OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\Namba\C_Sharp\Doan\QuanLyQuanCafe.mdb"))
            {
                con.Open();

                // Tạo một OleDbDataAdapter mới với câu truy vấn SQL và kết nối đã cung cấp
                OleDbDataAdapter adap = new OleDbDataAdapter(sql, con);

                // Tạo một DataTable mới để lưu kết quả
                DataTable table = new DataTable();

                // Đổ dữ liệu từ câu truy vấn SQL vào DataTable
                adap.Fill(table);

                // Kiểm tra xem DataTable có các dòng nào không
                return table.Rows.Count > 0;
            }
        }

        private void LoadInfoHoaDon()
        {
            string str;
            str = "SELECT NGAYBAN FROM DONHANG WHERE MADH = N'" + txtmadh.Text + "'";
            dtpkngayban.Text = ConvertDateTime(GetFieldValues(str, chuoi));
            str = "SELECT MANV FROM DONHANG WHERE MADH = N'" + txtmadh.Text + "'";
            cmbNhanVien.Text = GetFieldValues(str, chuoi);
            str = "SELECT MAKH FROM DONHANG WHERE MADH = N'" + txtmadh.Text + "'";
            cmbmakh.Text = GetFieldValues(str, chuoi);
            str = "SELECT TONGTIEN FROM DONHANG WHERE MADH = N'" + txtmadh.Text + "'";
            txttongtien.Text =GetFieldValues(str, chuoi);
            lblbangchu.Text = "Bằng chữ: " +ChuyenSoSangChu(txttongtien.Text);
        }

        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT a.MASP, b.TENSP, a.SOLUONG, b.DONGIA,a.THANHTIEN FROM CHITIETDONHANG AS a, SANPHAM AS b WHERE a.MADH ='" + txtmadh.Text + "' AND a.MASP=b.MASP";
           
            //dtgvhoadon.Columns[0].HeaderText = "Mã sản phẩm";
            //dtgvhoadon.Columns[1].HeaderText = "Tên sản phẩm";
            //dtgvhoadon.Columns[2].HeaderText = "Số lượng";
            //dtgvhoadon.Columns[3].HeaderText = "Đơn giá";
            //dtgvhoadon.Columns[4].HeaderText = "Thành tiền";
            //dtgvhoadon.Columns[0].Width = 80;
            //dtgvhoadon.Columns[1].Width = 130;
            //dtgvhoadon.Columns[2].Width = 80;
            //dtgvhoadon.Columns[3].Width = 90;
            //dtgvhoadon.Columns[4].Width = 90;
            //dtgvhoadon.AllowUserToAddRows = false;
            //dtgvhoadon.EditMode = DataGridViewEditMode.EditProgrammatically;
            tblCTDonHang = GetDataToTable(sql, chuoi);
            dtgvhoadon.DataSource = tblCTDonHang;
        }

        public static DataTable GetDataToTable(string sql, string chuoi)
        {
            OleDbDataAdapter dap = new OleDbDataAdapter(); // Định nghĩa đối tượng thuộc lớp OleDbDataAdapter
                                                           // Tạo đối tượng thuộc lớp OleDbCommand
            dap.SelectCommand = new OleDbCommand();
            dap.SelectCommand.Connection = new OleDbConnection(chuoi); // Kết nối cơ sở dữ liệu
            dap.SelectCommand.CommandText = sql; // Lệnh SQL
                                                 // Khai báo đối tượng table thuộc lớp DataTable
            DataTable table = new DataTable();
            dap.Fill(table);
            return table;
        }


        private void btnluu_Click(object sender, EventArgs e)
        {


            string sql;
            double tongTien, tongMoi;
            sql = "SELECT MADH FROM DONHANG WHERE MADH =N'" + txtmadh.Text + "'";
            if(!CheckKey(sql))
            {
                if (dtpkngayban.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập ngày bán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dtpkngayban.Focus();
                    return;
                }
                if (cmbNhanVien.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbNhanVien.Focus();
                    return;
                }
                if (cmbmakh.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbmakh.Focus();
                    return;
                }

                sql = "INSERT INTO DONHANG(MADH, NGAYBAN, MAKH, MABAN, MANV, TONGTIEN) VALUES (N'" + txtmadh.Text.Trim() + "','" +
                        ConvertDateTime(dtpkngayban.Text.Trim()) + "',N'" + cmbmakh.SelectedValue + "',N'" +
                        cmbmaban.SelectedValue + "',N'" +cmbNhanVien.SelectedValue + "'" + txttongtien.Text + ")";
                RunSQL(sql, chuoi);

                if (cmbmasp.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập mã hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbmasp.Focus();
                    return;
                }
                if ((nmsoluong.Text.Trim().Length == 0) || (nmsoluong.Text == "0"))
                {
                    MessageBox.Show("Bạn phải nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    nmsoluong.Text = "";
                    nmsoluong.Focus();
                    return;
                }
                sql = "SELECT MASP FROM CHITIETDONHANG WHERE MASP=N'" + cmbmasp.SelectedValue + "' AND MADH = N'" + txtmadh.Text.Trim() + "'";
                if (CheckKey(sql))
                {
                    MessageBox.Show("Mã hàng này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbmasp.Focus();
                    return;
                }

                sql = "INSERT INTO CHITIETDONHANG(MADH,MASP,SOLUONG, DONGIA,THANHTIEN) VALUES(N'" + txtmadh.Text.Trim() + "',N'" + cmbmasp.SelectedValue + "'," + nmsoluong.Text + "," + txtdongia.Text + "," + txtthanhtien.Text + ")";
                RunSQL(sql, chuoi);
                LoadDataGridView();

                //Cập nhật lại tổng tiền
                sql = "SELECT TONGTIEN FROM DONHANG WHERE MADH = N'" + txtmadh.Text + "'";
                tongTien = Convert.ToDouble(GetFieldValues(sql, chuoi));
                tongMoi = tongTien + Convert.ToDouble(txtthanhtien.Text);
                sql = "UPDATE DONHANG SET TONGTIEN =" + tongMoi + " WHERE MADH = N'" + txtmadh.Text + "'";
                RunSQL(sql,chuoi);
                txttongtien.Text = tongMoi.ToString();
                lblbangchu.Text = "Bằng chữ: " + ChuyenSoSangChu(tongMoi.ToString());
                btnhuy.Enabled = true;
                btnthem.Enabled = true;
                btnin.Enabled = true;
            }    
        }

        public static void RunSQL(string sql, string connectionString)
        {
            OleDbCommand cmd; // Đối tượng thuộc lớp OleDbCommand
            cmd = new OleDbCommand();
            cmd.Connection = new OleDbConnection(connectionString); // Khởi tạo kết nối

            cmd.CommandText = sql; // Gán lệnh SQL

            try
            {
                cmd.Connection.Open(); // Mở kết nối
                cmd.ExecuteNonQuery(); // Thực hiện câu lệnh SQL
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                cmd.Connection.Close(); // Đóng kết nối
                cmd.Dispose(); // Giải phóng bộ nhớ
                cmd = null;
            }
        }

        public static string ConvertDateTime(string date)
        {
            string[] elements = date.Split('/');
            string dt = string.Format("{0}/{1}/{2}", elements[0], elements[1], elements[2]);
            return dt;
        }

        public static string GetFieldValues(string sql, string chuoi)
        {
            string ma = "";
            using (OleDbConnection connection = new OleDbConnection(chuoi))
            {
                using (OleDbCommand cmd = new OleDbCommand(sql, connection))
                {
                    connection.Open();
                    OleDbDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ma = reader.GetValue(0).ToString();
                    }

                    reader.Close();
                }
            }

            return ma;
        }

        private void cmbmakh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isFirst)
            {

                string maKhachHang = cmbmakh.SelectedValue.ToString();
                string query = $"SELECT DIACHI, SODT FROM KHACHHANG WHERE MAKH = '{maKhachHang}'";

                OleDbCommand cmd = new OleDbCommand(query, con);
                OleDbDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtdiachi.Text = dr["DIACHI"].ToString();
                    txtdienthoai.Text = dr["SODT"].ToString();
                }

                dr.Close();
            }
        }

        private void cmbmasp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isFirst)
            {

                string maSanPham = cmbmasp.SelectedValue.ToString();
                string query = $"SELECT DONGIA FROM SANPHAM WHERE MASP = '{maSanPham}'";

                OleDbCommand cmd = new OleDbCommand(query, con);
                OleDbDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtdongia.Text = dr["DONGIA"].ToString();
                }

                dr.Close();
            }
        }

        private void btndong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void nmsoluong_ValueChanged(object sender, EventArgs e)
        {
            float thanhTien = float.Parse(nmsoluong.Value.ToString()) * float.Parse(txtdongia.Text);
            txtthanhtien.Text = thanhTien.ToString();
        }

        private void dtgvhoadon_DoubleClick(object sender, EventArgs e)
        {
            string maSPxoa, sql;
            Double thanhTienxoa, soLuongxoa, sl, tong, tongMoi;
            if (tblCTDonHang.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if ((MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                //Xóa hàng và cập nhật lại số lượng hàng 
                maSPxoa = dtgvhoadon.CurrentRow.Cells["MASP"].Value.ToString();
                soLuongxoa = Convert.ToDouble(dtgvhoadon.CurrentRow.Cells["SOLUONG"].Value.ToString());
                thanhTienxoa = Convert.ToDouble(dtgvhoadon.CurrentRow.Cells["THANHTIEN"].Value.ToString());
                sql = "DELETE CHITIETDONHANG WHERE MADH=N'" + txtmadh.Text + "' AND MASP = N'" + maSPxoa + "'";
                RunSQL(sql, chuoi);
                // Cập nhật lại tổng tiền cho hóa đơn bán
                sql = "SELECT TONGTIEN FROM DONHANG WHERE MADH = N'" + txtmadh.Text + "'";
                tong = Convert.ToDouble(GetFieldValues(sql, chuoi));
                tongMoi = tong - thanhTienxoa;
                sql = "UPDATE DONHANG SET TONGTIEN =" + tongMoi + " WHERE MAHD = N'" + txtmadh.Text + "'";
                RunSQL(sql, chuoi);
                txttongtien.Text = tongMoi.ToString();
                lblbangchu.Text = "Bằng chữ: " + ChuyenSoSangChu(tongMoi.ToString());
                LoadDataGridView();
            }
        }

        private void btntimkiem_Click(object sender, EventArgs e)
        {
            string sql = string.Format("" +
                "SELECT a.MASP, b.TENSP, a.SOLUONG, b.DONGIA,a.THANHTIEN" +
                " FROM CHITIETDONHANG AS a, SANPHAM AS b " +
                "WHERE a.MADH ='{0}' AND a.MaSP = b.MaSP",cmbmahdtk.SelectedItem);
            OleDbCommand cmd = new OleDbCommand(sql, con);
            OleDbDataReader dr = cmd.ExecuteReader();


                while (dr.Read())
                {
                DataGridViewRow row = new DataGridViewRow();
                row.Cells[0].Value = dr.GetString(0);
                row.Cells[1].Value = dr.GetString(1);
                row.Cells[2].Value = dr.GetString(2);
                row.Cells[3].Value = dr.GetString(3);
                row.Cells[4].Value = dr.GetString(4);
                MessageBox.Show(dr.GetString(0) + "-"
                    + dr.GetString(1) + "-"
                    + dr.GetString(2) + "-"
                    + dr.GetString(3) + "-"
                    + dr.GetString(4));
                dtgvhoadon.Rows.Add(row);
            }
            dr.Close();

            //DataGridViewRow row = (DataGridViewRow)dtgvhoadon.Rows[0].Clone();
            //row.Cells[0].Value = "0";
            //row.Cells[1].Value = "0";
            //row.Cells[2].Value = "0";
            //row.Cells[3].Value = 10;
            //row.Cells[4].Value = 10;
            //row.Cells[5].Value = 10;
            //dtgvhoadon.Rows.Add(temp);
        }
    }
}
