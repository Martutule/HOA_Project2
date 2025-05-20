using HOA.Models;

namespace HOA.Repositories.Interfaces
{
    public interface IPaymentsRepository : IRepositoryBase<Payment>
    {
        void UpdatePaymentStatus(int id, string state);
    }
}
