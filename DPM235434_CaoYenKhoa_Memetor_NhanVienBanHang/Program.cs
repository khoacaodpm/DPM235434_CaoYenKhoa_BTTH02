using System;
using System.Collections.Generic;

namespace SalesManagement.Behavioral.Memento
{
    // Memento: Luu trang thai don hang
    public class DonHangMemento
    {
        public string MaDonHang { get; }
        public string TenKhachHang { get; }
        public decimal TongTien { get; }
        public string TrangThai { get; }
        public string DiaChiGiaoHang { get; }

        public DonHangMemento(
            string maDonHang,
            string tenKhachHang,
            decimal tongTien,
            string trangThai,
            string diaChiGiaoHang)
        {
            MaDonHang = maDonHang;
            TenKhachHang = tenKhachHang;
            TongTien = tongTien;
            TrangThai = trangThai;
            DiaChiGiaoHang = diaChiGiaoHang;
        }
    }

    // Originator: Don hang
    public class DonHang
    {
        public string MaDonHang { get; private set; }
        public string TenKhachHang { get; private set; }
        public decimal TongTien { get; private set; }
        public string TrangThai { get; private set; }
        public string DiaChiGiaoHang { get; private set; }

        public DonHang(
            string maDonHang,
            string tenKhachHang,
            decimal tongTien,
            string diaChiGiaoHang)
        {
            MaDonHang = maDonHang;
            TenKhachHang = tenKhachHang;
            TongTien = tongTien;
            TrangThai = "Moi tao";
            DiaChiGiaoHang = diaChiGiaoHang;
        }

        public DonHangMemento LuuTrangThai()
        {
            return new DonHangMemento(
                MaDonHang,
                TenKhachHang,
                TongTien,
                TrangThai,
                DiaChiGiaoHang);
        }

        public void KhoiPhucTrangThai(DonHangMemento memento)
        {
            MaDonHang = memento.MaDonHang;
            TenKhachHang = memento.TenKhachHang;
            TongTien = memento.TongTien;
            TrangThai = memento.TrangThai;
            DiaChiGiaoHang = memento.DiaChiGiaoHang;

            Console.WriteLine(
                $"Da khoi phuc trang thai don hang {MaDonHang}.");
        }

        public void CapNhatThongTin(
            string tenKhachHang,
            decimal tongTien,
            string diaChiGiaoHang)
        {
            TenKhachHang = tenKhachHang;
            TongTien = tongTien;
            DiaChiGiaoHang = diaChiGiaoHang;

            Console.WriteLine("Da cap nhat thong tin don hang.");
        }

        public void CapNhatTrangThai(string trangThai)
        {
            TrangThai = trangThai;

            Console.WriteLine(
                $"Trang thai don hang {MaDonHang}: {TrangThai}.");
        }

        public void HienThiThongTin()
        {
            Console.WriteLine("\n===== THONG TIN DON HANG =====");
            Console.WriteLine($"Ma don hang: {MaDonHang}");
            Console.WriteLine($"Khach hang: {TenKhachHang}");
            Console.WriteLine($"Tong tien: {TongTien:N0} VND");
            Console.WriteLine($"Trang thai: {TrangThai}");
            Console.WriteLine($"Dia chi giao hang: {DiaChiGiaoHang}");
            Console.WriteLine("==============================");
        }
    }

    // Caretaker: Quan ly lich su trang thai
    public class LichSuDonHang
    {
        private readonly Stack<DonHangMemento> _lichSu =
            new Stack<DonHangMemento>();

        public void Luu(DonHang donHang)
        {
            _lichSu.Push(donHang.LuuTrangThai());
            Console.WriteLine("Da luu trang thai hien tai cua don hang.");
        }

        public void HoanTac(DonHang donHang)
        {
            if (_lichSu.Count == 0)
            {
                Console.WriteLine("Khong co lich su de khoi phuc.");
                return;
            }

            DonHangMemento memento = _lichSu.Pop();
            donHang.KhoiPhucTrangThai(memento);
        }

        public void HienThiSoLuongLichSu()
        {
            Console.WriteLine(
                $"So trang thai da luu: {_lichSu.Count}");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            DonHang donHang = new DonHang(
                "DH001",
                "Nguyen Van An",
                2500000,
                "123 Nguyen Trai, Ha Noi");

            LichSuDonHang lichSu = new LichSuDonHang();

            while (true)
            {
                Console.WriteLine("\n===== QUAN LY BAN HANG =====");
                Console.WriteLine("1. Xem thong tin don hang");
                Console.WriteLine("2. Cap nhat thong tin don hang");
                Console.WriteLine("3. Cap nhat trang thai don hang");
                Console.WriteLine("4. Luu trang thai don hang");
                Console.WriteLine("5. Khoi phuc trang thai truoc do");
                Console.WriteLine("6. Xem so luong lich su");
                Console.WriteLine("0. Thoat");
                Console.Write("Chon chuc nang: ");

                string luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1":
                        donHang.HienThiThongTin();
                        break;

                    case "2":
                        CapNhatThongTinDonHang(donHang);
                        break;

                    case "3":
                        CapNhatTrangThaiDonHang(donHang);
                        break;

                    case "4":
                        lichSu.Luu(donHang);
                        break;

                    case "5":
                        lichSu.HoanTac(donHang);
                        break;

                    case "6":
                        lichSu.HienThiSoLuongLichSu();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Lua chon khong hop le.");
                        break;
                }
            }
        }

        static void CapNhatThongTinDonHang(DonHang donHang)
        {
            Console.Write("Nhap ten khach hang moi: ");
            string tenKhachHang = Console.ReadLine();

            Console.Write("Nhap tong tien moi: ");

            if (!decimal.TryParse(
                Console.ReadLine(), out decimal tongTien) ||
                tongTien < 0)
            {
                Console.WriteLine("Tong tien khong hop le.");
                return;
            }

            Console.Write("Nhap dia chi giao hang moi: ");
            string diaChi = Console.ReadLine();

            donHang.CapNhatThongTin(
                tenKhachHang,
                tongTien,
                diaChi);
        }

        static void CapNhatTrangThaiDonHang(DonHang donHang)
        {
            Console.WriteLine("\nChon trang thai moi:");
            Console.WriteLine("1. Moi tao");
            Console.WriteLine("2. Cho thanh toan");
            Console.WriteLine("3. Da thanh toan");
            Console.WriteLine("4. Dang giao");
            Console.WriteLine("5. Da giao");
            Console.WriteLine("6. Da huy");
            Console.Write("Lua chon: ");

            string luaChon = Console.ReadLine();
            string trangThai;

            switch (luaChon)
            {
                case "1":
                    trangThai = "Moi tao";
                    break;

                case "2":
                    trangThai = "Cho thanh toan";
                    break;

                case "3":
                    trangThai = "Da thanh toan";
                    break;

                case "4":
                    trangThai = "Dang giao";
                    break;

                case "5":
                    trangThai = "Da giao";
                    break;

                case "6":
                    trangThai = "Da huy";
                    break;

                default:
                    Console.WriteLine("Trang thai khong hop le.");
                    return;
            }

            donHang.CapNhatTrangThai(trangThai);
        }
    }
}