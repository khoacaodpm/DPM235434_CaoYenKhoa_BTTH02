using System;
using System.Collections.Generic;

namespace SalesManagement.Behavioral.Visitor
{
    public interface IVisitor
    {
        void Visit(SanPham sanPham);
        void Visit(DonHang donHang);
        void Visit(KhachHang khachHang);
    }

    public interface IElement
    {
        void Accept(IVisitor visitor);
    }

    public class SanPham : IElement
    {
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuongTon { get; set; }

        public SanPham(string maSanPham, string tenSanPham, decimal donGia, int soLuongTon)
        {
            MaSanPham = maSanPham;
            TenSanPham = tenSanPham;
            DonGia = donGia;
            SoLuongTon = soLuongTon;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class DonHang : IElement
    {
        public string MaDonHang { get; set; }
        public string TenKhachHang { get; set; }
        public List<SanPham> DanhSachSanPham { get; set; }
        public decimal GiamGia { get; set; }

        public DonHang(string maDonHang, string tenKhachHang)
        {
            MaDonHang = maDonHang;
            TenKhachHang = tenKhachHang;
            DanhSachSanPham = new List<SanPham>();
            GiamGia = 0;
        }

        public decimal TinhTongTien()
        {
            decimal tongTien = 0;

            foreach (SanPham sanPham in DanhSachSanPham)
            {
                tongTien += sanPham.DonGia;
            }

            return tongTien;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class KhachHang : IElement
    {
        public string MaKhachHang { get; set; }
        public string HoTen { get; set; }
        public string LoaiKhachHang { get; set; }
        public decimal TongChiTieu { get; set; }

        public KhachHang(
            string maKhachHang,
            string hoTen,
            string loaiKhachHang,
            decimal tongChiTieu)
        {
            MaKhachHang = maKhachHang;
            HoTen = hoTen;
            LoaiKhachHang = loaiKhachHang;
            TongChiTieu = tongChiTieu;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class BaoCaoSanPhamVisitor : IVisitor
    {
        public void Visit(SanPham sanPham)
        {
            Console.WriteLine("\n--- BAO CAO SAN PHAM ---");
            Console.WriteLine($"Ma san pham: {sanPham.MaSanPham}");
            Console.WriteLine($"Ten san pham: {sanPham.TenSanPham}");
            Console.WriteLine($"Don gia: {sanPham.DonGia:N0} VND");
            Console.WriteLine($"So luong ton kho: {sanPham.SoLuongTon}");

            if (sanPham.SoLuongTon < 10)
            {
                Console.WriteLine("Canh bao: San pham sap het hang!");
            }
            else
            {
                Console.WriteLine("Tinh trang: Hang ton kho on dinh.");
            }
        }

        public void Visit(DonHang donHang)
        {
            Console.WriteLine("\n--- BAO CAO DON HANG ---");
            Console.WriteLine($"Ma don hang: {donHang.MaDonHang}");
            Console.WriteLine($"Khach hang: {donHang.TenKhachHang}");
            Console.WriteLine($"So san pham: {donHang.DanhSachSanPham.Count}");
            Console.WriteLine($"Tong tien: {donHang.TinhTongTien():N0} VND");
        }

        public void Visit(KhachHang khachHang)
        {
            Console.WriteLine("\n--- BAO CAO KHACH HANG ---");
            Console.WriteLine($"Ma khach hang: {khachHang.MaKhachHang}");
            Console.WriteLine($"Ho ten: {khachHang.HoTen}");
            Console.WriteLine($"Loai khach hang: {khachHang.LoaiKhachHang}");
            Console.WriteLine($"Tong chi tieu: {khachHang.TongChiTieu:N0} VND");
        }
    }

    public class TinhToanKhuyenMaiVisitor : IVisitor
    {
        public void Visit(SanPham sanPham)
        {
            decimal giaSauGiam = sanPham.DonGia;

            if (sanPham.SoLuongTon > 50)
            {
                giaSauGiam = sanPham.DonGia * 0.90m;
            }

            Console.WriteLine($"\nSan pham: {sanPham.TenSanPham}");
            Console.WriteLine($"Gia goc: {sanPham.DonGia:N0} VND");
            Console.WriteLine($"Gia sau khuyen mai: {giaSauGiam:N0} VND");
        }

        public void Visit(DonHang donHang)
        {
            decimal tongTien = donHang.TinhTongTien();
            decimal giamGia = tongTien >= 5000000 ? tongTien * 0.10m : 0;
            decimal thanhTien = tongTien - giamGia;

            donHang.GiamGia = giamGia;

            Console.WriteLine($"\nDon hang: {donHang.MaDonHang}");
            Console.WriteLine($"Tong tien: {tongTien:N0} VND");
            Console.WriteLine($"Giam gia: {giamGia:N0} VND");
            Console.WriteLine($"Thanh tien: {thanhTien:N0} VND");
        }

        public void Visit(KhachHang khachHang)
        {
            decimal tiLeGiamGia = 0;

            switch (khachHang.LoaiKhachHang.ToLower())
            {
                case "vip":
                    tiLeGiamGia = 0.15m;
                    break;

                case "than thiet":
                    tiLeGiamGia = 0.10m;
                    break;

                default:
                    tiLeGiamGia = 0.05m;
                    break;
            }

            Console.WriteLine($"\nKhach hang: {khachHang.HoTen}");
            Console.WriteLine($"Loai khach hang: {khachHang.LoaiKhachHang}");
            Console.WriteLine($"Uu dai: {tiLeGiamGia * 100:N0}%");
        }
    }

    public class KiemKeKhoVisitor : IVisitor
    {
        public void Visit(SanPham sanPham)
        {
            Console.WriteLine($"\nKiem ke: {sanPham.TenSanPham}");

            if (sanPham.SoLuongTon == 0)
            {
                Console.WriteLine("Trang thai: Het hang");
            }
            else if (sanPham.SoLuongTon < 10)
            {
                Console.WriteLine("Trang thai: Sap het hang");
            }
            else
            {
                Console.WriteLine("Trang thai: Du hang");
            }
        }

        public void Visit(DonHang donHang)
        {
            Console.WriteLine($"\nKiem tra don hang: {donHang.MaDonHang}");

            foreach (SanPham sanPham in donHang.DanhSachSanPham)
            {
                if (sanPham.SoLuongTon <= 0)
                {
                    Console.WriteLine($"Khong con hang: {sanPham.TenSanPham}");
                }
                else
                {
                    Console.WriteLine($"Co the cung ung: {sanPham.TenSanPham}");
                }
            }
        }

        public void Visit(KhachHang khachHang)
        {
            Console.WriteLine(
                $"\nKhong can kiem ke kho cho khach hang {khachHang.HoTen}.");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            SanPham sanPham1 = new SanPham("SP001", "Laptop", 15000000, 8);
            SanPham sanPham2 = new SanPham("SP002", "Chuot khong day", 500000, 60);
            SanPham sanPham3 = new SanPham("SP003", "Ban phim", 800000, 25);

            DonHang donHang = new DonHang("DH001", "Nguyen Van An");
            donHang.DanhSachSanPham.Add(sanPham1);
            donHang.DanhSachSanPham.Add(sanPham2);
            donHang.DanhSachSanPham.Add(sanPham3);

            KhachHang khachHang = new KhachHang(
                "KH001",
                "Nguyen Van An",
                "VIP",
                25000000);

            List<IElement> danhSach = new List<IElement>
            {
                sanPham1,
                sanPham2,
                sanPham3,
                donHang,
                khachHang
            };

            while (true)
            {
                Console.WriteLine("\n===== QUAN LY BAN HANG =====");
                Console.WriteLine("1. Xem bao cao");
                Console.WriteLine("2. Tinh khuyen mai");
                Console.WriteLine("3. Kiem ke kho");
                Console.WriteLine("0. Thoat");
                Console.Write("Chon chuc nang: ");

                string luaChon = Console.ReadLine() ?? "";

                if (luaChon == "0")
                {
                    break;
                }

                IVisitor visitor;

                switch (luaChon)
                {
                    case "1":
                        visitor = new BaoCaoSanPhamVisitor();
                        break;

                    case "2":
                        visitor = new TinhToanKhuyenMaiVisitor();
                        break;

                    case "3":
                        visitor = new KiemKeKhoVisitor();
                        break;

                    default:
                        Console.WriteLine("Lua chon khong hop le.");
                        continue;
                }

                foreach (IElement element in danhSach)
                {
                    element.Accept(visitor);
                }

                Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}