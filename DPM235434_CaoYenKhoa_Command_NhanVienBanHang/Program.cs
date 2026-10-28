using System;
using System.Collections.Generic;

namespace SalesManagement.Behavioral.Command
{
    // Command interface
    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    // Receiver
    public class DonHang
    {
        public string MaDonHang { get; set; }
        public string TenKhachHang { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }

        public DonHang(string maDonHang, string tenKhachHang, decimal tongTien)
        {
            MaDonHang = maDonHang;
            TenKhachHang = tenKhachHang;
            TongTien = tongTien;
            TrangThai = "Moi tao";
        }

        public void XacNhan()
        {
            TrangThai = "Da xac nhan";
            Console.WriteLine($"Don hang {MaDonHang} da duoc xac nhan.");
        }

        public void Huy()
        {
            TrangThai = "Da huy";
            Console.WriteLine($"Don hang {MaDonHang} da bi huy.");
        }

        public void ThanhToan()
        {
            TrangThai = "Da thanh toan";
            Console.WriteLine(
                $"Don hang {MaDonHang} da thanh toan {TongTien:N0} VND.");
        }

        public void HoanTacThanhToan()
        {
            TrangThai = "Da xac nhan";
            Console.WriteLine(
                $"Da hoan tac thanh toan don hang {MaDonHang}.");
        }

        public void HienThiThongTin()
        {
            Console.WriteLine("\n--- THONG TIN DON HANG ---");
            Console.WriteLine($"Ma don hang: {MaDonHang}");
            Console.WriteLine($"Khach hang: {TenKhachHang}");
            Console.WriteLine($"Tong tien: {TongTien:N0} VND");
            Console.WriteLine($"Trang thai: {TrangThai}");
        }
    }

    // Concrete Command: Xac nhan don hang
    public class XacNhanDonHangCommand : ICommand
    {
        private readonly DonHang _donHang;
        private string _trangThaiCu;

        public XacNhanDonHangCommand(DonHang donHang)
        {
            _donHang = donHang;
        }

        public void Execute()
        {
            _trangThaiCu = _donHang.TrangThai;
            _donHang.XacNhan();
        }

        public void Undo()
        {
            _donHang.TrangThai = _trangThaiCu;
            Console.WriteLine(
                $"Da hoan tac xac nhan don hang {_donHang.MaDonHang}.");
        }
    }

    // Concrete Command: Thanh toan don hang
    public class ThanhToanDonHangCommand : ICommand
    {
        private readonly DonHang _donHang;
        private string _trangThaiCu;

        public ThanhToanDonHangCommand(DonHang donHang)
        {
            _donHang = donHang;
        }

        public void Execute()
        {
            _trangThaiCu = _donHang.TrangThai;
            _donHang.ThanhToan();
        }

        public void Undo()
        {
            _donHang.HoanTacThanhToan();
        }
    }

    // Concrete Command: Huy don hang
    public class HuyDonHangCommand : ICommand
    {
        private readonly DonHang _donHang;
        private string _trangThaiCu;

        public HuyDonHangCommand(DonHang donHang)
        {
            _donHang = donHang;
        }

        public void Execute()
        {
            _trangThaiCu = _donHang.TrangThai;
            _donHang.Huy();
        }

        public void Undo()
        {
            _donHang.TrangThai = _trangThaiCu;
            Console.WriteLine(
                $"Da khoi phuc trang thai don hang {_donHang.MaDonHang}.");
        }
    }

    // Invoker
    public class QuanLyLenh
    {
        private readonly Stack<ICommand> _lichSuLenh = new Stack<ICommand>();

        public void ThucThiLenh(ICommand command)
        {
            command.Execute();
            _lichSuLenh.Push(command);
        }

        public void HoanTacLenh()
        {
            if (_lichSuLenh.Count == 0)
            {
                Console.WriteLine("Khong co lenh nao de hoan tac.");
                return;
            }

            ICommand command = _lichSuLenh.Pop();
            command.Undo();
        }
    }

    // Client
    internal class Program
    {
        static void Main(string[] args)
        {
            DonHang donHang1 = new DonHang(
                "DH001",
                "Nguyen Van An",
                1500000
            );

            DonHang donHang2 = new DonHang(
                "DH002",
                "Tran Thi Binh",
                2750000
            );

            QuanLyLenh quanLyLenh = new QuanLyLenh();

            while (true)
            {
                Console.WriteLine("\n===== QUAN LY BAN HANG =====");
                Console.WriteLine("1. Xem thong tin don hang");
                Console.WriteLine("2. Xac nhan don hang");
                Console.WriteLine("3. Thanh toan don hang");
                Console.WriteLine("4. Huy don hang");
                Console.WriteLine("5. Hoan tac lenh gan nhat");
                Console.WriteLine("0. Thoat");
                Console.Write("Chon chuc nang: ");

                string luaChon = Console.ReadLine();

                if (luaChon == "0")
                    break;

                if (luaChon == "5")
                {
                    quanLyLenh.HoanTacLenh();
                    continue;
                }

                DonHang donHang = ChonDonHang(donHang1, donHang2);

                if (donHang == null)
                    continue;

                ICommand command = null;

                switch (luaChon)
                {
                    case "1":
                        donHang.HienThiThongTin();
                        continue;

                    case "2":
                        command = new XacNhanDonHangCommand(donHang);
                        break;

                    case "3":
                        if (donHang.TrangThai != "Da xac nhan")
                        {
                            Console.WriteLine(
                                "Don hang phai duoc xac nhan truoc khi thanh toan.");
                            continue;
                        }

                        command = new ThanhToanDonHangCommand(donHang);
                        break;

                    case "4":
                        if (donHang.TrangThai == "Da thanh toan")
                        {
                            Console.WriteLine(
                                "Khong the huy don hang da thanh toan.");
                            continue;
                        }

                        command = new HuyDonHangCommand(donHang);
                        break;

                    default:
                        Console.WriteLine("Lua chon khong hop le.");
                        continue;
                }

                quanLyLenh.ThucThiLenh(command);
            }

            Console.WriteLine("Ket thuc chuong trinh.");
        }

        static DonHang ChonDonHang(DonHang donHang1, DonHang donHang2)
        {
            Console.WriteLine("\nChon don hang:");
            Console.WriteLine("1. DH001 - Nguyen Van An");
            Console.WriteLine("2. DH002 - Tran Thi Binh");
            Console.Write("Lua chon: ");

            string luaChon = Console.ReadLine();

            switch (luaChon)
            {
                case "1":
                    return donHang1;

                case "2":
                    return donHang2;

                default:
                    Console.WriteLine("Ma don hang khong hop le.");
                    return null;
            }
        }
    }
}   