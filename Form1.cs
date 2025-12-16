using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyLinhKienPC
{
    public partial class Form1 : Form
    {
        // Khai báo
        private DataGridView dgvLinhKien;
        private Button btnThem, btnSua, btnXoa, btnThoat, btnTimKiem, btnHuyTim;
        private TextBox txtMa, txtTen, txtLoai, txtHang, txtGia, txtSoLuong, txtTuKhoaTim;

        private XmlHelper xmlHelper = new XmlHelper();
        private List<LinhKien> danhSachGoc = new List<LinhKien>();
        private List<LinhKien> danhSachHienThi = new List<LinhKien>();

        public Form1()
        {
            VeGiaoDienDep(); // <--- Đổi tên hàm vẽ giao diện
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDuLieuGoc();
        }

        // --- GIỮ NGUYÊN LOGIC CŨ ---
        private void LoadDuLieuGoc()
        {
            danhSachGoc = xmlHelper.DocFile();
            danhSachHienThi = new List<LinhKien>(danhSachGoc);
            HienThiLenBang();
        }

        private void HienThiLenBang()
        {
            if (dgvLinhKien != null)
            {
                dgvLinhKien.DataSource = null;
                dgvLinhKien.DataSource = danhSachHienThi;

                if (dgvLinhKien.Columns.Count > 0)
                {
                    dgvLinhKien.Columns["MaLK"].HeaderText = "Mã";
                    dgvLinhKien.Columns["MaLK"].Width = 80;
                    dgvLinhKien.Columns["TenLK"].HeaderText = "Tên Linh Kiện";
                    dgvLinhKien.Columns["HangSX"].HeaderText = "Hãng";
                    dgvLinhKien.Columns["LoaiLK"].HeaderText = "Loại";
                    dgvLinhKien.Columns["SoLuong"].HeaderText = "SL";
                    dgvLinhKien.Columns["SoLuong"].Width = 50;
                    dgvLinhKien.Columns["DonGia"].HeaderText = "Đơn Giá";
                    dgvLinhKien.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                }
            }
        }

        // CÁC HÀM SỰ KIỆN (Giữ nguyên logic)
        private void BtnThem_Click(object sender, EventArgs e)
        {
            if (txtMa.Text == "" || danhSachGoc.Any(x => x.MaLK == txtMa.Text)) { MessageBox.Show("Mã không hợp lệ!"); return; }
            LinhKien lk = LayDuLieuTuForm();
            if (lk != null) { danhSachGoc.Add(lk); LuuVaCapNhat(); MessageBox.Show("Đã thêm!"); XoaTrang(); }
        }
        private void BtnSua_Click(object sender, EventArgs e)
        {
            var lk = danhSachGoc.FirstOrDefault(x => x.MaLK == txtMa.Text);
            if (lk != null)
            {
                lk.TenLK = txtTen.Text; lk.HangSX = txtHang.Text; lk.LoaiLK = txtLoai.Text;
                try
                {
                    lk.SoLuong = int.Parse(txtSoLuong.Text); lk.DonGia = decimal.Parse(txtGia.Text);
                    LuuVaCapNhat(); MessageBox.Show("Đã sửa!");
                }
                catch { }
            }
        }
        private void BtnXoa_Click(object sender, EventArgs e)
        {
            var lk = danhSachGoc.FirstOrDefault(x => x.MaLK == txtMa.Text);
            if (lk != null && MessageBox.Show("Xóa nhé?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            { danhSachGoc.Remove(lk); LuuVaCapNhat(); XoaTrang(); }
        }
        private void BtnTimKiem_Click(object sender, EventArgs e)
        {
            string k = txtTuKhoaTim.Text.ToLower();
            danhSachHienThi = danhSachGoc.Where(x => x.TenLK.ToLower().Contains(k)).ToList();
            HienThiLenBang();
        }
        private void BtnHuyTim_Click(object sender, EventArgs e)
        {
            txtTuKhoaTim.Text = ""; danhSachHienThi = new List<LinhKien>(danhSachGoc); HienThiLenBang();
        }
        private void DgvLinhKien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < danhSachHienThi.Count)
            {
                LinhKien lk = danhSachHienThi[e.RowIndex];
                txtMa.Text = lk.MaLK; txtTen.Text = lk.TenLK; txtHang.Text = lk.HangSX;
                txtLoai.Text = lk.LoaiLK; txtSoLuong.Text = lk.SoLuong.ToString(); txtGia.Text = lk.DonGia.ToString();
            }
        }
        private void LuuVaCapNhat() { xmlHelper.GhiFile(danhSachGoc); danhSachHienThi = new List<LinhKien>(danhSachGoc); HienThiLenBang(); }
        private LinhKien LayDuLieuTuForm() { try { return new LinhKien { MaLK = txtMa.Text, TenLK = txtTen.Text, HangSX = txtHang.Text, LoaiLK = txtLoai.Text, SoLuong = int.Parse(txtSoLuong.Text), DonGia = decimal.Parse(txtGia.Text) }; } catch { return null; } }
        private void XoaTrang() { txtMa.Text = ""; txtTen.Text = ""; txtHang.Text = ""; txtLoai.Text = ""; txtGia.Text = ""; txtSoLuong.Text = ""; }

        // --- PHẦN GIAO DIỆN ĐẸP (FLAT DESIGN) ---
        private void VeGiaoDienDep()
        {
            this.Text = "Quản Lý Linh Kiện";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240); // Nền xám nhạt hiện đại

            // 1. Header Xanh Đậm
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(44, 62, 80) };
            Label lblTitle = new Label { Text = "QUẢN LÝ KHO LINH KIỆN", Font = new Font("Segoe UI", 18, FontStyle.Bold), AutoSize = true, ForeColor = Color.White, Location = new Point(300, 15) };
            pnlHeader.Controls.Add(lblTitle);
            this.Controls.Add(pnlHeader);

            // 2. Khu vực nhập liệu (Gom nhóm vào Panel trắng)
            Panel pnlInput = new Panel { Location = new Point(20, 80), Size = new Size(840, 140), BackColor = Color.White };
            pnlInput.BorderStyle = BorderStyle.FixedSingle; // Viền mỏng

            txtMa = TaoInput(pnlInput, "Mã LK:", 20, 20);
            txtTen = TaoInput(pnlInput, "Tên Linh Kiện:", 300, 20, 250);
            txtHang = TaoInput(pnlInput, "Hãng SX:", 20, 60);
            txtLoai = TaoInput(pnlInput, "Loại LK:", 300, 60, 250);
            txtGia = TaoInput(pnlInput, "Đơn giá:", 20, 100);
            txtSoLuong = TaoInput(pnlInput, "Số lượng:", 300, 100);

            // Trang trí Panel nhập liệu một chút
            Label lblInputTitle = new Label { Text = "Thông tin chi tiết", Location = new Point(10, -10), BackColor = Color.White, ForeColor = Color.Blue, Font = new Font("Arial", 10, FontStyle.Italic) };
            // pnlInput.Controls.Add(lblInputTitle); // Thêm cái này hơi phức tạp vị trí, bỏ qua cho đơn giản
            this.Controls.Add(pnlInput);

            // 3. Thanh công cụ (Tìm kiếm & Nút bấm)
            int yTool = 240;

            // Tìm kiếm
            Label lblTim = new Label { Text = "Tìm nhanh:", Location = new Point(20, yTool + 5), AutoSize = true };
            this.Controls.Add(lblTim);
            txtTuKhoaTim = new TextBox { Location = new Point(90, yTool), Width = 200, Font = new Font("Segoe UI", 10) };
            this.Controls.Add(txtTuKhoaTim);

            btnTimKiem = TaoNut("🔍 Tìm", 300, yTool - 2, Color.FromArgb(52, 152, 219)); // Xanh dương
            btnTimKiem.Click += BtnTimKiem_Click;
            btnHuyTim = TaoNut("Hủy", 390, yTool - 2, Color.Gray);
            btnHuyTim.Click += BtnHuyTim_Click;

            // Nút chức năng (Căn phải)
            btnThem = TaoNut("➕ Thêm", 500, yTool - 2, Color.FromArgb(39, 174, 96)); // Xanh lá
            btnThem.Click += BtnThem_Click;

            btnSua = TaoNut("✏ Sửa", 610, yTool - 2, Color.FromArgb(243, 156, 18)); // Cam
            btnSua.Click += BtnSua_Click;

            btnXoa = TaoNut("🗑 Xóa", 720, yTool - 2, Color.FromArgb(192, 57, 43)); // Đỏ
            btnXoa.Click += BtnXoa_Click;

            // 4. DataGridView Đẹp (Quan trọng nhất)
            dgvLinhKien = new DataGridView();
            dgvLinhKien.Location = new Point(20, 300);
            dgvLinhKien.Size = new Size(840, 300);
            dgvLinhKien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLinhKien.CellClick += DgvLinhKien_CellClick;

            // Styling cho Grid
            dgvLinhKien.BackgroundColor = Color.White;
            dgvLinhKien.BorderStyle = BorderStyle.None;
            dgvLinhKien.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80); // Header màu tối
            dgvLinhKien.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // Chữ trắng
            dgvLinhKien.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvLinhKien.EnableHeadersVisualStyles = false; // Bắt buộc để nhận màu Header
            dgvLinhKien.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvLinhKien.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219); // Màu khi chọn dòng
            dgvLinhKien.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvLinhKien.RowHeadersVisible = false; // Ẩn cột đầu tiên xấu xí
            dgvLinhKien.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(236, 240, 241); // Dòng chẵn lẻ màu khác nhau

            this.Controls.Add(dgvLinhKien);
        }

        private TextBox TaoInput(Panel p, string nhan, int x, int y, int w = 150)
        {
            Label lbl = new Label { Text = nhan, Location = new Point(x, y + 3), AutoSize = true, Font = new Font("Segoe UI", 10) };
            p.Controls.Add(lbl);
            TextBox txt = new TextBox { Location = new Point(x + 90, y), Width = w, Font = new Font("Segoe UI", 10) };
            p.Controls.Add(txt);
            return txt;
        }

        private Button TaoNut(string text, int x, int y, Color mauNen)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Location = new Point(x, y);
            btn.Size = new Size(100, 35);
            btn.BackColor = mauNen;
            btn.ForeColor = Color.White; // Chữ trắng
            btn.FlatStyle = FlatStyle.Flat; // Bỏ viền nổi 3D cũ kỹ
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.Controls.Add(btn);
            return btn;
        }
    }
}