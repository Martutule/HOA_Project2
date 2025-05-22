using HOA.Models;

namespace HOA.Repositories.Interfaces
{
    public class SupplierContractRepository : RepositoryBase<SupplierContract>, ISupplierContractRepository
    {
        public SupplierContractRepository(HOADbContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
