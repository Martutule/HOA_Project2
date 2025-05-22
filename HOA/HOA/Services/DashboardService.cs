using HOA.Models;
using HOA.Repositories.Interfaces;
using HOA.Services.Interfaces;
using System.Linq;

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
            foreach (Payment payment in totalPayments)
            {
                totalDuePayments += payment.Amount;
            }

            int totalEvents = _repositoryWrapper.EventsRepository.FindAll().Count();

            // Get the last 3 announcements
            var recentAnnouncements = _repositoryWrapper.AnnouncementsRepository.FindAll()
                .OrderByDescending(a => a.Id)
                .Take(3)
                .ToList();

            // Get all notifications, ordered by Id (latest first)
            var notifications = _repositoryWrapper.NotificationRepository.FindAll()
                .OrderByDescending(n => n.Id)
                .ToList();

            // Create and return the dashboard object
            return new Dashboard(totalResidents, totalDuePayments, totalEvents, recentAnnouncements)
            {
                Notifications = notifications
            };
        }
    }
}
