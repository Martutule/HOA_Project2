using HOA.Models;
using HOA.Repositories.Interfaces;

namespace HOA.Repositories
{
    public class IncidentsRepository : RepositoryBase<Incident>, IIncidentsRepository
    {
        public IncidentsRepository(HOADbContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public void UpdateIncidentStatus(int id, string state)
        {
            var incident = _context.Incidents.FirstOrDefault(i => i.Id == id);
            if (incident != null)
            {
                incident.Status = state;
                _context.SaveChanges();
            }
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
