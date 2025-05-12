using HOA.Models;
using HOA.Repositories.Interfaces;

namespace HOA.Repositories
{
    public class AnnouncementsRepository: RepositoryBase<Announcement>, IAnnouncementsRepository
    {
        public AnnouncementsRepository(HOADbContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
