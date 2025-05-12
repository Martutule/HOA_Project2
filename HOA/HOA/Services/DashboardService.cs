using HOA.Models;
using HOA.Repositories.Interfaces;
using HOA.Services.Interfaces;

namespace HOA.Services
{
    public class DashboardService : IDashboardService
    {
        private IRepositoryWrapper _repositoryWrapper;
        public DashboardService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public Dashboard GetDashboardData()
        {
            int totalResidents = _repositoryWrapper.ResidentsRepository.FindAll().Count();
            var totalPayments = _repositoryWrapper.PaymentsRepository.FindByCondition(p => p.Status != "Paid");
            float totalDuePayments = 0;

            foreach(Payment payment in totalPayments)
            {
                totalDuePayments += payment.Amount;
            }

            int totalEvents = _repositoryWrapper.EventsRepository.FindAll().Count();
            //i want to get the last 3 announcements from the database
            var recentAnnouncements = _repositoryWrapper.AnnouncementsRepository.FindAll()
                .Take(3)
                .ToList();
            
            return new Dashboard(totalResidents, totalDuePayments, totalEvents, recentAnnouncements);
        }
    }
}
