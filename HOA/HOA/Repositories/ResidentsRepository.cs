using HOA.Models;

namespace HOA.Repositories.Interfaces
{
    public class ResidentsRepository : RepositoryBase<Resident>, IResidentsRepository
    {
        public ResidentsRepository(HOADbContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
