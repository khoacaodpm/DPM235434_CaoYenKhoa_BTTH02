using System;

namespace SalesManagement.Behavioral.State
{
    // State interface
    public interface ITrangThaiDonHang
    {
        void XuLy(DonHang donHang);
        void Huy(DonHang donHang);
        string LayTenTrangThai();
    }

    // Context
    public class DonHang
    {
        public string MaDonHang { get; private set; }
        public string TenKhachHang { get; private set; }
        public string TenSanPham { get; private set; }
        public int SoLuong { get; private set; }
        public decimal DonGia { get; private set; }

        public decimal TongTien => SoLuong * DonGia;

        private ITrangThaiDonHang _trangThai;

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

            _trangThai = new TrangThaiMoiTao();
        }

        public void ChuyenTrangThai(ITrangThaiDonHang trangThaiMoi)
        {
            _trangThai = trangThaiMoi;
        }

        public string LayTenTrangThai()
        {
            return _trangThai.LayTenTrangThai();
        }

        public void ThanhToan()
        {
            _trangThai.XuLy(this);
        }

        public void HuyDonHang()
        {
            _trangThai.Huy(this);
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
            Console.WriteLine($"Trang thai: {LayTenTrangThai()}");
            Console.WriteLine("==============================");
        }
    }

    // Concrete State: Moi tao
    public class TrangThaiMoiTao : ITrangThaiDonHang
    {
        public string LayTenTrangThai()
        {
            return "Moi tao";
        }

        public void XuLy(DonHang donHang)
        {
            Console.WriteLine(
                $"Don hang {donHang.MaDonHang} da duoc thanh toan.");

            donHang.ChuyenTrangThai(new TrangThaiDaThanhToan());
        }

        public void Huy(DonHang donHang)
        {
            Console.WriteLine(
                $"Don hang {donHang.MaDonHang} da bi huy.");

            donHang.ChuyenTrangThai(new TrangThaiDaHuy());
        }
    }

    // Concrete State: Da thanh toan
    public class TrangThaiDaThanhToan : ITrangThaiDonHang
    {
        public string LayTenTrangThai()
        {
            return "Da thanh toan";
        }

        public void XuLy(DonHang donHang)
        {
            Console.WriteLine(
                $"Don hang {donHang.MaDonHang} da duoc thanh toan truoc do.");
        }

        public void Huy(DonHang donHang)
        {
            Console.WriteLine(
                $"Don hang {donHang.MaDonHang} da bi huy sau khi thanh toan.");

            donHang.ChuyenTrangThai(new TrangThaiDaHuy());
        }
    }

    // Concrete State: Dang giao
    public class TrangThaiDangGiao : ITrangThaiDonHang
    {
        public string LayTenTrangThai()
        {
            return "Dang giao";
        }

        public void XuLy(DonHang donHang)
        {
            Console.WriteLine(
                $"Don hang {donHang.MaDonHang} dang duoc giao.");
        }

        public void Huy(DonHang donHang)
        {
            Console.WriteLine(
                "Khong the huy don hang khi dang giao.");

        }
    }

    // Concrete State: Da giao
    public class TrangThaiDaGiao : ITrangThaiDonHang
    {
        public string LayTenTrangThai()
        {
            return "Da giao";
        }

        public void XuLy(DonHang donHang)
        {
            Console.WriteLine(
                $"Don hang {donHang.MaDonHang} da giao thanh cong.");
        }

        public void Huy(DonHang donHang)
        {
            Console.WriteLine(
                "Khong the huy don hang da giao.");
        }
    }

    // Concrete State: Da huy
    public class TrangThaiDaHuy : ITrangThaiDonHang
    {
        public string LayTenTrangThai()
        {
            return "Da huy";
        }

        public void XuLy(DonHang donHang)
        {
            Console.WriteLine(
                $"Khong the thanh toan don hang {donHang.MaDonHang} da huy.");
        }

        public void Huy(DonHang donHang)
        {
            Console.WriteLine(
                $"Don hang {donHang.MaDonHang} da bi huy truoc do.");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            DonHang donHang = null;

            while (true)
            {
                Console.WriteLine("\n===== QUAN LY BAN HANG =====");
                Console.WriteLine("1. Tao don hang");
                Console.WriteLine("2. Hien thi thong tin don hang");
                Console.WriteLine("3. Thanh toan don hang");
                Console.WriteLine("4. Chuyen sang trang thai dang giao");
                Console.WriteLine("5. Xac nhan da giao hang");
                Console.WriteLine("6. Huy don hang");
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
                            donHang.HienThiThongTin();
                        break;

                    case "3":
                        if (KiemTraDonHang(donHang))
                            donHang.ThanhToan();
                        break;

                    case "4":
                        if (KiemTraDonHang(donHang))
                            ChuyenSangDangGiao(donHang);
                        break;

                    case "5":
                        if (KiemTraDonHang(donHang))
                            XacNhanDaGiao(donHang);
                        break;

                    case "6":
                        if (KiemTraDonHang(donHang))
                            donHang.HuyDonHang();
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

            Console.Write("Nhap ten khach hang: ");
            string tenKhachHang = Console.ReadLine();

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

        static bool KiemTraDonHang(DonHang donHang)
        {
            if (donHang == null)
            {
                Console.WriteLine("Vui long tao don hang truoc.");
                return false;
            }

            return true;
        }

        static void ChuyenSangDangGiao(DonHang donHang)
        {
            if (donHang.LayTenTrangThai() != "Da thanh toan")
            {
                Console.WriteLine(
                    "Chi co the giao hang sau khi thanh toan.");
                return;
            }

            donHang.ChuyenTrangThai(new TrangThaiDangGiao());

            Console.WriteLine(
                $"Don hang {donHang.MaDonHang} dang duoc giao.");
        }

        static void XacNhanDaGiao(DonHang donHang)
        {
            if (donHang.LayTenTrangThai() != "Dang giao")
            {
                Console.WriteLine(
                    "Don hang chua o trang thai dang giao.");
                return;
            }

            donHang.ChuyenTrangThai(new TrangThaiDaGiao());

            Console.WriteLine(
                $"Don hang {donHang.MaDonHang} da giao thanh cong.");
        }
    }
}