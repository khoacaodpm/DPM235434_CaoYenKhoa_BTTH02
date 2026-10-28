using System;

namespace SalesManagement.Behavioral.Strategy
{
    // Strategy interface
    public interface IChienLuocThanhToan
    {
        bool ThanhToan(decimal soTien);
        string LayTenPhuongThuc();
    }

    // Concrete Strategy: Tien mat
    public class ThanhToanTienMat : IChienLuocThanhToan
    {
        public bool ThanhToan(decimal soTien)
        {
            Console.WriteLine(
                $"Thanh toan tien mat: {soTien:N0} VND");

            Console.WriteLine("Thanh toan tien mat thanh cong.");
            return true;
        }

        public string LayTenPhuongThuc()
        {
            return "Tien mat";
        }
    }

    // Concrete Strategy: Chuyen khoan ngan hang
    public class ThanhToanChuyenKhoan : IChienLuocThanhToan
    {
        public bool ThanhToan(decimal soTien)
        {
            Console.WriteLine(
                $"Chuyen khoan ngan hang: {soTien:N0} VND");

            Console.WriteLine("Chuyen khoan thanh cong.");
            return true;
        }

        public string LayTenPhuongThuc()
        {
            return "Chuyen khoan ngan hang";
        }
    }

    // Concrete Strategy: The ngan hang
    public class ThanhToanTheNganHang : IChienLuocThanhToan
    {
        public bool ThanhToan(decimal soTien)
        {
            Console.WriteLine(
                $"Thanh toan the ngan hang: {soTien:N0} VND");

            Console.WriteLine("Thanh toan the thanh cong.");
            return true;
        }

        public string LayTenPhuongThuc()
        {
            return "The ngan hang";
        }
    }

    // Concrete Strategy: Vi dien tu
    public class ThanhToanViDienTu : IChienLuocThanhToan
    {
        public bool ThanhToan(decimal soTien)
        {
            Console.WriteLine(
                $"Thanh toan qua vi dien tu: {soTien:N0} VND");

            Console.WriteLine("Thanh toan vi dien tu thanh cong.");
            return true;
        }

        public string LayTenPhuongThuc()
        {
            return "Vi dien tu";
        }
    }

    // Context: Don hang
    public class DonHang
    {
        public string MaDonHang { get; private set; }
        public string TenKhachHang { get; private set; }
        public string TenSanPham { get; private set; }
        public int SoLuong { get; private set; }
        public decimal DonGia { get; private set; }
        public decimal TongTien => SoLuong * DonGia;
        public string TrangThai { get; private set; }

        private IChienLuocThanhToan _chienLuocThanhToan;

        public DonHang(
            string maDonHang,
            string tenKhachHang,
            string tenSanPham,
            int soLuong,
            decimal donGia)
        {
            MaDonHang = maDonHang;
            TenKhachHang = tenKhachHang;
            TenSanPham = tenSanPham;
            SoLuong = soLuong;
            DonGia = donGia;
            TrangThai = "Chua thanh toan";
        }

        public void DatChienLuocThanhToan(
            IChienLuocThanhToan chienLuocThanhToan)
        {
            _chienLuocThanhToan = chienLuocThanhToan;
        }

        public void ThucHienThanhToan()
        {
            if (TrangThai == "Da thanh toan")
            {
                Console.WriteLine("Don hang da duoc thanh toan.");
                return;
            }

            if (_chienLuocThanhToan == null)
            {
                Console.WriteLine("Vui long chon phuong thuc thanh toan.");
                return;
            }

            bool thanhCong = _chienLuocThanhToan.ThanhToan(TongTien);

            if (thanhCong)
            {
                TrangThai = "Da thanh toan";

                Console.WriteLine(
                    $"Phuong thuc: {_chienLuocThanhToan.LayTenPhuongThuc()}");

                Console.WriteLine(
                    $"Don hang {MaDonHang} da thanh toan thanh cong.");
            }
            else
            {
                Console.WriteLine("Thanh toan that bai.");
            }
        }

