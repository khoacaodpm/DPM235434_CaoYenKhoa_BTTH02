using System;
using System.Collections.Generic;

namespace SalesManagement.Behavioral.Mediator
{
    // Mediator interface
    public interface IMediator
    {
        void GuiThongBao(string thongDiep, NhanVien nguoiGui);
        void TaoDonHang(DonHang donHang);
        void XacNhanThanhToan(DonHang donHang);
        void CapNhatTonKho(DonHang donHang);
    }

    // Concrete Mediator
    public class HeThongQuanLyBanHang : IMediator
    {
        private NhanVienBanHang _nhanVienBanHang;
        private NhanVienKho _nhanVienKho;
        private NhanVienKeToan _nhanVienKeToan;
        private NhanVienGiaoHang _nhanVienGiaoHang;

        public void DangKyNhanVien(
            NhanVienBanHang nhanVienBanHang,
            NhanVienKho nhanVienKho,
            NhanVienKeToan nhanVienKeToan,
            NhanVienGiaoHang nhanVienGiaoHang)
        {
            _nhanVienBanHang = nhanVienBanHang;
            _nhanVienKho = nhanVienKho;
            _nhanVienKeToan = nhanVienKeToan;
            _nhanVienGiaoHang = nhanVienGiaoHang;
        }

        public void GuiThongBao(string thongDiep, NhanVien nguoiGui)
        {
            Console.WriteLine(
                $"[THONG BAO] {nguoiGui.TenNhanVien}: {thongDiep}");
        }

        public void TaoDonHang(DonHang donHang)
        {
            Console.WriteLine(
                $"\n[HE THONG] Tiep nhan don hang {donHang.MaDonHang}.");

            _nhanVienKho.KiemTraTonKho(donHang);
        }

        public void XacNhanThanhToan(DonHang donHang)
        {
            donHang.TrangThaiThanhToan = "Da thanh toan";

            GuiThongBao(
                $"Don hang {donHang.MaDonHang} da thanh toan.",
                _nhanVienKeToan);

            _nhanVienKho.XuatHang(donHang);
        }

        public void CapNhatTonKho(DonHang donHang)
        {
            GuiThongBao(
                $"Ton kho da duoc cap nhat cho don hang {donHang.MaDonHang}.",
                _nhanVienKho);

            _nhanVienGiaoHang.TiepNhanDonHang(donHang);
        }

        public void HoanTatGiaoHang(DonHang donHang)
        {
            donHang.TrangThaiDonHang = "Da giao";

            GuiThongBao(
                $"Don hang {donHang.MaDonHang} da giao thanh cong.",
                _nhanVienGiaoHang);

            _nhanVienBanHang.CapNhatTrangThaiDonHang(donHang);
        }
    }

    // Colleague base class
    public abstract class NhanVien
    {
        protected IMediator _mediator;

        public string TenNhanVien { get; set; }

        protected NhanVien(string tenNhanVien, IMediator mediator)
        {
            TenNhanVien = tenNhanVien;
            _mediator = mediator;
        }
    }

    // Concrete Colleague: Sales employee
    public class NhanVienBanHang : NhanVien
    {
        public NhanVienBanHang(
            string tenNhanVien,
            IMediator mediator)
            : base(tenNhanVien, mediator)
        {
        }

        public void TaoDonHang(DonHang donHang)
        {
            Console.WriteLine(
                $"\n[{TenNhanVien}] Tao don hang {donHang.MaDonHang}.");

            donHang.TrangThaiDonHang = "Cho kiem tra kho";
            _mediator.TaoDonHang(donHang);
        }

        public void CapNhatTrangThaiDonHang(DonHang donHang)
        {
            Console.WriteLine(
                $"[{TenNhanVien}] Cap nhat don hang " +
                $"{donHang.MaDonHang}: {donHang.TrangThaiDonHang}.");

            HienThiDonHang(donHang);
        }

        public void HienThiDonHang(DonHang donHang)
        {
            Console.WriteLine("\n===== THONG TIN DON HANG =====");
            Console.WriteLine($"Ma don hang: {donHang.MaDonHang}");
            Console.WriteLine($"Khach hang: {donHang.TenKhachHang}");
            Console.WriteLine($"San pham: {donHang.TenSanPham}");
            Console.WriteLine($"So luong: {donHang.SoLuong}");
            Console.WriteLine($"Tong tien: {donHang.TongTien:N0} VND");
            Console.WriteLine(
                $"Thanh toan: {donHang.TrangThaiThanhToan}");
            Console.WriteLine(
                $"Trang thai don hang: {donHang.TrangThaiDonHang}");
            Console.WriteLine("==============================");
        }
    }

