using System;

namespace QuanLyLinhKienPC
{
    public class HoaDon
    {
        public string MaHD { get; set; }
        public string NgayLap { get; set; }
        public string MaNV { get; set; } // Quan trọng: Lưu người bán
        public string TenKhach { get; set; }
        public string TenSP { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien { get; set; }
    }
}