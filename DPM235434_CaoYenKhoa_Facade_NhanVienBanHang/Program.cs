
using System;
using System.Collections.Generic;

namespace SalesManagement.FacadePattern
{
    // 1. San pham
    public class SanPham
    {
        public string Ten { get; set; }
        public double DonGia { get; set; }
        public int SoLuongTon { get; set; }

        public SanPham(string ten, double donGia, int soLuongTon)
        {
            Ten = ten;
            DonGia = donGia;
            SoLuongTon = soLuongTon;
        }
    }

    // 2. He thong kho hang
    public class QuanLyKho
    {
        public bool KiemTraTonKho(SanPham sanPham, int soLuong)
        {
            if (sanPham.SoLuongTon >= soLuong)
            {
                Console.WriteLine("Kho hang: San pham du so luong.");
                return true;
            }

            Console.WriteLine("Kho hang: Khong du so luong trong kho.");
            return false;
        }

        public void CapNhatTonKho(SanPham sanPham, int soLuong)
        {
            sanPham.SoLuongTon -= soLuong;
            Console.WriteLine(
                $"Kho hang: Da cap nhat ton kho, con {sanPham.SoLuongTon} san pham.");
        }
    }

    // 3. He thong thanh toan
    public class XuLyThanhToan
    {
        public bool ThanhToan(double soTien)
        {
            Console.WriteLine($"Thanh toan: Dang xu ly {soTien:N0} VND.");
            Console.WriteLine("Thanh toan: Giao dich thanh cong.");
            return true;
        }
    }

    // 4. He thong lap hoa don
    public class LapHoaDon
    {
        public void TaoHoaDon(
            string maDonHang,
            SanPham sanPham,
            int soLuong,
            double tongTien)
        {
            Console.WriteLine("\n===== HOA DON BAN HANG =====");
            Console.WriteLine($"Ma don hang: {maDonHang}");
            Console.WriteLine($"San pham: {sanPham.Ten}");
            Console.WriteLine($"Don gia: {sanPham.DonGia:N0} VND");
            Console.WriteLine($"So luong: {soLuong}");
            Console.WriteLine($"Tong tien: {tongTien:N0} VND");
        }
    }

    // 5. He thong giao hang
    public class QuanLyGiaoHang
    {
        public void TaoPhieuGiaoHang(string maDonHang)
        {
            Console.WriteLine(
                $"Giao hang: Da tao phieu giao hang cho don {maDonHang}.");
        }
    }

    // 6. Facade: Don gian hoa quy trinh ban hang
    public class BanHangFacade
    {
        private readonly QuanLyKho _quanLyKho;
        private readonly XuLyThanhToan _xuLyThanhToan;
        private readonly LapHoaDon _lapHoaDon;
        private readonly QuanLyGiaoHang _quanLyGiaoHang;

        public BanHangFacade()
        {
            _quanLyKho = new QuanLyKho();
            _xuLyThanhToan = new XuLyThanhToan();
            _lapHoaDon = new LapHoaDon();
            _quanLyGiaoHang = new QuanLyGiaoHang();
        }

        public void DatHang(
            string maDonHang,
            SanPham sanPham,
            int soLuong)
        {
            Console.WriteLine("\n===== BAT DAU XU LY DON HANG =====");

            if (sanPham == null || soLuong <= 0)
            {
                Console.WriteLine("Loi: Thong tin don hang khong hop le.");
                return;
            }

            if (!_quanLyKho.KiemTraTonKho(sanPham, soLuong))
            {
                Console.WriteLine("Don hang that bai.");
                return;
            }

            double tongTien = sanPham.DonGia * soLuong;

            if (!_xuLyThanhToan.ThanhToan(tongTien))
            {
                Console.WriteLine("Don hang that bai do thanh toan.");
                return;
            }

            _quanLyKho.CapNhatTonKho(sanPham, soLuong);

            _lapHoaDon.TaoHoaDon(
                maDonHang,
                sanPham,
                soLuong,
                tongTien);

            _quanLyGiaoHang.TaoPhieuGiaoHang(maDonHang);

            Console.WriteLine("\nDat hang thanh cong!");
        }
    }

    // 7. Client: Chuong trinh chinh
    class Program
    {
        static void Main(string[] args)
        {
            SanPham laptop = new SanPham(
                "Laptop Dell", 18000000, 10);

            SanPham chuot = new SanPham(
                "Chuot Logitech", 500000, 3);

            // Khoi tao Facade
            BanHangFacade banHang = new BanHangFacade();

            // Khach hang dat mua san pham
            banHang.DatHang("DH001", laptop, 2);

            banHang.DatHang("DH002", chuot, 5);

            banHang.DatHang("DH003", laptop, 1);

            Console.WriteLine("\n===== KET THUC CHUONG TRINH =====");
            Console.ReadKey();
        }
    }
}
