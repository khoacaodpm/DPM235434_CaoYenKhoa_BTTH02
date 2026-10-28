
using System;

namespace SalesManagement.DecoratorPattern
{
    // 1. Component: Giao dien chung cho don hang
    public interface IDonHang
    {
        string MoTa();
        double TinhTongTien();
    }

    // 2. Concrete Component: Don hang co ban
    public class DonHang : IDonHang
    {
        private string _tenSanPham;
        private double _donGia;
        private int _soLuong;

        public DonHang(string tenSanPham, double donGia, int soLuong)
        {
            _tenSanPham = tenSanPham;
            _donGia = donGia;
            _soLuong = soLuong;
        }

        public string MoTa()
        {
            return $"{_tenSanPham} x {_soLuong}";
        }

        public double TinhTongTien()
        {
            return _donGia * _soLuong;
        }
    }

    // 3. Decorator: Lop trang tri co so
    public abstract class DonHangDecorator : IDonHang
    {
        protected IDonHang _donHang;

        public DonHangDecorator(IDonHang donHang)
        {
            _donHang = donHang;
        }

        public virtual string MoTa()
        {
            return _donHang.MoTa();
        }

        public virtual double TinhTongTien()
        {
            return _donHang.TinhTongTien();
        }
    }

    // 4. Concrete Decorator: Giam gia khach hang VIP
    public class GiamGiaVIP : DonHangDecorator
    {
        private double _tyLeGiam;

        public GiamGiaVIP(IDonHang donHang, double tyLeGiam)
            : base(donHang)
        {
            _tyLeGiam = tyLeGiam;
        }

        public override string MoTa()
        {
            return base.MoTa() + " + Giam gia VIP";
        }

        public override double TinhTongTien()
        {
            return base.TinhTongTien() * (1 - _tyLeGiam);
        }
    }

    // 5. Concrete Decorator: Ma giam gia
    public class MaGiamGia : DonHangDecorator
    {
        private double _soTienGiam;

        public MaGiamGia(IDonHang donHang, double soTienGiam)
            : base(donHang)
        {
            _soTienGiam = soTienGiam;
        }

        public override string MoTa()
        {
            return base.MoTa() + " + Ma giam gia";
        }

        public override double TinhTongTien()
        {
            return Math.Max(0, base.TinhTongTien() - _soTienGiam);
        }
    }

    // 6. Concrete Decorator: Phi giao hang
    public class PhiGiaoHang : DonHangDecorator
    {
        private double _phiGiaoHang;

        public PhiGiaoHang(IDonHang donHang, double phiGiaoHang)
            : base(donHang)
        {
            _phiGiaoHang = phiGiaoHang;
        }

        public override string MoTa()
        {
            return base.MoTa() + " + Phi giao hang";
        }

        public override double TinhTongTien()
        {
            return base.TinhTongTien() + _phiGiaoHang;
        }
    }

    // 7. Concrete Decorator: Dong goi qua tang
    public class DongGoiQuaTang : DonHangDecorator
    {
        private double _phiDongGoi;

        public DongGoiQuaTang(IDonHang donHang, double phiDongGoi)
            : base(donHang)
        {
            _phiDongGoi = phiDongGoi;
        }

        public override string MoTa()
        {
            return base.MoTa() + " + Dong goi qua tang";
        }

        public override double TinhTongTien()
        {
            return base.TinhTongTien() + _phiDongGoi;
        }
    }

    // 8. Client: He thong quan ly ban hang
    class Program
    {
        static void Main(string[] args)
        {
            // Don hang co ban
            IDonHang donHang1 = new DonHang(
                "Laptop Dell", 18000000, 1);

            // Don hang ap dung giam gia VIP
            IDonHang donHang2 = new GiamGiaVIP(
                new DonHang("Dien thoai Samsung", 12000000, 1),
                0.10);

            // Don hang ap dung ma giam gia va phi giao hang
            IDonHang donHang3 = new PhiGiaoHang(
                new MaGiamGia(
                    new DonHang("Tai nghe Bluetooth", 2000000, 2),
                    200000),
                50000);

            // Don hang VIP, ma giam gia, giao hang va dong goi qua tang
            IDonHang donHang4 = new DongGoiQuaTang(
                new PhiGiaoHang(
                    new MaGiamGia(
                        new GiamGiaVIP(
                            new DonHang("Dong ho thong minh", 5000000, 1),
                            0.10),
                        300000),
                    40000),
                60000);

            IDonHang[] danhSachDonHang =
            {
                donHang1,
                donHang2,
                donHang3,
                donHang4
            };

            Console.WriteLine("===== QUAN LY BAN HANG =====");

            for (int i = 0; i < danhSachDonHang.Length; i++)
            {
                Console.WriteLine($"\n----- Don hang {i + 1} -----");
                Console.WriteLine($"San pham va dich vu: {danhSachDonHang[i].MoTa()}");
                Console.WriteLine($"Tong thanh toan: {danhSachDonHang[i].TinhTongTien():N0} VND");
            }

            Console.ReadKey();
        }
    }
}
