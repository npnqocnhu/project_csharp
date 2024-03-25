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
    public partial class Fthucuong : Form
    {
        OleDbConnection con = new OleDbConnection();
        OleDbCommand command = new OleDbCommand();
        String chuoi = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\Namba\C_Sharp\Doan\QuanLyQuanCafe.mdb";
        DataTable tblThucUong = new DataTable();
        OleDbDataAdapter adap = new OleDbDataAdapter();
        public Fthucuong()
        {
            InitializeComponent();
        }

        private void Fthucuong_Load(object sender, EventArgs e)
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
            command.CommandText = "select * from SANPHAM";
            adap.SelectCommand = command;
            tblThucUong.Clear();
            adap.Fill(tblThucUong);
            dtgvsanpham.DataSource = tblThucUong;
        }

        private void dtgvsanpham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtmasp.ReadOnly = true;
            int i = dtgvsanpham.CurrentRow.Index;
            txtmasp.Text = dtgvsanpham.Rows[i].Cells[0].Value.ToString();
            txttensp.Text = dtgvsanpham.Rows[i].Cells[1].Value.ToString();
            nmgia.Text = dtgvsanpham.Rows[i].Cells[2].Value.ToString();
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            txtmasp.ReadOnly = false;

            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            btnluu.Enabled = true;
            btnthem.Enabled = false;
            ResetValues();
            txtmasp.Focus();
        }

        private void ResetValues()
        {
            txtmasp.Text = "";
            txttensp.Text = "";
            nmgia.Text = "0";
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtmasp.Text))
            {
                MessageBox.Show("Vui lòng chọn một thức uống để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có muốn cập nhật thông tin của thức uống này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                command.Connection.CreateCommand();
                command.CommandText = "update SANPHAM set TENSP = '" + txttensp.Text + "', GIA = '" + nmgia.Text + "' where MASP = '" + txtmasp.Text + "'";
                command.ExecuteNonQuery();
                loaddulieu();

                MessageBox.Show("Cập nhật thông tin thức uống thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnluu_Click(object sender, EventArgs e)
        {
            command.Connection.CreateCommand();
            string sql = string.Format("INSERT INTO SANPHAM (MASP, TENSP, GIA) VALUES ('{0}', '{1}', '{2}')", txtmasp.Text, txttensp.Text, nmgia.Value);

            command.CommandText = sql;
            command.ExecuteNonQuery();
            loaddulieu();
            ResetValues();
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtmasp.Text))
            {
                MessageBox.Show("Vui lòng chọn một thức uống để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa thức uống này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                command.Connection.CreateCommand();
                command.CommandText = "DELETE FROM SANPHAM WHERE MASP = @masp";

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@masp", txtmasp.Text);

                command.ExecuteNonQuery();
                loaddulieu();
                ResetValues();

                MessageBox.Show("Xóa thức uống thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btndong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
