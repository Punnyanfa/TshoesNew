using FCSP.Common.Enums;
using FCSP.Models.Entities;

namespace FCSP.Services
{
    public interface IPaymentProcessor
    {
        Task<bool> ProcessPaymentAsync(Payment payment);
    }

    public class PaymentProcessor : IPaymentProcessor
    {
        public async Task<bool> ProcessPaymentAsync(Payment payment)
        {
            // Giả lập gọi cổng thanh toán
            // Trả về true nếu thành công, false nếu thất bại
            await Task.Delay(1000); // Giả lập thời gian xử lý
            return payment.PaymentMethod == PaymentMethod.CashOnDelivery || new Random().Next(0, 2) == 1;
        }
    }
}