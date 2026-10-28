using System;
using System.Collections.Generic;

namespace SalesManagement.Behavioral.Observer
{
    // Observer interface
    public interface IObserver
    {
        void CapNhat(string thongBao);
    }

    // Subject interface
    public interface ISubject
    {
        void DangKy(IObserver observer);
        void HuyDangKy(IObserver observer);
        void ThongBaoTatCa(string thongBao);
    }

    // Don hang
    public class DonHang
    {
        public string MaDonHang { get; private set; }
        public string TenKhachHang { get; private set; }
        public string TenSanPham { get; private set; }
        public int SoLuong { get; private set; }
        public decimal TongTien { get; private set; }
        public string TrangThai { get; private set; }

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
            TongTien = soLuong * donGia;
            TrangThai = "Moi tao";
        }

        public void CapNhatTrangThai(string trangThai)
        {
            TrangThai = trangThai;
        }

        public void HienThiThongTin()
        {
            Console.WriteLine("\n===== THONG TIN DON HANG =====");
            Console.WriteLine($"Ma don hang: {MaDonHang}");
            Console.WriteLine($"Khach hang: {TenKhachHang}");
            Console.WriteLine($"San pham: {TenSanPham}");
            Console.WriteLine($"So luong: {SoLuong}");
            Console.WriteLine($"Tong tien: {TongTien:N0} VND");
            Console.WriteLine($"Trang thai: {TrangThai}");
            Console.WriteLine("==============================");
        }
    }

    // Concrete Subject
    public class HeThongDonHang : ISubject
    {
        private readonly List<IObserver> _observers =
            new List<IObserver>();

        private readonly List<DonHang> _danhSachDonHang =
            new List<DonHang>();

        public void DangKy(IObserver observer)
        {
            if (observer == null || _observers.Contains(observer))
                return;

            _observers.Add(observer);
            Console.WriteLine("Da dang ky nhan thong bao.");
        }

        public void HuyDangKy(IObserver observer)
        {
            if (_observers.Remove(observer))
            {
                Console.WriteLine("Da huy dang ky nhan thong bao.");
            }
            else
            {
                Console.WriteLine("Observer chua duoc dang ky.");
            }
        }

        public void ThongBaoTatCa(string thongBao)
        {
            foreach (IObserver observer in _observers.ToArray())
            {
                observer.CapNhat(thongBao);
            }
        }

        public void TaoDonHang(DonHang donHang)
        {
            _danhSachDonHang.Add(donHang);

            ThongBaoTatCa(
                $"Don hang {donHang.MaDonHang} da duoc tao. " +
                $"Khach hang: {donHang.TenKhachHang}.");

            donHang.HienThiThongTin();
        }

        public DonHang TimDonHang(string maDonHang)
        {
            return _danhSachDonHang.Find(
                d => d.MaDonHang.Equals(
                    maDonHang,
                    StringComparison.OrdinalIgnoreCase));
        }

        public void CapNhatTrangThai(
            string maDonHang,
            string trangThaiMoi)
        {
            DonHang donHang = TimDonHang(maDonHang);

            if (donHang == null)
            {
                Console.WriteLine("Khong tim thay don hang.");
                return;
            }

            string trangThaiCu = donHang.TrangThai;
            donHang.CapNhatTrangThai(trangThaiMoi);

            ThongBaoTatCa(
                $"Don hang {donHang.MaDonHang} cua " +
                $"{donHang.TenKhachHang} da thay doi trang thai: " +
                $"{trangThaiCu} -> {trangThaiMoi}.");

            donHang.HienThiThongTin();
        }

        public void HienThiDanhSachDonHang()
        {
            if (_danhSachDonHang.Count == 0)
            {
                Console.WriteLine("Chua co don hang nao.");
                return;
            }

            foreach (DonHang donHang in _danhSachDonHang)
            {
                donHang.HienThiThongTin();
            }
        }
    }

    // Concrete Observer: Khach hang
    public class KhachHang : IObserver
    {
        public string TenKhachHang { get; private set; }

        public KhachHang(string tenKhachHang)
        {
            TenKhachHang = tenKhachHang;
        }

        public void CapNhat(string thongBao)
        {
            Console.WriteLine(
                $"[THONG BAO KHACH HANG - {TenKhachHang}] {thongBao}");
        }
    }

    // Concrete Observer: Nhan vien ban hang
    public class NhanVienBanHang : IObserver
    {
        public string TenNhanVien { get; private set; }

        public NhanVienBanHang(string tenNhanVien)
        {
            TenNhanVien = tenNhanVien;
        }

        public void CapNhat(string thongBao)
        {
            Console.WriteLine(
                $"[THONG BAO NHAN VIEN - {TenNhanVien}] {thongBao}");
        }
    }

    // Concrete Observer: Nhan vien kho
    public class NhanVienKho : IObserver
    {
        public string TenNhanVien { get; private set; }

        public NhanVienKho(string tenNhanVien)
        {
            TenNhanVien = tenNhanVien;
        }

        public void CapNhat(string thongBao)
        {
            Console.WriteLine(
                $"[THONG BAO KHO - {TenNhanVien}] {thongBao}");
        }
    }

    // Concrete Observer: Quan ly
    public class QuanLy : IObserver
    {
        public string TenQuanLy { get; private set; }

        public QuanLy(string tenQuanLy)
        {
            TenQuanLy = tenQuanLy;
        }

        public void CapNhat(string thongBao)
        {
            Console.WriteLine(
                $"[THONG BAO QUAN LY - {TenQuanLy}] {thongBao}");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            HeThongDonHang heThong = new HeThongDonHang();

            KhachHang khachHang = new KhachHang("Nguyen Van An");
            NhanVienBanHang nhanVien =
                new NhanVienBanHang("Tran Thi Binh");
            NhanVienKho nhanVienKho =
                new NhanVienKho("Le Van Cuong");
            QuanLy quanLy = new QuanLy("Pham Thi Dung");

            heThong.DangKy(khachHang);
            heThong.DangKy(nhanVien);
            heThong.DangKy(nhanVienKho);
            heThong.DangKy(quanLy);

            while (true)
            {
                Console.WriteLine("\n===== QUAN LY BAN HANG =====");
                Console.WriteLine("1. Tao don hang");
                Console.WriteLine("2. Hien thi danh sach don hang");
                Console.WriteLine("3. Cap nhat trang thai don hang");
                Console.WriteLine("4. Dang ky nhan thong bao");
                Console.WriteLine("5. Huy dang ky nhan thong bao");
                Console.WriteLine("0. Thoat");
                Console.Write("Chon chuc nang: ");

                string luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1":
                        TaoDonHang(heThong);
                        break;

                    case "2":
                        heThong.HienThiDanhSachDonHang();
                        break;

                    case "3":
                        CapNhatTrangThaiDonHang(heThong);
                        break;

                    case "4":
                        QuanLyDangKyObserver(
                            heThong,
                            khachHang,
                            nhanVien,
                            nhanVienKho,
                            quanLy,
                            true);
                        break;

                    case "5":
                        QuanLyDangKyObserver(
                            heThong,
                            khachHang,
                            nhanVien,
                            nhanVienKho,
                            quanLy,
                            false);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Lua chon khong hop le.");
                        break;
                }
            }
        }

        static void TaoDonHang(HeThongDonHang heThong)
        {
            Console.Write("Nhap ma don hang: ");
            string maDonHang = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(maDonHang))
            {
                Console.WriteLine("Ma don hang khong hop le.");
                return;
            }

            if (heThong.TimDonHang(maDonHang) != null)
            {
                Console.WriteLine("Ma don hang da ton tai.");
                return;
            }

            Console.Write("Nhap ten khach hang: ");
            string tenKhachHang = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(tenKhachHang))
            {
                Console.WriteLine("Ten khach hang khong hop le.");
                return;
            }

            Console.WriteLine("\nChon san pham:");
            Console.WriteLine("1. Laptop - 15000000 VND");
            Console.WriteLine("2. Dien thoai - 8000000 VND");
            Console.WriteLine("3. Tai nghe - 500000 VND");
            Console.Write("Lua chon: ");

            string luaChonSanPham = Console.ReadLine();

            string tenSanPham;
            decimal donGia;

            switch (luaChonSanPham)
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
                    return;
            }

            Console.Write("Nhap so luong: ");

            if (!int.TryParse(Console.ReadLine(), out int soLuong) ||
                soLuong <= 0)
            {
                Console.WriteLine("So luong khong hop le.");
                return;
            }

            DonHang donHang = new DonHang(
                maDonHang,
                tenKhachHang,
                tenSanPham,
                soLuong,
                donGia);

            heThong.TaoDonHang(donHang);
        }

        static void CapNhatTrangThaiDonHang(HeThongDonHang heThong)
        {
            Console.Write("Nhap ma don hang: ");
            string maDonHang = Console.ReadLine();

            DonHang donHang = heThong.TimDonHang(maDonHang);

            if (donHang == null)
            {
                Console.WriteLine("Khong tim thay don hang.");
                return;
            }

            Console.WriteLine("\nChon trang thai moi:");
            Console.WriteLine("1. Cho thanh toan");
            Console.WriteLine("2. Da thanh toan");
            Console.WriteLine("3. Dang giao");
            Console.WriteLine("4. Da giao");
            Console.WriteLine("5. Da huy");
            Console.Write("Lua chon: ");

            string luaChon = Console.ReadLine();
            string trangThaiMoi;

            switch (luaChon)
            {
                case "1":
                    trangThaiMoi = "Cho thanh toan";
                    break;

                case "2":
                    trangThaiMoi = "Da thanh toan";
                    break;

                case "3":
                    trangThaiMoi = "Dang giao";
                    break;

                case "4":
                    trangThaiMoi = "Da giao";
                    break;

                case "5":
                    trangThaiMoi = "Da huy";
                    break;

                default:
                    Console.WriteLine("Trang thai khong hop le.");
                    return;
            }

            heThong.CapNhatTrangThai(maDonHang, trangThaiMoi);
        }

        static void QuanLyDangKyObserver(
            HeThongDonHang heThong,
            KhachHang khachHang,
            NhanVienBanHang nhanVien,
            NhanVienKho nhanVienKho,
            QuanLy quanLy,
            bool dangKy)
        {
            Console.WriteLine("\nChon doi tuong:");
            Console.WriteLine("1. Khach hang");
            Console.WriteLine("2. Nhan vien ban hang");
            Console.WriteLine("3. Nhan vien kho");
            Console.WriteLine("4. Quan ly");
            Console.Write("Lua chon: ");

            string luaChon = Console.ReadLine();
            IObserver observer;

            switch (luaChon)
            {
                case "1":
                    observer = khachHang;
                    break;

                case "2":
                    observer = nhanVien;
                    break;

                case "3":
                    observer = nhanVienKho;
                    break;

                case "4":
                    observer = quanLy;
                    break;

                default:
                    Console.WriteLine("Lua chon khong hop le.");
                    return;
            }

            if (dangKy)
            {
                heThong.DangKy(observer);
            }
            else
            {
                heThong.HuyDangKy(observer);
            }
        }
    }
}