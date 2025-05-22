using HOA.Models;

namespace HOA.Repositories.Interfaces
{
    public interface IIncidentsRepository : IRepositoryBase<Incident>
    {
        void UpdateIncidentStatus(int id, string state);
    }
}
