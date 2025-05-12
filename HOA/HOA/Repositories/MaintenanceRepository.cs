using HOA.Models;
using HOA.Repositories.Interfaces;

namespace HOA.Repositories
{
    public class MaintenanceRepository: RepositoryBase<Maintenance>, IMaintenanceRepository
    {
        public MaintenanceRepository(HOADbContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
