
using System;
using System.Collections.Generic;

namespace SalesManagement.FlyweightPattern
{
    // 1. Flyweight: Giao dien chung cho thong tin san pham dung chung
    public interface ISanPhamFlyweight
    {
        void HienThi(string maSanPham, double giaBan, int soLuong);
    }

    // 2. ConcreteFlyweight: Luu tru thong tin san pham dung chung
    public class ThongTinSanPham : ISanPhamFlyweight
    {
        private string _tenSanPham;
        private string _thuongHieu;
        private string _danhMuc;

        public ThongTinSanPham(
            string tenSanPham,
            string thuongHieu,
            string danhMuc)
        {
            _tenSanPham = tenSanPham;
            _thuongHieu = thuongHieu;
            _danhMuc = danhMuc;

            Console.WriteLine(
                $"Tao doi tuong dung chung: {_tenSanPham}");
        }

        public void HienThi(
            string maSanPham,
            double giaBan,
            int soLuong)
        {
            Console.WriteLine(
                $"Ma SP: {maSanPham} | " +
                $"Ten: {_tenSanPham} | " +
                $"Thuong hieu: {_thuongHieu} | " +
                $"Danh muc: {_danhMuc} | " +
                $"Gia ban: {giaBan:N0} VND | " +
                $"So luong: {soLuong}");
        }
    }

    // 3. FlyweightFactory: Quan ly va tai su dung doi tuong san pham
    public class SanPhamFactory
    {
        private readonly Dictionary<string, ISanPhamFlyweight>
            _sanPham = new Dictionary<string, ISanPhamFlyweight>();

        public ISanPhamFlyweight LaySanPham(
            string tenSanPham,
            string thuongHieu,
            string danhMuc)
        {
            string key = $"{tenSanPham}|{thuongHieu}|{danhMuc}";

            if (!_sanPham.ContainsKey(key))
            {
                _sanPham[key] = new ThongTinSanPham(
                    tenSanPham,
                    thuongHieu,
                    danhMuc);
            }

            return _sanPham[key];
        }

        public int SoLuongDoiTuongDungChung()
        {
            return _sanPham.Count;
        }
    }

    // 4. Context: Thong tin rieng cua tung san pham trong kho
    public class SanPhamKho
    {
        private string _maSanPham;
        private double _giaBan;
        private int _soLuong;
        private ISanPhamFlyweight _thongTinSanPham;

        public SanPhamKho(
            string maSanPham,
            double giaBan,
            int soLuong,
            ISanPhamFlyweight thongTinSanPham)
        {
            _maSanPham = maSanPham;
            _giaBan = giaBan;
            _soLuong = soLuong;
            _thongTinSanPham = thongTinSanPham;
        }

        public void HienThiThongTin()
        {
            _thongTinSanPham.HienThi(
                _maSanPham,
                _giaBan,
                _soLuong);
        }
    }

    // 5. Client: He thong quan ly ban hang
    class Program
    {
        static void Main(string[] args)
        {
            SanPhamFactory factory = new SanPhamFactory();

            List<SanPhamKho> danhSachSanPham =
                new List<SanPhamKho>();

            // Nhieu san pham cung loai duoc dung chung thong tin
            danhSachSanPham.Add(new SanPhamKho(
                "SP001",
                18000000,
                5,
                factory.LaySanPham(
                    "Laptop Dell",
                    "Dell",
                    "May tinh")));

            danhSachSanPham.Add(new SanPhamKho(
                "SP002",
                18500000,
                3,
                factory.LaySanPham(
                    "Laptop Dell",
                    "Dell",
                    "May tinh")));

            danhSachSanPham.Add(new SanPhamKho(
                "SP003",
                500000,
                10,
                factory.LaySanPham(
                    "Chuot khong day",
                    "Logitech",
                    "Phu kien")));

            danhSachSanPham.Add(new SanPhamKho(
                "SP004",
                550000,
                7,
                factory.LaySanPham(
                    "Chuot khong day",
                    "Logitech",
                    "Phu kien")));

            danhSachSanPham.Add(new SanPhamKho(
                "SP005",
                1200000,
                8,
                factory.LaySanPham(
                    "Ban phim co",
                    "Keychron",
                    "Phu kien")));

            // Hien thi danh sach san pham
            Console.WriteLine(
                "\n===== DANH SACH SAN PHAM =====");

            foreach (SanPhamKho sanPham in danhSachSanPham)
            {
                sanPham.HienThiThongTin();
            }

            // Thong ke doi tuong dung chung
            Console.WriteLine(
                "\n===== THONG KE HE THONG =====");

            Console.WriteLine(
                $"Tong so san pham trong kho: {danhSachSanPham.Count}");

            Console.WriteLine(
                $"So doi tuong thong tin dung chung: " +
                $"{factory.SoLuongDoiTuongDungChung()}");

            Console.ReadKey();
        }
    }
}
