using System;
using System.Collections.Generic;

namespace SalesManagement.Behavioral.Iterator
{
    // Aggregate interface
    public interface ICollectionDonHang
    {
        IIteratorDonHang CreateIterator();
    }

    // Iterator interface
    public interface IIteratorDonHang
    {
        bool HasNext();
        DonHang Next();
        void Reset();
    }

    // Entity
    public class DonHang
    {
        public string MaDonHang { get; set; }
        public string TenKhachHang { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }

        public DonHang(
            string maDonHang,
            string tenKhachHang,
            decimal tongTien,
            string trangThai)
        {
            MaDonHang = maDonHang;
            TenKhachHang = tenKhachHang;
            TongTien = tongTien;
            TrangThai = trangThai;
        }

        public void HienThiThongTin()
        {
            Console.WriteLine($"Ma don hang: {MaDonHang}");
            Console.WriteLine($"Khach hang: {TenKhachHang}");
            Console.WriteLine($"Tong tien: {TongTien:N0} VND");
            Console.WriteLine($"Trang thai: {TrangThai}");
            Console.WriteLine("--------------------------------");
        }
    }

    // Concrete Aggregate
    public class DanhSachDonHang : ICollectionDonHang
    {
        private readonly List<DonHang> _donHangs = new List<DonHang>();

        public void ThemDonHang(DonHang donHang)
        {
            _donHangs.Add(donHang);
        }

        public int SoLuong
        {
            get { return _donHangs.Count; }
        }

        public DonHang LayDonHang(int index)
        {
            if (index < 0 || index >= _donHangs.Count)
                return null;

            return _donHangs[index];
        }

        public IIteratorDonHang CreateIterator()
        {
            return new DonHangIterator(this);
        }
    }

    // Concrete Iterator
    public class DonHangIterator : IIteratorDonHang
    {
        private readonly DanhSachDonHang _danhSach;
        private int _viTriHienTai;

        public DonHangIterator(DanhSachDonHang danhSach)
        {
            _danhSach = danhSach;
            _viTriHienTai = 0;
        }

        public bool HasNext()
        {
            return _viTriHienTai < _danhSach.SoLuong;
        }

        public DonHang Next()
        {
            if (!HasNext())
                return null;

            DonHang donHang = _danhSach.LayDonHang(_viTriHienTai);
            _viTriHienTai++;
            return donHang;
        }

        public void Reset()
        {
            _viTriHienTai = 0;
        }
    }

    // Iterator for filtering orders by status
    public class DonHangTheoTrangThaiIterator : IIteratorDonHang
    {
        private readonly DanhSachDonHang _danhSach;
        private readonly string _trangThai;
        private int _viTriHienTai;

        public DonHangTheoTrangThaiIterator(
            DanhSachDonHang danhSach,
            string trangThai)
        {
            _danhSach = danhSach;
            _trangThai = trangThai;
            _viTriHienTai = 0;
        }

        public bool HasNext()
        {
            for (int i = _viTriHienTai; i < _danhSach.SoLuong; i++)
            {
                DonHang donHang = _danhSach.LayDonHang(i);

                if (donHang != null && donHang.TrangThai == _trangThai)
                    return true;
            }

            return false;
        }

        public DonHang Next()
        {
            while (_viTriHienTai < _danhSach.SoLuong)
            {
                DonHang donHang = _danhSach.LayDonHang(_viTriHienTai);
                _viTriHienTai++;

                if (donHang != null && donHang.TrangThai == _trangThai)
                    return donHang;
            }

            return null;
        }

        public void Reset()
        {
            _viTriHienTai = 0;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            DanhSachDonHang danhSach = new DanhSachDonHang();

            danhSach.ThemDonHang(
                new DonHang("DH001", "Nguyen Van An", 1500000, "Cho xu ly"));

            danhSach.ThemDonHang(
                new DonHang("DH002", "Tran Thi Binh", 2500000, "Da thanh toan"));

            danhSach.ThemDonHang(
                new DonHang("DH003", "Le Van Cuong", 3200000, "Cho xu ly"));

            danhSach.ThemDonHang(
                new DonHang("DH004", "Pham Thi Dung", 1800000, "Da giao"));

            danhSach.ThemDonHang(
                new DonHang("DH005", "Hoang Van Em", 4200000, "Da thanh toan"));

            while (true)
            {
                Console.WriteLine("\n===== QUAN LY BAN HANG =====");
                Console.WriteLine("1. Hien thi tat ca don hang");
                Console.WriteLine("2. Hien thi don hang theo trang thai");
                Console.WriteLine("0. Thoat");
                Console.Write("Chon chuc nang: ");

                string luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1":
                        HienThiTatCaDonHang(danhSach);
                        break;

                    case "2":
                        HienThiDonHangTheoTrangThai(danhSach);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Lua chon khong hop le.");
                        break;
                }
            }
        }

        static void HienThiTatCaDonHang(DanhSachDonHang danhSach)
        {
            Console.WriteLine("\n===== DANH SACH TAT CA DON HANG =====");

            IIteratorDonHang iterator = danhSach.CreateIterator();

            while (iterator.HasNext())
            {
                DonHang donHang = iterator.Next();
                donHang?.HienThiThongTin();
            }
        }

        static void HienThiDonHangTheoTrangThai(
            DanhSachDonHang danhSach)
        {
            Console.WriteLine("\nChon trang thai don hang:");
            Console.WriteLine("1. Cho xu ly");
            Console.WriteLine("2. Da thanh toan");
            Console.WriteLine("3. Da giao");
            Console.Write("Lua chon: ");

            string luaChon = Console.ReadLine();
            string trangThai;

            switch (luaChon)
            {
                case "1":
                    trangThai = "Cho xu ly";
                    break;

                case "2":
                    trangThai = "Da thanh toan";
                    break;

                case "3":
                    trangThai = "Da giao";
                    break;

                default:
                    Console.WriteLine("Trang thai khong hop le.");
                    return;
            }

            Console.WriteLine(
                $"\n===== DON HANG: {trangThai.ToUpper()} =====");

            IIteratorDonHang iterator =
                new DonHangTheoTrangThaiIterator(danhSach, trangThai);

            bool timThay = false;

            while (iterator.HasNext())
            {
                DonHang donHang = iterator.Next();

                if (donHang != null)
                {
                    donHang.HienThiThongTin();
                    timThay = true;
                }
            }

            if (!timThay)
            {
                Console.WriteLine("Khong tim thay don hang phu hop.");
            }
        }
    }
}