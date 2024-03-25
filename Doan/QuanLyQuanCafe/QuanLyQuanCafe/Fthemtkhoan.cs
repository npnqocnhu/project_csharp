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
    public partial class Fthemtkhoan : Form
    {

        OleDbConnection con = new OleDbConnection();
        OleDbCommand command = new OleDbCommand();
        String chuoi = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\Namba\C_Sharp\Doan\QuanLyQuanCafe.mdb";
        DataTable tblTaiKhoan = new DataTable();
        OleDbDataAdapter adap = new OleDbDataAdapter();
        public Fthemtkhoan()
        {
            InitializeComponent();
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            if (txttentaikhoan.Text.Trim() == "" || txtmatkhau.Text.Trim() == "" || cmbloaitk.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (OleDbConnection con = new OleDbConnection(chuoi))
            {
                con.Open();

                string query = "INSERT INTO TAIKHOAN (user_name, [password], [type]) VALUES (@UserName, @Password, @Type)";

                using (OleDbCommand command = new OleDbCommand(query, con))
                {
                    command.Parameters.AddWithValue("@UserName", txttentaikhoan.Text);
                    command.Parameters.AddWithValue("@Password", txtmatkhau.Text);
                    command.Parameters.AddWithValue("@Type", cmbloaitk.SelectedValue);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Đã thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                loaddulieu();
            }
        }

        private void Fthemtkhoan_Load(object sender, EventArgs e)
        {
            con = new OleDbConnection(chuoi);
            if (con.State != ConnectionState.Open)
            {
                con.Open();
                loaddulieu();
            }

            List<object> userTypeList = new List<object>
            {
                new { Text = "Quản trị viên", Value = 1 },
                new { Text = "Nhân viên", Value = 2 }
            };

            cmbloaitk.DataSource = userTypeList;
            cmbloaitk.DisplayMember = "Text";
            cmbloaitk.ValueMember = "Value";

            cmbloaitk.SelectedIndex = -1; // Chọn giá trị mặc định
            cmbloaitk.Text = "--Chọn loại tài khoản--";

            dtgvtaikhoan.CellFormatting += new DataGridViewCellFormattingEventHandler(dtgvtaikhoan_CellFormatting); // Thêm sự kiện CellFormatting
            dtgvtaikhoan.CellDoubleClick -= new DataGridViewCellEventHandler(dtgvtaikhoan_CellDoubleClick);
            dtgvtaikhoan.CellDoubleClick += new DataGridViewCellEventHandler(dtgvtaikhoan_CellDoubleClick);
        }

        void loaddulieu()
        {
            command = con.CreateCommand();
            command.CommandText = "select user_id, user_name, password, type from TAIKHOAN";
            adap.SelectCommand = command;
            tblTaiKhoan.Clear();
            adap.Fill(tblTaiKhoan);
            dtgvtaikhoan.DataSource = tblTaiKhoan;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dtgvtaikhoan_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dtgvtaikhoan.Columns[e.ColumnIndex].Name == "type" && e.Value != null)
            {
                if (e.Value.ToString() == "1")
                {
                    e.Value = "Quản trị viên";
                }
                else if (e.Value.ToString() == "2")
                {
                    e.Value = "Nhân viên";
                }
            }
        }

        private void dtgvtaikhoan_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa tài khoản này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string userid = dtgvtaikhoan.Rows[e.RowIndex].Cells["user_id"].Value.ToString();

                    using (OleDbConnection con = new OleDbConnection(chuoi)) // Sử dụng using để đảm bảo kết nối được đóng đúng cách
                    {
                        con.Open();

                        using (OleDbCommand deleteCommand = new OleDbCommand("DELETE FROM TAIKHOAN WHERE user_id = @Userid", con))
                        {
                            deleteCommand.Parameters.AddWithValue("@Userid", userid);
                            deleteCommand.ExecuteNonQuery();
                        }

                        con.Close();
                    }

                    loaddulieu(); 
                }
            }
        }
    }
}
