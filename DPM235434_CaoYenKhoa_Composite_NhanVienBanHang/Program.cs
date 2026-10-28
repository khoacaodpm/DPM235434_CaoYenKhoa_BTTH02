
using System;
using System.Collections.Generic;

namespace SalesManagement.CompositePattern
{
    // 1. Component: Thanh phan chung cua san pham va nhom san pham
    public abstract class DanhMucSanPham
    {
        protected string Ten;

        public DanhMucSanPham(string ten)
        {
            Ten = ten;
        }

        public abstract void HienThi(int cap = 0);

        public abstract double TinhTongGiaTri();
    }

    // 2. Leaf: San pham cu the
    public class SanPham : DanhMucSanPham
    {
        private double _donGia;
        private int _soLuong;

        public SanPham(string ten, double donGia, int soLuong)
            : base(ten)
        {
            _donGia = donGia;
            _soLuong = soLuong;
        }

        public override void HienThi(int cap = 0)
        {
            Console.WriteLine(
                $"{new string(' ', cap * 4)}- {Ten} | " +
                $"Don gia: {_donGia:N0} VND | " +
                $"So luong: {_soLuong} | " +
                $"Thanh tien: {TinhTongGiaTri():N0} VND");
        }

        public override double TinhTongGiaTri()
        {
            return _donGia * _soLuong;
        }
    }

    // 3. Composite: Nhom san pham hoac danh muc san pham
    public class NhomSanPham : DanhMucSanPham
    {
        private List<DanhMucSanPham> _thanhPhan =
            new List<DanhMucSanPham>();

        public NhomSanPham(string ten) : base(ten)
        {
        }

        public void Them(DanhMucSanPham thanhPhan)
        {
            _thanhPhan.Add(thanhPhan);
        }

        public void Xoa(DanhMucSanPham thanhPhan)
        {
            _thanhPhan.Remove(thanhPhan);
        }

        public override void HienThi(int cap = 0)
        {
            Console.WriteLine(
                $"{new string(' ', cap * 4)}+ Danh muc: {Ten}");

            foreach (DanhMucSanPham thanhPhan in _thanhPhan)
            {
                thanhPhan.HienThi(cap + 1);
            }

            Console.WriteLine(
                $"{new string(' ', cap * 4)}Tong gia tri {Ten}: " +
                $"{TinhTongGiaTri():N0} VND");
        }

        public override double TinhTongGiaTri()
        {
            double tong = 0;

            foreach (DanhMucSanPham thanhPhan in _thanhPhan)
            {
                tong += thanhPhan.TinhTongGiaTri();
            }

            return tong;
        }
    }

    // 4. Client: He thong quan ly ban hang
    class Program
    {
        static void Main(string[] args)
        {
            // Tao cac san pham
            DanhMucSanPham laptop = new SanPham(
                "Laptop Dell", 18000000, 5);

            DanhMucSanPham chuot = new SanPham(
                "Chuot Logitech", 500000, 10);

            DanhMucSanPham banPhim = new SanPham(
                "Ban phim co", 1200000, 8);

            DanhMucSanPham dienThoai = new SanPham(
                "iPhone", 22000000, 3);

            // Tao danh muc con
            NhomSanPham mayTinh = new NhomSanPham("May tinh va phu kien");
            mayTinh.Them(laptop);
            mayTinh.Them(chuot);
            mayTinh.Them(banPhim);

            NhomSanPham thietBiDiDong = new NhomSanPham("Thiet bi di dong");
            thietBiDiDong.Them(dienThoai);

            // Tao danh muc tong
            NhomSanPham cuaHang = new NhomSanPham("Cua hang dien tu");
            cuaHang.Them(mayTinh);
            cuaHang.Them(thietBiDiDong);

            // Hien thi toan bo danh muc va san pham
            Console.WriteLine("===== QUAN LY DANH MUC SAN PHAM =====\n");
            cuaHang.HienThi();

            // Tinh tong gia tri hang hoa
            Console.WriteLine("\n===== BAO CAO KHO HANG =====");
            Console.WriteLine(
                $"Tong gia tri hang hoa: {cuaHang.TinhTongGiaTri():N0} VND");

            Console.ReadKey();
        }
    }
}
