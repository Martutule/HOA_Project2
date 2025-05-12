﻿namespace HOA.Repositories.Interfaces
{
    public interface IRepositoryWrapper
    {
        IResidentsRepository ResidentsRepository { get; }
        IPaymentsRepository PaymentsRepository { get; }
        IMaintenanceRepository MaintenanceRepository { get; }
        IAnnouncementsRepository AnnouncementsRepository { get; }
        IEventsRepository EventsRepository { get; }

        void Save();
    }
}
