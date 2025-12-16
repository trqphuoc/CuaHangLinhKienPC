using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyLinhKienPC
{
    public class FormMain : Form
    {
        public FormMain()
        {
            this.Text = "HỆ THỐNG QUẢN LÝ LINH KIỆN PC";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            // Tiêu đề
            Label lblTitle = new Label { Text = "MENU CHÍNH", Font = new Font("Arial", 20, FontStyle.Bold), AutoSize = true, Location = new Point(200, 40), ForeColor = Color.Blue };
            this.Controls.Add(lblTitle);

            // Nút 1: Vào Quản lý Kho (Form1 cũ của bạn)
            Button btnKho = TaoNut("QUẢN LÝ KHO", 150, 100, Color.LightBlue);
            btnKho.Click += (s, e) => {
                Form1 f = new Form1();
                f.ShowDialog(); // Mở form kho lên
            };
            this.Controls.Add(btnKho);

            // Nút 2: Vào Bán Hàng (FormBanHang mới làm)
            Button btnBan = TaoNut("LẬP HÓA ĐƠN", 150, 180, Color.LightGreen);
            btnBan.Click += (s, e) => {
                FormBanHang f = new FormBanHang();
                f.ShowDialog(); // Mở form bán hàng lên
            };
            this.Controls.Add(btnBan);

            // Nút 3: Đăng xuất
            Button btnOut = TaoNut("ĐĂNG XUẤT", 150, 260, Color.LightPink);
            btnOut.Click += (s, e) => {
                this.Close(); // Đóng form menu -> Sẽ quay về form đăng nhập
            };
            this.Controls.Add(btnOut);
        }

        private Button TaoNut(string text, int x, int y, Color mau)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Location = new Point(x, y);
            btn.Size = new Size(300, 60);
            btn.Font = new Font("Arial", 12, FontStyle.Bold);
            btn.BackColor = mau;
            return btn;
        }
    }
}