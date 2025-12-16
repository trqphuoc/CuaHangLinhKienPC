using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq; // Thư viện quan trọng để xử lý XML

namespace QuanLyLinhKienPC
{
    public class XmlHelper
    {
        private string path = "LinhKien.xml";

        // Chức năng 1: Đọc file XML lên thành Danh sách
        public List<LinhKien> DocFile()
        {
            List<LinhKien> list = new List<LinhKien>();

            // Nếu chưa có file thì trả về danh sách rỗng để tránh lỗi
            if (!File.Exists(path)) return list;

            try
            {
                XDocument doc = XDocument.Load(path);
                // Tìm tất cả thẻ <LinhKien> trong file
                var elements = doc.Descendants("LinhKien");

                foreach (var e in elements)
                {
                    LinhKien lk = new LinhKien();
                    lk.MaLK = e.Element("MaLK").Value;
                    lk.TenLK = e.Element("TenLK").Value;
                    lk.HangSX = e.Element("HangSX").Value;
                    lk.LoaiLK = e.Element("LoaiLK").Value;
                    // Chuyển đổi từ chữ sang số
                    lk.SoLuong = int.Parse(e.Element("SoLuong").Value);
                    lk.DonGia = decimal.Parse(e.Element("DonGia").Value);

                    list.Add(lk);
                }
            }
            catch (Exception)
            {
                // Nếu file lỗi cấu trúc thì bỏ qua
            }

            return list;
        }

        // Chức năng 2: Ghi danh sách đè xuống file XML
        public void GhiFile(List<LinhKien> list)
        {
            // Tạo thẻ gốc <KhoLinhKien>
            XElement root = new XElement("KhoLinhKien");

            foreach (var item in list)
            {
                // Tạo từng thẻ <LinhKien> con
                XElement e = new XElement("LinhKien",
                    new XElement("MaLK", item.MaLK),
                    new XElement("TenLK", item.TenLK),
                    new XElement("HangSX", item.HangSX),
                    new XElement("LoaiLK", item.LoaiLK),
                    new XElement("SoLuong", item.SoLuong),
                    new XElement("DonGia", item.DonGia)
                );
                root.Add(e);
            }

            // Lưu xuống ổ cứng
            XDocument doc = new XDocument(root);
            doc.Save(path);
        }

        // --- PHẦN XỬ LÝ HÓA ĐƠN ---
        private string pathHD = "HoaDon.xml";

        // 1. Đọc hóa đơn
        public List<HoaDon> DocFileHoaDon()
        {
            List<HoaDon> list = new List<HoaDon>();
            if (!File.Exists(pathHD)) return list;
            try
            {
                XDocument doc = XDocument.Load(pathHD);
                var elements = doc.Descendants("HoaDon");
                foreach (var e in elements)
                {
                    list.Add(new HoaDon
                    {
                        MaHD = e.Element("MaHD").Value,
                        NgayLap = e.Element("NgayLap").Value,
                        MaNV = e.Element("MaNV").Value, // Đọc mã nhân viên
                        TenKhach = e.Element("TenKhach").Value,
                        TenSP = e.Element("TenSP").Value,
                        SoLuong = int.Parse(e.Element("SoLuong").Value),
                        ThanhTien = decimal.Parse(e.Element("ThanhTien").Value)
                    });
                }
            }
            catch { }
            return list;
        }

        // 2. Ghi hóa đơn (Thêm mới)
        public void GhiFileHoaDon(List<HoaDon> list)
        {
            XElement root = new XElement("DanhSachHoaDon");
            foreach (var item in list)
            {
                root.Add(new XElement("HoaDon",
                    new XElement("MaHD", item.MaHD),
                    new XElement("NgayLap", item.NgayLap),
                    new XElement("MaNV", item.MaNV), // Ghi mã nhân viên
                    new XElement("TenKhach", item.TenKhach),
                    new XElement("TenSP", item.TenSP),
                    new XElement("SoLuong", item.SoLuong),
                    new XElement("ThanhTien", item.ThanhTien)
                ));
            }
            XDocument doc = new XDocument(root);
            doc.Save(pathHD);
        }
    }
}