    // Concrete Colleague: Warehouse employee
    public class NhanVienKho : NhanVien
    {
        private readonly Dictionary<string, int> _tonKho =
            new Dictionary<string, int>();

        public NhanVienKho(
            string tenNhanVien,
            IMediator mediator)
            : base(tenNhanVien, mediator)
        {
            _tonKho["Laptop"] = 10;
            _tonKho["Dien thoai"] = 20;
            _tonKho["Tai nghe"] = 30;
        }

        public void HienThiTonKho()
        {
            Console.WriteLine("\n===== TON KHO =====");

            foreach (var sanPham in _tonKho)
            {
                Console.WriteLine(
                    $"{sanPham.Key}: {sanPham.Value} san pham");
            }
        }

        public void KiemTraTonKho(DonHang donHang)
        {
            Console.WriteLine(
                $"[{TenNhanVien}] Kiem tra ton kho " +
                $"san pham {donHang.TenSanPham}.");

            if (!_tonKho.ContainsKey(donHang.TenSanPham))
            {
                Console.WriteLine(
                    "San pham khong ton tai trong he thong kho.");

                donHang.TrangThaiDonHang = "Khong tim thay san pham";
                return;
            }

            if (_tonKho[donHang.TenSanPham] < donHang.SoLuong)
            {
                Console.WriteLine("Khong du hang trong kho.");

                donHang.TrangThaiDonHang = "Het hang";
                return;
            }

            Console.WriteLine("San pham du so luong.");

            if (donHang.TrangThaiThanhToan == "Da thanh toan")
            {
                XuatHang(donHang);
            }
            else
            {
                donHang.TrangThaiDonHang = "Cho thanh toan";

                Console.WriteLine(
                    "Don hang dang cho thanh toan.");

                _mediator.GuiThongBao(
                    $"Don hang {donHang.MaDonHang} cho thanh toan.",
                    this);
            }
        }

        public void XuatHang(DonHang donHang)
        {
            if (!_tonKho.ContainsKey(donHang.TenSanPham) ||
                _tonKho[donHang.TenSanPham] < donHang.SoLuong)
            {
                Console.WriteLine("Khong the xuat hang do khong du ton kho.");
                donHang.TrangThaiDonHang = "Khong du hang";
                return;
            }

            _tonKho[donHang.TenSanPham] -= donHang.SoLuong;
            donHang.TrangThaiDonHang = "Dang chuan bi hang";

            Console.WriteLine(
                $"[{TenNhanVien}] Xuat {donHang.SoLuong} " +
                $"{donHang.TenSanPham} cho don {donHang.MaDonHang}.");

            _mediator.CapNhatTonKho(donHang);
        }
    }

    // Concrete Colleague: Accountant
    public class NhanVienKeToan : NhanVien
    {
        public NhanVienKeToan(
            string tenNhanVien,
            IMediator mediator)
            : base(tenNhanVien, mediator)
        {
        }

        public void ThanhToanDonHang(DonHang donHang)
        {
            if (donHang.TrangThaiDonHang == "Het hang" ||
                donHang.TrangThaiDonHang == "Khong tim thay san pham")
            {
                Console.WriteLine(
                    "Khong the thanh toan do don hang khong hop le.");
                return;
            }

            if (donHang.TrangThaiThanhToan == "Da thanh toan")
            {
                Console.WriteLine("Don hang da duoc thanh toan.");
                return;
            }

            Console.WriteLine(
                $"[{TenNhanVien}] Xac nhan thanh toan " +
                $"{donHang.TongTien:N0} VND.");

            _mediator.XacNhanThanhToan(donHang);
        }
    }

    // Concrete Colleague: Delivery employee
    public class NhanVienGiaoHang : NhanVien
    {
        public NhanVienGiaoHang(
            string tenNhanVien,
            IMediator mediator)
            : base(tenNhanVien, mediator)
        {
        }

        public void TiepNhanDonHang(DonHang donHang)
        {
            Console.WriteLine(
                $"[{TenNhanVien}] Tiep nhan don hang " +
                $"{donHang.MaDonHang} de giao.");

            donHang.TrangThaiDonHang = "Dang giao";
        }

        public void XacNhanGiaoHang(DonHang donHang)
        {
            if (donHang.TrangThaiDonHang != "Dang giao")
            {
                Console.WriteLine(
                    "Don hang chua san sang de xac nhan giao hang.");
                return;
            }

            Console.WriteLine(
                $"[{TenNhanVien}] Xac nhan giao hang " +
                $"{donHang.MaDonHang} thanh cong.");

            if (_mediator is HeThongQuanLyBanHang heThong)
            {
                heThong.HoanTatGiaoHang(donHang);
            }
        }
    }

