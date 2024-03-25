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

namespace QuanLyQuanCafe
{
    public partial class Fmenu : Form
    {
        OleDbConnection con = new OleDbConnection();
        OleDbCommand command = new OleDbCommand();
        String chuoi = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\Namba\C_Sharp\Doan\QuanLyQuanCafe.mdb";
        DataTable tblCTDonHang = new DataTable();
        OleDbDataAdapter adap = new OleDbDataAdapter();
        List<KhachHang> listKhachHang = new List<KhachHang>();

        //
        bool isFirst = true;

        public Fmenu()
        {
            InitializeComponent();
            CheckUserType();
        }

        private void CheckUserType()
        {
            if (Formlogin.UserType == 1)
            {
                mnuttk.Visible = true;  // Hiển thị mnuttk nếu UserType = 1
            }
            else
            {
                mnuttk.Visible = false;  // Ẩn mnuttk nếu UserType khác 1
            }
        }

        private void mnutkhoadon_Click(object sender, EventArgs e)
        {
            Ftkhoadon frmTkHoaDon = new Ftkhoadon();
            frmTkHoaDon.ShowDialog();
        }

        private void mnuthucuong_Click_1(object sender, EventArgs e)
        {
            Fthucuong frmThucUong = new Fthucuong();
            frmThucUong.ShowDialog();
        }

        private void mnukhachhang_Click_1(object sender, EventArgs e)
        {
            Fkhachhang frmKhachHang = new Fkhachhang();
            frmKhachHang.ShowDialog();
        }

        private void mnunhanvien_Click_1(object sender, EventArgs e)
        {
            Fnhanvien frmNhanVien = new Fnhanvien();
            frmNhanVien.ShowDialog();
        }

        private void mnuban_Click_1(object sender, EventArgs e)
        {
            FBan frmBan = new FBan();
            frmBan.ShowDialog();
        }

        private void mnut_Click_1(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void mnuhoadonban_Click_1(object sender, EventArgs e)
        {
            Fhoadonban frmHoaDon = new Fhoadonban();
            frmHoaDon.ShowDialog();
        }

        private void mnuttk_Click(object sender, EventArgs e)
        {
            Ftkhoadon frmTkHoaDon = new Ftkhoadon();
            frmTkHoaDon.ShowDialog();
        }

        public void SetMnuThongTinKhoan(bool isVisible)
        {
            mnuttk.Visible = isVisible;
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {

        }

        private void Fmenu_Load(object sender, EventArgs e)
        {
            con = new OleDbConnection(chuoi);
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            LoadTables();

            lsvthanhtoan.Columns.Clear();
            lsvthanhtoan.Columns.Add("Tên Món", 150);
            lsvthanhtoan.Columns.Add("Giá", 100);
            lsvthanhtoan.Columns.Add("Số Lượng", 80);
            lsvthanhtoan.Columns.Add("Thành Tiền", 100);

            FillComboBox("SELECT MASP, TENSP FROM SANPHAM", cmbchonmon);
            cmbchonmon.Text = "--Chọn món--"; 

            FillComboBox("SELECT MABAN, TENBAN FROM BAN", cmbban);
            cmbban.Text = "--Chọn bàn--";
            txtgia.Text = "--Giá--";

            isFirst = false;

        }

        private void FillComboBox(string query, ComboBox comboBox)
        {
            OleDbCommand cmd = new OleDbCommand(query, con);
            OleDbDataReader dr = cmd.ExecuteReader();
            List<KeyValuePair<string, string>> results = new List<KeyValuePair<string, string>>();

            while (dr.Read())
            {
                string key = dr.GetString(0);
                string value = dr.GetString(1);
                results.Add(new KeyValuePair<string, string>(key, value));
            }
            comboBox.DataSource = results;
            comboBox.DisplayMember = "value";
            comboBox.ValueMember = "key";
            dr.Close();
        }


        private void LoadTables()
        {

            string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\Namba\C_Sharp\Doan\QuanLyQuanCafe.mdb";

            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();
                string sqlQuery = "SELECT MABAN, TENBAN FROM BAN";

                OleDbCommand cmd = new OleDbCommand(sqlQuery, con);
                OleDbDataReader reader = cmd.ExecuteReader();

                flptable1.Controls.Clear();

                while (reader.Read())
                {
                    string maBan = reader["MABAN"].ToString();
                    string tenBan = reader["TENBAN"].ToString();

                    Button btn = new Button();
                    btn.Text = tenBan;
                    btn.Name = maBan;  
                    btn.Width = 100;  
                    btn.Height = 100;  
                    btn.Margin = new Padding(10);  // Đặt margin
                    btn.TextAlign = ContentAlignment.MiddleCenter;  // Canh giữa nội dung
                    btn.FlatStyle = FlatStyle.Flat;  // Loại bỏ viền
                    btn.BackColor = Color.LightGray;  // Đặt màu nền

                    // Thêm sự kiện click cho button
                    btn.Click += (sender, e) =>
                    {
                        // Xử lý khi click vào button (ví dụ: mở form mới cho bàn đó)
                        MessageBox.Show($"Bạn đã chọn bàn: {btn.Text}");
                    };

                    flptable1.Controls.Add(btn);  // Thêm button vào FlowLayoutPanel
                }

                reader.Close();
            }
        }

        private void cmbchonmon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isFirst)
            {

                string maSanPham = cmbchonmon.SelectedValue.ToString();
                string query = $"SELECT DONGIA FROM SANPHAM WHERE MASP = '{maSanPham}'";

                OleDbCommand cmd = new OleDbCommand(query, con);
                OleDbDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtgia.Text = dr["DONGIA"].ToString();
                }

                dr.Close();
            }
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
                Formlogin frmLogin = new Formlogin();
                frmLogin.ShowDialog();
            }
        }

        
    }
}
