
using System;
using System.Collections.Generic;

namespace SalesManagement.Behavioral.ChainOfResponsibility
{
    // 1. Handler: Giao dien xu ly yeu cau don hang
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);
        object Handle(object request);
    }

    // 2. AbstractHandler: Lop xu ly co so
    public abstract class AbstractHandler : IHandler
    {
        private IHandler _nextHandler;

        public IHandler SetNext(IHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual object Handle(object request)
        {
            if (_nextHandler != null)
                return _nextHandler.Handle(request);

            return "Don hang da di qua tat ca cac buoc xu ly.";
        }
    }

    // 3. Du lieu don hang
    public class DonHang
    {
        public string MaDonHang { get; set; }
        public string TenSanPham { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }
        public bool DaThanhToan { get; set; }

        public DonHang(
            string maDonHang,
            string tenSanPham,
            double donGia,
            int soLuong,
            bool daThanhToan)
        {
            MaDonHang = maDonHang;
            TenSanPham = tenSanPham;
            DonGia = donGia;
            SoLuong = soLuong;
            DaThanhToan = daThanhToan;
        }

        public double TongTien
        {
            get { return DonGia * SoLuong; }
        }
    }

    // 4. Handler A: Kiem tra thong tin don hang
    public class KiemTraDonHangHandler : AbstractHandler
    {
        public override object Handle(object request)
        {
            DonHang donHang = request as DonHang;

            if (donHang == null ||
                string.IsNullOrWhiteSpace(donHang.MaDonHang) ||
                string.IsNullOrWhiteSpace(donHang.TenSanPham) ||
                donHang.DonGia <= 0 ||
                donHang.SoLuong <= 0)
            {
                return "Tu choi don hang: Thong tin khong hop le.";
            }

            Console.WriteLine(
                $"[{donHang.MaDonHang}] Kiem tra thong tin thanh cong.");

            return base.Handle(request);
        }
    }

    // 5. Handler B: Kiem tra thanh toan
    public class KiemTraThanhToanHandler : AbstractHandler
    {
        public override object Handle(object request)
        {
            DonHang donHang = request as DonHang;

            if (donHang == null)
                return "Tu choi don hang: Du lieu khong hop le.";

            if (!donHang.DaThanhToan)
            {
                return $"Tu choi don hang {donHang.MaDonHang}: Chua thanh toan.";
            }

            Console.WriteLine(
                $"[{donHang.MaDonHang}] Kiem tra thanh toan thanh cong.");

            return base.Handle(request);
        }
    }

    // 6. Handler C: Kiem tra ton kho
    public class KiemTraTonKhoHandler : AbstractHandler
    {
        private Dictionary<string, int> _tonKho;

        public KiemTraTonKhoHandler(Dictionary<string, int> tonKho)
        {
            _tonKho = tonKho;
        }

        public override object Handle(object request)
        {
            DonHang donHang = request as DonHang;

            if (donHang == null)
                return "Tu choi don hang: Du lieu khong hop le.";

            if (!_tonKho.ContainsKey(donHang.TenSanPham))
            {
                return $"Tu choi don hang {donHang.MaDonHang}: San pham khong ton tai trong kho.";
            }

            if (_tonKho[donHang.TenSanPham] < donHang.SoLuong)
            {
                return $"Tu choi don hang {donHang.MaDonHang}: Khong du hang trong kho.";
            }

            _tonKho[donHang.TenSanPham] -= donHang.SoLuong;

            Console.WriteLine(
                $"[{donHang.MaDonHang}] Kiem tra ton kho thanh cong.");

            return base.Handle(request);
        }
    }

    // 7. Handler D: Phe duyet don hang gia tri cao
    public class PheDuyetDonHangHandler : AbstractHandler
    {
        private double _nguongPheDuyet;

        public PheDuyetDonHangHandler(double nguongPheDuyet)
        {
            _nguongPheDuyet = nguongPheDuyet;
        }

        public override object Handle(object request)
        {
            DonHang donHang = request as DonHang;

            if (donHang == null)
                return "Tu choi don hang: Du lieu khong hop le.";

            if (donHang.TongTien >= _nguongPheDuyet)
            {
                Console.WriteLine(
                    $"[{donHang.MaDonHang}] Don hang gia tri cao, can phe duyet.");

                Console.WriteLine(
                    $"[{donHang.MaDonHang}] Phe duyet don hang thanh cong.");
            }
            else
            {
                Console.WriteLine(
                    $"[{donHang.MaDonHang}] Khong can phe duyet dac biet.");
            }

            return base.Handle(request);
        }
    }

    // 8. Handler E: Xac nhan va giao hang
    public class XacNhanDonHangHandler : AbstractHandler
    {
        public override object Handle(object request)
        {
            DonHang donHang = request as DonHang;

            if (donHang == null)
                return "Tu choi don hang: Du lieu khong hop le.";

            Console.WriteLine(
                $"[{donHang.MaDonHang}] Xac nhan don hang thanh cong.");

            Console.WriteLine(
                $"[{donHang.MaDonHang}] Dang chuan bi giao hang.");

            return $"Don hang {donHang.MaDonHang} da duoc xu ly thanh cong.";
        }
    }

    // 9. Client: He thong quan ly ban hang
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, int> tonKho =
                new Dictionary<string, int>
                {
                    { "Laptop Dell", 5 },
                    { "Chuot Logitech", 10 },
                    { "Ban phim co", 8 }
                };

            var kiemTraDonHang = new KiemTraDonHangHandler();
            var kiemTraThanhToan = new KiemTraThanhToanHandler();
            var kiemTraTonKho = new KiemTraTonKhoHandler(tonKho);
            var pheDuyetDonHang = new PheDuyetDonHangHandler(10000000);
            var xacNhanDonHang = new XacNhanDonHangHandler();

            kiemTraDonHang
                .SetNext(kiemTraThanhToan)
                .SetNext(kiemTraTonKho)
                .SetNext(pheDuyetDonHang)
                .SetNext(xacNhanDonHang);

            List<DonHang> danhSachDonHang = new List<DonHang>
            {
                new DonHang(
                    "DH001", "Chuot Logitech", 500000, 2, true),

                new DonHang(
                    "DH002", "Laptop Dell", 18000000, 1, true),

                new DonHang(
                    "DH003", "Ban phim co", 1200000, 20, true),

                new DonHang(
                    "DH004", "Laptop Dell", 18000000, 1, false),

                new DonHang(
                    "DH005", "Laptop Dell", 18000000, 0, true)
            };

            Console.WriteLine("===== HE THONG QUAN LY BAN HANG =====");
            Console.WriteLine(
                "Chuoi xu ly: Kiem tra don hang > Thanh toan > Ton kho > Phe duyet > Xac nhan");

            foreach (DonHang donHang in danhSachDonHang)
            {
                Console.WriteLine("\n------------------------------------");
                Console.WriteLine($"Ma don hang: {donHang.MaDonHang}");
                Console.WriteLine($"San pham: {donHang.TenSanPham}");
                Console.WriteLine($"Tong tien: {donHang.TongTien:N0} VND");

                object ketQua = kiemTraDonHang.Handle(donHang);

                Console.WriteLine(ketQua);
            }

            Console.WriteLine("\n===== TON KHO CON LAI =====");

            foreach (var item in tonKho)
            {
                Console.WriteLine(
                    $"{item.Key}: {item.Value} san pham");
            }

            Console.WriteLine("\nNhan phim bat ky de ket thuc...");
            Console.ReadKey();
        }
    }
}
