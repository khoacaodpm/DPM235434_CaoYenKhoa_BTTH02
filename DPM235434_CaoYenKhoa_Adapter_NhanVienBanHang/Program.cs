
using System;

namespace SalesManagement.AdapterPattern
{
    // 1. Target: Giao dien thanh toan ma he thong ban hang mong muon
    public interface IPayment
    {
        void Pay(double amount);
    }

    // 2. Adaptee: Cong thanh toan cu, co giao dien khong tuong thich
    public class LegacyPaymentGateway
    {
        public void MakePayment(double totalAmount)
        {
            Console.WriteLine("Dang xu ly thanh toan qua cong cu cu...");
            Console.WriteLine($"So tien thanh toan: {totalAmount:N0} VND");
            Console.WriteLine("Thanh toan thanh cong!");
        }
    }

    // 3. Adapter: Chuyen doi giao dien cua cong thanh toan cu
    public class PaymentAdapter : IPayment
    {
        private readonly LegacyPaymentGateway _gateway;

        public PaymentAdapter(LegacyPaymentGateway gateway)
        {
            _gateway = gateway;
        }

        public void Pay(double amount)
        {
            // Chuyen loi goi Pay() sang MakePayment()
            _gateway.MakePayment(amount);
        }
    }

    // 4. San pham
    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }

        public Product(string name, double price)
        {
            Name = name;
            Price = price;
        }
    }

    // 5. He thong quan ly ban hang (Client)
    public class SalesSystem
    {
        private readonly IPayment _payment;

        public SalesSystem(IPayment payment)
        {
            _payment = payment;
        }

        public void Checkout(Product product, int quantity)
        {
            if (quantity <= 0)
            {
                Console.WriteLine("So luong phai lon hon 0.");
                return;
            }

            double total = product.Price * quantity;

            Console.WriteLine("\n===== HOA DON BAN HANG =====");
            Console.WriteLine($"San pham: {product.Name}");
            Console.WriteLine($"Don gia: {product.Price:N0} VND");
            Console.WriteLine($"So luong: {quantity}");
            Console.WriteLine($"Tong tien: {total:N0} VND");

            Console.WriteLine("\n===== THANH TOAN =====");
            _payment.Pay(total);
        }
    }

    // 6. Chuong trinh chinh
    class Program
    {
        static void Main(string[] args)
        {
            // Khoi tao cong thanh toan cu
            LegacyPaymentGateway oldGateway =
                new LegacyPaymentGateway();

            // Su dung Adapter de tuong thich giao dien
            IPayment payment = new PaymentAdapter(oldGateway);

            // Tao he thong ban hang
            SalesSystem sales = new SalesSystem(payment);

            // Tao san pham
            Product product = new Product("Ban phim co", 750000);

            // Khach hang mua 2 san pham
            sales.Checkout(product, 2);

            Console.ReadKey();
        }
    }
}
