using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyLinhKienPC
{
    public class FormDangNhap : Form
    {
        private TextBox txtUser, txtPass;
        private Button btnLogin, btnExit;

        public FormDangNhap()
        {
            this.Text = "Đăng Nhập Hệ Thống";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Label tiêu đề
            Label lbl = new Label { Text = "ĐĂNG NHẬP", Font = new Font("Arial", 16, FontStyle.Bold), Location = new Point(130, 20), AutoSize = true, ForeColor = Color.Red };
            this.Controls.Add(lbl);

            // Ô nhập User
            this.Controls.Add(new Label { Text = "Tài khoản:", Location = new Point(50, 80), AutoSize = true });
            txtUser = new TextBox { Location = new Point(130, 77), Width = 180, Text = "admin" }; // Điền sẵn cho nhanh
            this.Controls.Add(txtUser);

            // Ô nhập Pass
            this.Controls.Add(new Label { Text = "Mật khẩu:", Location = new Point(50, 130), AutoSize = true });
            txtPass = new TextBox { Location = new Point(130, 127), Width = 180, Text = "123", PasswordChar = '*' }; // Điền sẵn
            this.Controls.Add(txtPass);

            // Nút Đăng nhập
            btnLogin = new Button { Text = "Đăng Nhập", Location = new Point(130, 180), Size = new Size(100, 40), BackColor = Color.LightBlue };
            btnLogin.Click += BtnLogin_Click;
            this.Controls.Add(btnLogin);

            // Nút Thoát
            btnExit = new Button { Text = "Thoát", Location = new Point(240, 180), Size = new Size(70, 40) };
            btnExit.Click += (s, e) => { Application.Exit(); };
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            // Kiểm tra mật khẩu cứng (Vì làm nhanh)
            if (txtUser.Text == "admin" && txtPass.Text == "123")
            {
                MessageBox.Show("Đăng nhập thành công! Xin chào Admin.");

                // Ẩn form đăng nhập đi
                this.Hide();

                // Mở Form Menu chính lên
                FormMain f = new FormMain();
                f.ShowDialog();

                // Khi tắt Form Menu thì hiện lại Form Đăng nhập
                this.Show();
                txtPass.Text = ""; // Xóa mật khẩu đi
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
            }
        }
    }
}