        public void HienThiThongTin()
        {
            Console.WriteLine("\n===== THONG TIN DON HANG =====");
            Console.WriteLine($"Ma don hang: {MaDonHang}");
            Console.WriteLine($"Khach hang: {TenKhachHang}");
            Console.WriteLine($"San pham: {TenSanPham}");
            Console.WriteLine($"So luong: {SoLuong}");
            Console.WriteLine($"Don gia: {DonGia:N0} VND");
            Console.WriteLine($"Tong tien: {TongTien:N0} VND");
            Console.WriteLine($"Trang thai: {TrangThai}");
            Console.WriteLine("==============================");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            DonHang donHang = null;

            while (true)
            {
                Console.WriteLine("\n===== HE THONG QUAN LY BAN HANG =====");
                Console.WriteLine("1. Tao don hang");
                Console.WriteLine("2. Hien thi thong tin don hang");
                Console.WriteLine("3. Thanh toan bang tien mat");
                Console.WriteLine("4. Thanh toan bang chuyen khoan");
                Console.WriteLine("5. Thanh toan bang the ngan hang");
                Console.WriteLine("6. Thanh toan bang vi dien tu");
                Console.WriteLine("0. Thoat");
                Console.Write("Chon chuc nang: ");

                string luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1":
                        donHang = TaoDonHang();
                        break;

                    case "2":
                        if (KiemTraDonHang(donHang))
                        {
                            donHang.HienThiThongTin();
                        }
                        break;

                    case "3":
                        ThanhToan(donHang, new ThanhToanTienMat());
                        break;

                    case "4":
                        ThanhToan(donHang, new ThanhToanChuyenKhoan());
                        break;

                    case "5":
                        ThanhToan(donHang, new ThanhToanTheNganHang());
                        break;

                    case "6":
                        ThanhToan(donHang, new ThanhToanViDienTu());
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Lua chon khong hop le.");
                        break;
                }
            }
        }

        static DonHang TaoDonHang()
        {
            Console.Write("Nhap ma don hang: ");
            string maDonHang = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(maDonHang))
            {
                Console.WriteLine("Ma don hang khong hop le.");
                return null;
            }

            Console.Write("Nhap ten khach hang: ");
            string tenKhachHang = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(tenKhachHang))
            {
                Console.WriteLine("Ten khach hang khong hop le.");
                return null;
            }

            Console.WriteLine("\nChon san pham:");
            Console.WriteLine("1. Laptop - 15000000 VND");
            Console.WriteLine("2. Dien thoai - 8000000 VND");
            Console.WriteLine("3. Tai nghe - 500000 VND");
            Console.Write("Lua chon: ");

            string luaChon = Console.ReadLine();

            string tenSanPham;
            decimal donGia;

            switch (luaChon)
            {
                case "1":
                    tenSanPham = "Laptop";
                    donGia = 15000000;
                    break;

                case "2":
                    tenSanPham = "Dien thoai";
                    donGia = 8000000;
                    break;

                case "3":
                    tenSanPham = "Tai nghe";
                    donGia = 500000;
                    break;

                default:
                    Console.WriteLine("San pham khong hop le.");
                    return null;
            }

            Console.Write("Nhap so luong: ");

            if (!int.TryParse(Console.ReadLine(), out int soLuong) ||
                soLuong <= 0)
            {
                Console.WriteLine("So luong khong hop le.");
                return null;
            }

            DonHang donHang = new DonHang(
                maDonHang,
                tenKhachHang,
                tenSanPham,
                soLuong,
                donGia);

            Console.WriteLine("Tao don hang thanh cong.");
            return donHang;
        }

        static void ThanhToan(
            DonHang donHang,
            IChienLuocThanhToan chienLuocThanhToan)
        {
            if (!KiemTraDonHang(donHang))
                return;

            donHang.DatChienLuocThanhToan(chienLuocThanhToan);
            donHang.ThucHienThanhToan();
        }

        static bool KiemTraDonHang(DonHang donHang)
        {
            if (donHang == null)
            {
                Console.WriteLine("Vui long tao don hang truoc.");
                return false;
            }

            return true;
        }
    }
}