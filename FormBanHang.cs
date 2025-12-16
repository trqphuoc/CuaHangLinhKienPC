using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Xsl;
using System.Xml;

namespace QuanLyLinhKienPC
{
    public partial class FormBanHang : Form
    {
        private DataGridView dgvLinhKien;
        private TextBox txtTenKhach, txtMaHD, txtTenSP, txtDonGia, txtSoLuong, txtThanhTien;
        private Button btnLuuHoaDon, btnInHoaDon;

        private XmlHelper xmlHelper = new XmlHelper();
        private List<LinhKien> dsLinhKien = new List<LinhKien>();
        public string NhanVienHienTai = "Admin";

        public FormBanHang()
        {
            VeGiaoDienPOS(); // Giao diện kiểu máy tính tiền (POS)
            this.Load += FormBanHang_Load;
        }

        // --- GIỮ NGUYÊN LOGIC CŨ ---
        private void FormBanHang_Load(object sender, EventArgs e) { TaoMaHDMoi(); LoadKhoHang(); }
        private void TaoMaHDMoi() { txtMaHD.Text = "HD" + new Random().Next(100, 999); }
        private void LoadKhoHang()
        {
            dsLinhKien = xmlHelper.DocFile();
            dgvLinhKien.DataSource = dsLinhKien;
            if (dgvLinhKien.Columns.Count > 0)
            {
                dgvLinhKien.Columns["MaLK"].Width = 60; dgvLinhKien.Columns["TenLK"].Width = 150;
                dgvLinhKien.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            }
        }
        private void DgvLinhKien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvLinhKien.Rows[e.RowIndex];
                txtTenSP.Text = row.Cells["TenLK"].Value.ToString();
                txtDonGia.Text = row.Cells["DonGia"].Value.ToString();
                txtSoLuong.Text = "1"; TinhTongTien();
            }
        }
        private void TinhTongTien()
        {
            try { txtThanhTien.Text = (decimal.Parse(txtDonGia.Text) * int.Parse(txtSoLuong.Text)).ToString("N0"); }
            catch { txtThanhTien.Text = "0"; }
        }
        private void BtnLuuHoaDon_Click(object sender, EventArgs e)
        {
            if (txtTenKhach.Text == "" || txtTenSP.Text == "") { MessageBox.Show("Thiếu thông tin!"); return; }
            HoaDon hd = new HoaDon { MaHD = txtMaHD.Text, NgayLap = DateTime.Now.ToString("dd/MM/yyyy"), MaNV = NhanVienHienTai, TenKhach = txtTenKhach.Text, TenSP = txtTenSP.Text };
            try { hd.SoLuong = int.Parse(txtSoLuong.Text); hd.ThanhTien = decimal.Parse(txtThanhTien.Text.Replace(",", "").Replace(".", "")); } catch { return; }
            var list = xmlHelper.DocFileHoaDon(); list.Add(hd); xmlHelper.GhiFileHoaDon(list);
            MessageBox.Show("Đã thanh toán!"); btnInHoaDon.Enabled = true;
        }
        private void BtnInHoaDon_Click(object sender, EventArgs e)
        {
            string fileTam = "HoaDonTam.xml";
            using (XmlWriter writer = XmlWriter.Create(fileTam))
            {
                writer.WriteStartElement("HoaDon");
                writer.WriteElementString("MaHD", txtMaHD.Text);
                writer.WriteElementString("NgayLap", DateTime.Now.ToString("dd/MM/yyyy"));
                writer.WriteElementString("MaNV", NhanVienHienTai);
                writer.WriteElementString("TenKhach", txtTenKhach.Text);
                writer.WriteElementString("TenSP", txtTenSP.Text);
                writer.WriteElementString("SoLuong", txtSoLuong.Text);
                writer.WriteElementString("ThanhTien", txtThanhTien.Text);
                writer.WriteEndElement();
            }
            try
            {
                XslCompiledTransform xslt = new XslCompiledTransform();
                xslt.Load("MauHoaDon.xslt");
                xslt.Transform(fileTam, "PhieuThanhToan.html");
                System.Diagnostics.Process.Start("PhieuThanhToan.html");
                TaoMaHDMoi(); txtTenSP.Text = ""; txtThanhTien.Text = ""; btnInHoaDon.Enabled = false;
            }
            catch (Exception ex) { MessageBox.Show("Lỗi in: " + ex.Message); }
        }

        // --- GIAO DIỆN POS (ĐẸP) ---
        private void VeGiaoDienPOS()
        {
            this.Text = "Quản Lý Bán Hàng";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(236, 240, 241); // Nền xám nhạt toàn cục

            // Header
            Panel pnlHead = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(41, 128, 185) }; // Xanh dương đậm
            pnlHead.Controls.Add(new Label { Text = "HỆ THỐNG BÁN HÀNG & THANH TOÁN", ForeColor = Color.White, Font = new Font("Segoe UI", 16, FontStyle.Bold), AutoSize = true, Location = new Point(300, 10) });
            this.Controls.Add(pnlHead);

            // BÊN TRÁI: DANH SÁCH SẢN PHẨM (Chiếm 60%)
            GroupBox grpKho = new GroupBox { Text = "Danh sách linh kiện", Location = new Point(10, 60), Size = new Size(550, 480), Font = new Font("Segoe UI", 10) };

            dgvLinhKien = new DataGridView();
            dgvLinhKien.Dock = DockStyle.Fill;
            dgvLinhKien.BackgroundColor = Color.White;
            dgvLinhKien.BorderStyle = BorderStyle.None;
            dgvLinhKien.ReadOnly = true;
            dgvLinhKien.CellClick += DgvLinhKien_CellClick;
            // Style Grid
            dgvLinhKien.EnableHeadersVisualStyles = false;
            dgvLinhKien.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            dgvLinhKien.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLinhKien.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 204, 113); // Màu xanh lá khi chọn

            grpKho.Controls.Add(dgvLinhKien);
            this.Controls.Add(grpKho);

            // BÊN PHẢI: HÓA ĐƠN (Chiếm 40%) -> Làm màu trắng như tờ giấy
            Panel pnlBill = new Panel { Location = new Point(570, 70), Size = new Size(400, 470), BackColor = Color.White };
            pnlBill.BorderStyle = BorderStyle.FixedSingle;

            Label lblBillTitle = new Label { Text = "THÔNG TIN HÓA ĐƠN", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.FromArgb(192, 57, 43), Location = new Point(90, 10), AutoSize = true };
            pnlBill.Controls.Add(lblBillTitle);

            // Các ô nhập liệu trong hóa đơn
            int yB = 60;
            txtMaHD = TaoInputBill(pnlBill, "Mã HĐ:", yB); txtMaHD.ReadOnly = true;
            txtTenKhach = TaoInputBill(pnlBill, "Khách hàng:", yB + 40);

            // Kẻ đường line
            Label line = new Label { Text = "", BorderStyle = BorderStyle.Fixed3D, Location = new Point(20, yB + 80), Size = new Size(360, 2), Height = 2 };
            pnlBill.Controls.Add(line);

            txtTenSP = TaoInputBill(pnlBill, "Sản phẩm:", yB + 100); txtTenSP.ReadOnly = true;
            txtDonGia = TaoInputBill(pnlBill, "Đơn giá:", yB + 140); txtDonGia.ReadOnly = true;

            txtSoLuong = TaoInputBill(pnlBill, "Số lượng:", yB + 180);
            txtSoLuong.TextChanged += (s, e) => { TinhTongTien(); };

            // Tổng tiền to đùng
            Label lblTong = new Label { Text = "TỔNG THANH TOÁN", Location = new Point(20, yB + 240), Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            pnlBill.Controls.Add(lblTong);

            txtThanhTien = new TextBox { Location = new Point(20, yB + 265), Width = 360, Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.Red, TextAlign = HorizontalAlignment.Right, BackColor = Color.White, ReadOnly = true };
            txtThanhTien.Text = "0";
            pnlBill.Controls.Add(txtThanhTien);

            // Nút bấm to
            btnLuuHoaDon = new Button { Text = "💰 THANH TOÁN", Location = new Point(20, 350), Size = new Size(360, 50), BackColor = Color.FromArgb(39, 174, 96), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            btnLuuHoaDon.Click += BtnLuuHoaDon_Click;
            pnlBill.Controls.Add(btnLuuHoaDon);

            btnInHoaDon = new Button { Text = "🖨 IN HÓA ĐƠN", Location = new Point(20, 410), Size = new Size(360, 40), BackColor = Color.FromArgb(230, 126, 34), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold), Enabled = false };
            btnInHoaDon.Click += BtnInHoaDon_Click;
            pnlBill.Controls.Add(btnInHoaDon);

            this.Controls.Add(pnlBill);
        }

        private TextBox TaoInputBill(Panel p, string nhan, int y)
        {
            Label lbl = new Label { Text = nhan, Location = new Point(20, y + 3), AutoSize = true, Font = new Font("Segoe UI", 10) };
            p.Controls.Add(lbl);
            TextBox txt = new TextBox { Location = new Point(120, y), Width = 260, Font = new Font("Segoe UI", 10) };
            p.Controls.Add(txt);
            return txt;
        }
    }
}