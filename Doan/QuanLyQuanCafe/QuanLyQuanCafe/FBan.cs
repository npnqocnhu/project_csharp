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
    public partial class FBan : Form
    {
        OleDbConnection con = new OleDbConnection();
        OleDbCommand command = new OleDbCommand();
        String chuoi = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\Namba\C_Sharp\Doan\QuanLyQuanCafe.mdb";
        DataTable tblBan = new DataTable();
        OleDbDataAdapter adap = new OleDbDataAdapter();

        public FBan()
        {
            InitializeComponent();
        }
        private void FBan_Load(object sender, EventArgs e)
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
            command.CommandText = "select * from BAN";
            adap.SelectCommand = command;
            tblBan.Clear();
            adap.Fill(tblBan);
            dtgvdsban.DataSource = tblBan;
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            txtmaban.ReadOnly = false;

            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            btnluu.Enabled = true;
            btnthem.Enabled = false;
            ResetValues();
            txtmaban.Enabled = true;
            txtmaban.Focus();
            txttenban.Enabled = true;
        }

        private void ResetValues()
        {
            txtmaban.Text = "";
            txttenban.Text = "";
        }

        private void btnluu_Click(object sender, EventArgs e)
        {
            command.Connection.CreateCommand();
            command.CommandText = "INSERT INTO BAN(MABAN, TENBAN) VALUES(@maban, @tenban)";

            command.Parameters.AddWithValue("@maban", txtmaban.Text);
            command.Parameters.AddWithValue("@tenban", txttenban.Text);

            command.ExecuteNonQuery();
            loaddulieu();
            ResetValues();
        }

        private void dtgvdsban_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtmaban.ReadOnly = true;
            int i = dtgvdsban.CurrentRow.Index;
            txtmaban.Text = dtgvdsban.Rows[i].Cells[0].Value.ToString();
            txttenban.Text = dtgvdsban.Rows[i].Cells[1].Value.ToString();
            
        }

        private void btndong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtmaban.Text))
            {
                MessageBox.Show("Vui lòng chọn một bàn để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có muốn cập nhật thông tin của bàn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                command.Connection.CreateCommand();
                command.CommandText = "UPDATE BAN SET TENBAN = @tenban WHERE MABAN = @maban";

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@tenban", txttenban.Text);
                command.Parameters.AddWithValue("@maban", txtmaban.Text);

                command.ExecuteNonQuery();
                loaddulieu();

                MessageBox.Show("Cập nhật thông tin bàn thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtmaban.Text))
            {
                MessageBox.Show("Vui lòng chọn một bàn để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa bàn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                command.Connection.CreateCommand();
                command.CommandText = "DELETE FROM BAN WHERE MABAN = @maban";

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@maban", txtmaban.Text);

                command.ExecuteNonQuery();
                loaddulieu();
                ResetValues();

                MessageBox.Show("Xóa bàn thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
