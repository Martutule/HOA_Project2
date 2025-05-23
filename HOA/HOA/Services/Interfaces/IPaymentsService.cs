using HOA.Models;

namespace HOA.Services.Interfaces
{
    public interface IPaymentsService
    {
        Payment GetPaymentsById(int id);
        void AddPayment(Payment payment);
        void UpdatePayment(Payment payment);
        void DeletePayment(int id);
        IEnumerable<Payment> GetAllPayments();
        IEnumerable<Payment> SearchPaymentsByApartmentNumber(string searchQuery);
        void UpdatePaymentStatus(int id, string state);
    }
}
