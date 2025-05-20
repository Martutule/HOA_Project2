using HOA.Models;
using HOA.Repositories.Interfaces;

namespace HOA.Repositories
{
    public class PaymentsRepository : RepositoryBase<Payment>, IPaymentsRepository
    {
        public PaymentsRepository(HOADbContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdatePaymentStatus(int id, string state)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.Id == id);

            if (payment != null)
            {
                payment.Status = state;
                _context.SaveChanges();
            }
        }
    }
}
