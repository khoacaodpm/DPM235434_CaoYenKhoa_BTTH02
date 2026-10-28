
using System;

namespace SalesManagement.BridgePattern
{
    // 1. Implementor: Giao dien thanh toan
    public interface IThanhToan
    {
        string ThucHienThanhToan(double soTien);
    }

    // 2. Concrete Implementor A: Thanh toan tien mat
    public class ThanhToanTienMat : IThanhToan
    {
        public string ThucHienThanhToan(double soTien)
        {
            return $"Thanh toan tien mat: {soTien:N0} VND";
        }
    }

    // 3. Concrete Implementor B: Thanh toan chuyen khoan
    public class ThanhToanChuyenKhoan : IThanhToan
    {
        public string ThucHienThanhToan(double soTien)
        {
            return $"Thanh toan chuyen khoan: {soTien:N0} VND";
        }
    }

    // 4. Abstraction: Don hang
    public class DonHang
    {
        protected IThanhToan _thanhToan;

        public string TenSanPham { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }

        public DonHang(
            string tenSanPham,
            double donGia,
            int soLuong,
            IThanhToan thanhToan)
        {
            TenSanPham = tenSanPham;
            DonGia = donGia;
            SoLuong = soLuong;
            _thanhToan = thanhToan;
        }

        public virtual void XuLyDonHang()
        {
            double tongTien = DonGia * SoLuong;

            Console.WriteLine($"San pham: {TenSanPham}");
            Console.WriteLine($"Don gia: {DonGia:N0} VND");
            Console.WriteLine($"So luong: {SoLuong}");
            Console.WriteLine($"Tong tien: {tongTien:N0} VND");
            Console.WriteLine(_thanhToan.ThucHienThanhToan(tongTien));
        }
    }

    // 5. Refined Abstraction A: Don hang thuong
    public class DonHangThuong : DonHang
    {
        public DonHangThuong(
            string tenSanPham,
            double donGia,
            int soLuong,
            IThanhToan thanhToan)
            : base(tenSanPham, donGia, soLuong, thanhToan)
        {
        }

        public override void XuLyDonHang()
        {
            Console.WriteLine("\n===== DON HANG THUONG =====");
            base.XuLyDonHang();
        }
    }

    // 6. Refined Abstraction B: Don hang VIP
    public class DonHangVIP : DonHang
    {
        public DonHangVIP(
            string tenSanPham,
            double donGia,
            int soLuong,
            IThanhToan thanhToan)
            : base(tenSanPham, donGia, soLuong, thanhToan)
        {
        }

        public override void XuLyDonHang()
        {
            Console.WriteLine("\n===== DON HANG VIP =====");

            double tongTien = DonGia * SoLuong;
            double giamGia = tongTien * 0.10;
            double thanhTien = tongTien - giamGia;

            Console.WriteLine($"San pham: {TenSanPham}");
            Console.WriteLine($"Don gia: {DonGia:N0} VND");
            Console.WriteLine($"So luong: {SoLuong}");
            Console.WriteLine($"Tong tien: {tongTien:N0} VND");
            Console.WriteLine($"Giam gia VIP (10%): {giamGia:N0} VND");
            Console.WriteLine($"Thanh tien: {thanhTien:N0} VND");
            Console.WriteLine(
                _thanhToan.ThucHienThanhToan(thanhTien));
        }
    }

    // 7. Client: Chuong trinh quan ly ban hang
    class Program
    {
        static void Main(string[] args)
        {
            // Don hang thuong + thanh toan tien mat
            DonHang donHang1 = new DonHangThuong(
                "Chuot may tinh",
                250000,
                2,
                new ThanhToanTienMat());

            donHang1.XuLyDonHang();

            // Don hang VIP + thanh toan chuyen khoan
            DonHang donHang2 = new DonHangVIP(
                "Laptop",
                20000000,
                1,
                new ThanhToanChuyenKhoan());

            donHang2.XuLyDonHang();

            // Don hang VIP + thanh toan tien mat
            DonHang donHang3 = new DonHangVIP(
                "Ban phim co",
                1500000,
                2,
                new ThanhToanTienMat());

            donHang3.XuLyDonHang();

            Console.ReadKey();
        }
    }
}
