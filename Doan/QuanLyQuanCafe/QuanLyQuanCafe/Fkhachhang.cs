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
    public partial class Fkhachhang : Form
    {
        OleDbConnection con = new OleDbConnection();
        OleDbCommand command = new OleDbCommand();
        String chuoi = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\Namba\C_Sharp\Doan\QuanLyQuanCafe.mdb";
        DataTable tblKhachHang = new DataTable();
        OleDbDataAdapter adap = new OleDbDataAdapter();
        public Fkhachhang()
        {
            InitializeComponent();
        }

        private void FKHang_Load(object sender, EventArgs e)
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
            command.CommandText = "select * from KHACHHANG";
            adap.SelectCommand = command;
            tblKhachHang.Clear();
            adap.Fill(tblKhachHang);
            dtgvkhachhang.DataSource = tblKhachHang;
        }

        private void dtgvkhachhang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtmakh.ReadOnly = true;
            int i = dtgvkhachhang.CurrentRow.Index;
            txtmakh.Text = dtgvkhachhang.Rows[i].Cells[0].Value.ToString();
            txttenkh.Text = dtgvkhachhang.Rows[i].Cells[1].Value.ToString();
            txtsdt.Text = dtgvkhachhang.Rows[i].Cells[2].Value.ToString();
            txtdiachi.Text = dtgvkhachhang.Rows[i].Cells[3].Value.ToString();
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            txtmakh.ReadOnly = false;

            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            btnluu.Enabled = true;
            btnthem.Enabled = false;
            ResetValues();
            txtmakh.Focus();
        }

        private void ResetValues()
        {
            txtmakh.Text = "";
            txttenkh.Text = "";
            txtsdt.Text = "";
            txtdiachi.Text = "";
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtmakh.Text))
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có muốn cập nhật thông tin của khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                command.Connection.CreateCommand();
                command.CommandText = "update KHACHHANG set HOTENKH = '" + txttenkh.Text + "', SODT = '" + txtsdt.Text + "', DIACHI = '" + txtdiachi.Text + "' where MAKH = '" + txtmakh.Text + "'";
                command.ExecuteNonQuery();
                loaddulieu();

                MessageBox.Show("Cập nhật thông tin khách hàng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnluu_Click(object sender, EventArgs e)
        {
            command.Connection.CreateCommand();
            command.CommandText = "INSERT INTO KHACHHANG(MAKH, HOTENKH, SODT, DIACHI) VALUES(@makh, @tenkh, @sodt, @diachi)";

            command.Parameters.AddWithValue("@makh", txtmakh.Text);
            command.Parameters.AddWithValue("@tenkh", txttenkh.Text);
            command.Parameters.AddWithValue("@sodt", txtsdt.Text);
            command.Parameters.AddWithValue("@diachi", txtdiachi.Text);

            command.ExecuteNonQuery();
            loaddulieu();
            ResetValues();
        }

        private void btndong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtmakh.Text))
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                command.Connection.CreateCommand();
                command.CommandText = "DELETE FROM KHACHHANG WHERE MAKH = @makh";

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@makh", txtmakh.Text);

                command.ExecuteNonQuery();
                loaddulieu();
                ResetValues();

                MessageBox.Show("Xóa bàn thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
