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
    public partial class Fnhanvien : Form
    {
        OleDbConnection con = new OleDbConnection();
        OleDbCommand command = new OleDbCommand();
        String chuoi = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\Namba\C_Sharp\Doan\QuanLyQuanCafe.mdb";
        DataTable tblNhanVien = new DataTable();
        OleDbDataAdapter adap = new OleDbDataAdapter();
        private bool isMale;
        public Fnhanvien()
        {
            InitializeComponent();
            LoadPositionComboBox();
        }

        private void Fnhanvien_Load(object sender, EventArgs e)
        {
            con = new OleDbConnection(chuoi);
            if (con.State != ConnectionState.Open)
            {
                con.Open();
                loaddulieu();
            }
        }
        void loaddulieu()
        {
            command = con.CreateCommand();
            command.CommandText = "select * from NHANVIEN";
            adap.SelectCommand = command;
            tblNhanVien.Clear();
            adap.Fill(tblNhanVien);
            dtgvnhanvien.DataSource = tblNhanVien;
        }

        private void LoadPositionComboBox()
        {
            cmbchucvu.Items.Clear();

            cmbchucvu.Items.Add("Quản lý");
            cmbchucvu.Items.Add("Nhân viên");
            cmbchucvu.Items.Add("Bảo vệ");

            cmbchucvu.SelectedIndex = 0;
        }
        private void ResetValues()
        {
            txtmanv.Text = "";
            txttennv.Text = "";
            cbnam.Checked = false;
            dtpk.Value = DateTime.Today;
            cmbchucvu.SelectedIndex = -1;
            txtluong.Text = "";
        }

        private void dtgvnhanvien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtmanv.ReadOnly = true;
            int i = dtgvnhanvien.CurrentRow.Index;
            txtmanv.Text = dtgvnhanvien.Rows[i].Cells[0].Value.ToString();
            txttennv.Text = dtgvnhanvien.Rows[i].Cells[1].Value.ToString();
            cbnam.Checked = dtgvnhanvien.Rows[i].Cells[2].Value.ToString() == "Nam";
            dtpk.Text = dtgvnhanvien.Rows[i].Cells[3].Value.ToString();
            cmbchucvu.Text = dtgvnhanvien.Rows[i].Cells[4].Value.ToString();
            txtluong.Text = dtgvnhanvien.Rows[i].Cells[5].Value.ToString();
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            txtmanv.ReadOnly = false;

            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            btnluu.Enabled = true;
            btnthem.Enabled = false;
            ResetValues();
            txtmanv.Focus();
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtmanv.Text))
            {
                MessageBox.Show("Vui lòng chọn một bàn để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có muốn cập nhật thông tin của bàn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                command.Connection.CreateCommand();
                command.CommandText = "UPDATE NHANVIEN SET HOTEN = @hoten, GIOITINH =@gioitinh, NGAYSINH = @ngaysinh, CHUCVU = @chucvu, LUONG = @luong WHERE MANV = @manv";

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@hoten", txttennv.Text);
                command.Parameters.AddWithValue("@gioitinh", cbnam.Checked ? "Nam" : "Nữ");
                command.Parameters.AddWithValue("@ngaysinh", dtpk.Text);
                command.Parameters.AddWithValue("@chucvu", cmbchucvu.Text);
                command.Parameters.AddWithValue("@luong", txtluong.Text);

                command.Parameters.AddWithValue("@manv", txtmanv.Text);

                command.ExecuteNonQuery();
                loaddulieu();

                MessageBox.Show("Cập nhật thông tin nhân viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnluu_Click(object sender, EventArgs e)
        {
            command.Connection.CreateCommand();
            command.CommandText = "INSERT INTO NHANVIEN(MANV, HOTEN, GIOITINH, NGAYSINH, CHUCVU, LUONG) VALUES(@manv, @hoten, @gioitinh, @ngaysinh, @chucvu, @luong)";

            command.Parameters.AddWithValue("@manv", txtmanv.Text);
            command.Parameters.AddWithValue("@hoten", txttennv.Text);
            command.Parameters.AddWithValue("@gioitinh", cbnam.Checked ? "Nam" : "Nữ");
            command.Parameters.AddWithValue("@ngaysinh", dtpk.Text); 
            command.Parameters.AddWithValue("@chucvu", cmbchucvu.Text);
            command.Parameters.AddWithValue("@luong", txtluong.Text);

            command.ExecuteNonQuery();
            loaddulieu();
            ResetValues();
            btnsua.Enabled = true;
            btnxoa.Enabled = true;
            btnluu.Enabled = true;
            btnthem.Enabled = true;
        }

        private void btndong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtmanv.Text))
            {
                MessageBox.Show("Vui lòng chọn một bàn để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa bàn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                command.Connection.CreateCommand();
                command.CommandText = "DELETE FROM NHANVIEN WHERE MANV = @manv";

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@manv", txtmanv.Text);

                command.ExecuteNonQuery();
                loaddulieu();
                ResetValues();

                MessageBox.Show("Xóa nhân viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txttennv_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
