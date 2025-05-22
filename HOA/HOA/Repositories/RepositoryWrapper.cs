using HOA.Models;
using HOA.Repositories.Interfaces;

namespace HOA.Repositories
{
    public class RepositoryWrapper: IRepositoryWrapper
    {
        private readonly HOADbContext _context;
        private IResidentsRepository _residents;
        private IPaymentsRepository _payments;
        private IMaintenanceRepository _maintenance;
        private IAnnouncementsRepository _announcements;
        private IEventsRepository _events;
        private INotificationRepository _notification;

        public RepositoryWrapper(HOADbContext context)
        {
            _context = context;
        }

        public IResidentsRepository ResidentsRepository
        {
            get
            {
                if (_residents == null)
                {
                    _residents = new ResidentsRepository(_context);
                }
                return _residents;
            }
        }

        public IPaymentsRepository PaymentsRepository
        {
            get
            {
                if (_payments == null)
                {
                    _payments = new PaymentsRepository(_context);
                }
                return _payments;
            }
        }

        public IMaintenanceRepository MaintenanceRepository
        {
            get
            {
                if (_maintenance == null)
                {
                    _maintenance = new MaintenanceRepository(_context);
                }
                return _maintenance;
            }
        }

        public IAnnouncementsRepository AnnouncementsRepository
        {
            get
            {
                if (_announcements == null)
                {
                    _announcements = new AnnouncementsRepository(_context);
                }
                return _announcements;
            }
        }

        public IEventsRepository EventsRepository
        {
            get
            {
                if (_events == null)
                {
                    _events = new EventsRepository(_context);
                }
                return _events;
            }
        }

        public INotificationRepository NotificationRepository
        {
            get
            {
                if (_notification == null)
                {
                    _notification = new NotificationRepository(_context);
                }
                return _notification;
            }
        }


        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
