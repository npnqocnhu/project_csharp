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
    public partial class Formlogin : Form
    {
        public static int UserType { get; private set; }
        public Formlogin()
        {
            InitializeComponent();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\Namba\C_Sharp\Doan\QuanLyQuanCafe.mdb";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(1), type FROM TAIKHOAN WHERE user_name=@username AND password=@password GROUP BY type";

                using (OleDbCommand cmd = new OleDbCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@username", txtuser.Text);
                    cmd.Parameters.AddWithValue("@password", txtpass.Text);

                    OleDbDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows && reader.Read())
                    {
                        int count = Convert.ToInt32(reader[0]);
                        UserType = Convert.ToInt32(reader[1]);  // Lưu loại bảng

                        if (count == 1)
                        {
                            MessageBox.Show("Đăng nhập thành công!");
                            this.Hide();
                            Fmenu menuForm = new Fmenu();
                            menuForm.Show();
                        }
                        else
                        {
                            MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
                    }
                }
            }
        }
    }
}
