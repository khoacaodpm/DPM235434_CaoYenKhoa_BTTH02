using System;
using System.Collections.Generic;

namespace SalesManagement.Behavioral.TemplateMethod
{
    public class SanPham
    {
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }

        public SanPham(string maSanPham, string tenSanPham, decimal donGia, int soLuong)
        {
            MaSanPham = maSanPham;
            TenSanPham = tenSanPham;
            DonGia = donGia;
            SoLuong = soLuong;
        }

        public decimal ThanhTien()
        {
            return DonGia * SoLuong;
        }
    }

    public class DonHang
    {
        public string MaDonHang { get; set; }
        public string TenKhachHang { get; set; }
        public List<SanPham> DanhSachSanPham { get; set; }

        public DonHang(string maDonHang, string tenKhachHang)
        {
            MaDonHang = maDonHang;
            TenKhachHang = tenKhachHang;
            DanhSachSanPham = new List<SanPham>();
        }

        public decimal TinhTongTien()
        {
            decimal tongTien = 0;

            foreach (SanPham sanPham in DanhSachSanPham)
            {
                tongTien += sanPham.ThanhTien();
            }

            return tongTien;
        }
    }

    public abstract class QuyTrinhThanhToan
    {
        public void ThucHienThanhToan(DonHang donHang)
        {
            Console.WriteLine("\n===== QUY TRINH THANH TOAN =====");

            KiemTraDonHang(donHang);
            HienThiThongTinDonHang(donHang);
            decimal tongTien = TinhSoTienThanhToan(donHang);
            ThucHienThanhToanCuThe(tongTien);
            CapNhatTrangThaiDonHang(donHang);
            GuiThongBao(donHang, tongTien);

            Console.WriteLine("Thanh toan hoan tat!");
        }

        protected virtual void KiemTraDonHang(DonHang donHang)
        {
            if (donHang == null)
                throw new ArgumentNullException(nameof(donHang));

            if (donHang.DanhSachSanPham.Count == 0)
                throw new InvalidOperationException("Don hang khong co san pham.");

            foreach (SanPham sanPham in donHang.DanhSachSanPham)
            {
                if (sanPham.SoLuong <= 0 || sanPham.DonGia < 0)
                    throw new InvalidOperationException("Thong tin san pham khong hop le.");
            }

            Console.WriteLine("Kiem tra don hang thanh cong.");
        }

        protected virtual void HienThiThongTinDonHang(DonHang donHang)
        {
            Console.WriteLine($"Ma don hang: {donHang.MaDonHang}");
            Console.WriteLine($"Khach hang: {donHang.TenKhachHang}");

            foreach (SanPham sanPham in donHang.DanhSachSanPham)
            {
                Console.WriteLine(
                    $"{sanPham.TenSanPham} - So luong: {sanPham.SoLuong} - " +
                    $"Don gia: {sanPham.DonGia:N0} - Thanh tien: {sanPham.ThanhTien():N0} VND");
            }
        }

        protected abstract decimal TinhSoTienThanhToan(DonHang donHang);

        protected abstract void ThucHienThanhToanCuThe(decimal soTien);

        protected virtual void CapNhatTrangThaiDonHang(DonHang donHang)
        {
            Console.WriteLine($"Don hang {donHang.MaDonHang} da duoc cap nhat trang thai thanh toan.");
        }

        protected virtual void GuiThongBao(DonHang donHang, decimal soTien)
        {
            Console.WriteLine(
                $"Thong bao: Khach hang {donHang.TenKhachHang} da thanh toan {soTien:N0} VND.");
        }
    }

    public class ThanhToanTienMat : QuyTrinhThanhToan
    {
        protected override decimal TinhSoTienThanhToan(DonHang donHang)
        {
            return donHang.TinhTongTien();
        }

        protected override void ThucHienThanhToanCuThe(decimal soTien)
        {
            Console.WriteLine($"Thanh toan bang tien mat: {soTien:N0} VND");
        }
    }

    public class ThanhToanThe : QuyTrinhThanhToan
    {
        protected override decimal TinhSoTienThanhToan(DonHang donHang)
        {
            return donHang.TinhTongTien() * 1.02m;
        }

        protected override void ThucHienThanhToanCuThe(decimal soTien)
        {
            Console.WriteLine($"Thanh toan bang the ngan hang: {soTien:N0} VND");
            Console.WriteLine("Phi giao dich: 2%");
        }
    }

    public class ThanhToanViDienTu : QuyTrinhThanhToan
    {
        protected override decimal TinhSoTienThanhToan(DonHang donHang)
        {
            return donHang.TinhTongTien() * 0.95m;
        }

        protected override void ThucHienThanhToanCuThe(decimal soTien)
        {
            Console.WriteLine($"Thanh toan bang vi dien tu: {soTien:N0} VND");
            Console.WriteLine("Da ap dung giam gia 5%.");
        }

        protected override void GuiThongBao(DonHang donHang, decimal soTien)
        {
            Console.WriteLine(
                $"Vi dien tu da thanh toan {soTien:N0} VND cho don hang {donHang.MaDonHang}.");
            Console.WriteLine("Email xac nhan da duoc gui den khach hang.");
        }
    }

    public class ThanhToanChuyenKhoan : QuyTrinhThanhToan
    {
        protected override decimal TinhSoTienThanhToan(DonHang donHang)
        {
            return donHang.TinhTongTien();
        }

        protected override void ThucHienThanhToanCuThe(decimal soTien)
        {
            Console.WriteLine($"Chuyen khoan ngan hang: {soTien:N0} VND");
            Console.WriteLine("Vui long su dung ma don hang lam noi dung chuyen khoan.");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            DonHang donHang = new DonHang("DH001", "Nguyen Van An");

            donHang.DanhSachSanPham.Add(
                new SanPham("SP001", "Laptop", 15000000, 1));

            donHang.DanhSachSanPham.Add(
                new SanPham("SP002", "Chuot khong day", 500000, 2));

            Console.WriteLine("===== HE THONG QUAN LY BAN HANG =====");
            Console.WriteLine("1. Thanh toan tien mat");
            Console.WriteLine("2. Thanh toan the ngan hang");
            Console.WriteLine("3. Thanh toan vi dien tu");
            Console.WriteLine("4. Thanh toan chuyen khoan");
            Console.Write("Chon phuong thuc thanh toan: ");

            if (!int.TryParse(Console.ReadLine(), out int luaChon))
            {
                Console.WriteLine("Lua chon khong hop le.");
                return;
            }

            QuyTrinhThanhToan quyTrinh;

            switch (luaChon)
            {
                case 1:
                    quyTrinh = new ThanhToanTienMat();
                    break;

                case 2:
                    quyTrinh = new ThanhToanThe();
                    break;

                case 3:
                    quyTrinh = new ThanhToanViDienTu();
                    break;

                case 4:
                    quyTrinh = new ThanhToanChuyenKhoan();
                    break;

                default:
                    Console.WriteLine("Phuong thuc thanh toan khong hop le.");
                    return;
            }

            try
            {
                quyTrinh.ThucHienThanhToan(donHang);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Loi: {ex.Message}");
            }

            Console.WriteLine("\nNhan phim bat ky de thoat...");
            Console.ReadKey();
        }
    }
}