    // Order entity
    public class DonHang
    {
        public string MaDonHang { get; set; }
        public string TenKhachHang { get; set; }
        public string TenSanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal TongTien => SoLuong * DonGia;

        public string TrangThaiThanhToan { get; set; }
        public string TrangThaiDonHang { get; set; }

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
            TrangThaiThanhToan = "Chua thanh toan";
            TrangThaiDonHang = "Moi tao";
        }
    }

    // Client
    internal class Program
    {
        static void Main(string[] args)
        {
            HeThongQuanLyBanHang heThong = new HeThongQuanLyBanHang();

            NhanVienBanHang banHang =
                new NhanVienBanHang("Nguyen Van An", heThong);

            NhanVienKho kho =
                new NhanVienKho("Tran Van Binh", heThong);

            NhanVienKeToan keToan =
                new NhanVienKeToan("Le Thi Hoa", heThong);

            NhanVienGiaoHang giaoHang =
                new NhanVienGiaoHang("Pham Van Nam", heThong);

            heThong.DangKyNhanVien(
                banHang,
                kho,
                keToan,
                giaoHang);

            List<DonHang> danhSachDonHang = new List<DonHang>();

            while (true)
            {
                Console.WriteLine("\n===== HE THONG QUAN LY BAN HANG =====");
                Console.WriteLine("1. Tao don hang");
                Console.WriteLine("2. Hien thi danh sach don hang");
                Console.WriteLine("3. Thanh toan don hang");
                Console.WriteLine("4. Xac nhan giao hang");
                Console.WriteLine("5. Hien thi ton kho");
                Console.WriteLine("0. Thoat");
                Console.Write("Chon chuc nang: ");

                string luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1":
                        {
                            Console.Write("Nhap ma don hang: ");
                            string maDonHang = Console.ReadLine();

                            if (danhSachDonHang.Exists(
                                d => d.MaDonHang == maDonHang))
                            {
                                Console.WriteLine("Ma don hang da ton tai.");
                                break;
                            }

                            Console.Write("Nhap ten khach hang: ");
                            string tenKhachHang = Console.ReadLine();

                            Console.WriteLine("Chon san pham:");
                            Console.WriteLine("1. Laptop - 15000000 VND");
                            Console.WriteLine("2. Dien thoai - 8000000 VND");
                            Console.WriteLine("3. Tai nghe - 500000 VND");
                            Console.Write("Lua chon: ");

                            string chonSanPham = Console.ReadLine();

                            string tenSanPham = "";
                            decimal donGia = 0;

                            switch (chonSanPham)
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
                                    break;
                            }

                            if (chonSanPham != "1" &&
                                chonSanPham != "2" &&
                                chonSanPham != "3")
                            {
                                break;
                            }

                            Console.Write("Nhap so luong: ");

                            if (!int.TryParse(
                                Console.ReadLine(), out int soLuong) ||
                                soLuong <= 0)
                            {
                                Console.WriteLine("So luong khong hop le.");
                                break;
                            }

                            DonHang donHang = new DonHang(
                                maDonHang,
                                tenKhachHang,
                                tenSanPham,
                                soLuong,
                                donGia);

                            danhSachDonHang.Add(donHang);
                            banHang.TaoDonHang(donHang);
                            break;
                        }

                    case "2":
                        {
                            if (danhSachDonHang.Count == 0)
                            {
                                Console.WriteLine("Chua co don hang nao.");
                                break;
                            }

                            foreach (DonHang donHang in danhSachDonHang)
                            {
                                banHang.HienThiDonHang(donHang);
                            }

                            break;
                        }

                    case "3":
                        {
                            DonHang donHang = ChonDonHang(danhSachDonHang);

                            if (donHang == null)
                                break;

                            keToan.ThanhToanDonHang(donHang);
                            break;
                        }

                    case "4":
                        {
                            DonHang donHang = ChonDonHang(danhSachDonHang);

                            if (donHang == null)
                                break;

                            giaoHang.XacNhanGiaoHang(donHang);
                            break;
                        }

                    case "5":
                        kho.HienThiTonKho();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Lua chon khong hop le.");
                        break;
                }
            }
        }

        static DonHang ChonDonHang(List<DonHang> danhSachDonHang)
        {
            if (danhSachDonHang.Count == 0)
            {
                Console.WriteLine("Chua co don hang nao.");
                return null;
            }

            Console.Write("Nhap ma don hang: ");
            string maDonHang = Console.ReadLine();

            DonHang donHang = danhSachDonHang.Find(
                d => d.MaDonHang == maDonHang);

            if (donHang == null)
            {
                Console.WriteLine("Khong tim thay don hang.");
            }

            return donHang;
        }
    }
}