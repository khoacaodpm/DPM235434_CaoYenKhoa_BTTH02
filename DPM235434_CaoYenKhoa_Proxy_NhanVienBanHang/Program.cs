
using System;
using System.Collections.Generic;

namespace SalesManagement.ProxyPattern
{
    // 1. Subject: Giao dien quan ly san pham
    public interface IQuanLySanPham
    {
        void XemSanPham();
        void ThemSanPham(string tenSanPham, double giaBan, int soLuong);
        void XoaSanPham(string tenSanPham);
    }

    // 2. RealSubject: He thong quan ly san pham thuc te
    public class QuanLySanPham : IQuanLySanPham
    {
        private List<string> _danhSachSanPham =
            new List<string>();

        public QuanLySanPham()
        {
            _danhSachSanPham.Add("Laptop Dell - 18000000 VND - So luong: 5");
            _danhSachSanPham.Add("Chuot Logitech - 500000 VND - So luong: 10");
            _danhSachSanPham.Add("Ban phim co - 1200000 VND - So luong: 8");
        }

        public void XemSanPham()
        {
            Console.WriteLine("\n===== DANH SACH SAN PHAM =====");

            if (_danhSachSanPham.Count == 0)
            {
                Console.WriteLine("Khong co san pham nao.");
                return;
            }

            foreach (string sanPham in _danhSachSanPham)
            {
                Console.WriteLine(sanPham);
            }
        }

        public void ThemSanPham(
            string tenSanPham,
            double giaBan,
            int soLuong)
        {
            _danhSachSanPham.Add(
                $"{tenSanPham} - {giaBan:N0} VND - So luong: {soLuong}");

            Console.WriteLine(
                $"Da them san pham: {tenSanPham}");
        }

        public void XoaSanPham(string tenSanPham)
        {
            int index = _danhSachSanPham.FindIndex(
                sp => sp.StartsWith(
                    tenSanPham + " - ",
                    StringComparison.OrdinalIgnoreCase));

            if (index >= 0)
            {
                _danhSachSanPham.RemoveAt(index);
                Console.WriteLine(
                    $"Da xoa san pham: {tenSanPham}");
            }
            else
            {
                Console.WriteLine(
                    $"Khong tim thay san pham: {tenSanPham}");
            }
        }
    }

    // 3. Proxy: Kiem soat quyen truy cap he thong
    public class QuanLySanPhamProxy : IQuanLySanPham
    {
        private QuanLySanPham _quanLySanPham;
        private string _vaiTro;

        public QuanLySanPhamProxy(string vaiTro)
        {
            _vaiTro = vaiTro;
        }

        private QuanLySanPham LayHeThong()
        {
            if (_quanLySanPham == null)
            {
                _quanLySanPham = new QuanLySanPham();
            }

            return _quanLySanPham;
        }

        public void XemSanPham()
        {
            if (_vaiTro == "Admin" ||
                _vaiTro == "NhanVien" ||
                _vaiTro == "KhachHang")
            {
                Console.WriteLine(
                    $"[{_vaiTro}] Duoc phep xem san pham.");

                LayHeThong().XemSanPham();
            }
            else
            {
                Console.WriteLine(
                    $"[{_vaiTro}] Khong co quyen xem san pham.");
            }
        }

        public void ThemSanPham(
            string tenSanPham,
            double giaBan,
            int soLuong)
        {
            if (_vaiTro == "Admin")
            {
                if (string.IsNullOrWhiteSpace(tenSanPham) ||
                    giaBan <= 0 ||
                    soLuong < 0)
                {
                    Console.WriteLine(
                        "Thong tin san pham khong hop le.");
                    return;
                }

                Console.WriteLine(
                    "[Admin] Duoc phep them san pham.");

                LayHeThong().ThemSanPham(
                    tenSanPham,
                    giaBan,
                    soLuong);
            }
            else
            {
                Console.WriteLine(
                    $"[{_vaiTro}] Khong co quyen them san pham.");
            }
        }

        public void XoaSanPham(string tenSanPham)
        {
            if (_vaiTro == "Admin")
            {
                Console.WriteLine(
                    "[Admin] Duoc phep xoa san pham.");

                LayHeThong().XoaSanPham(tenSanPham);
            }
            else
            {
                Console.WriteLine(
                    $"[{_vaiTro}] Khong co quyen xoa san pham.");
            }
        }
    }

    // 4. Client: Chuong trinh quan ly ban hang
    class Program
    {
        static void Main(string[] args)
        {
            IQuanLySanPham admin =
                new QuanLySanPhamProxy("Admin");

            IQuanLySanPham nhanVien =
                new QuanLySanPhamProxy("NhanVien");

            IQuanLySanPham khachHang =
                new QuanLySanPhamProxy("KhachHang");

            Console.WriteLine(
                "===== HE THONG QUAN LY BAN HANG =====");

            // Admin xem va quan ly san pham
            Console.WriteLine("\n--- ADMIN ---");
            admin.XemSanPham();

            admin.ThemSanPham(
                "Tai nghe Bluetooth",
                850000,
                12);

            admin.XemSanPham();

            admin.XoaSanPham("Chuot Logitech");
            admin.XemSanPham();

            // Nhan vien chi duoc xem san pham
            Console.WriteLine("\n--- NHAN VIEN ---");
            nhanVien.XemSanPham();

            nhanVien.ThemSanPham(
                "May in Canon",
                2500000,
                4);

            nhanVien.XoaSanPham("Laptop Dell");

            // Khach hang chi duoc xem san pham
            Console.WriteLine("\n--- KHACH HANG ---");
            khachHang.XemSanPham();

            khachHang.ThemSanPham(
                "May tinh bang",
                7000000,
                3);

            Console.WriteLine(
                "\n===== KET THUC CHUONG TRINH =====");

            Console.ReadKey();
        }
    }
}
