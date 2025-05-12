using System.Data.Entity;
using System.Linq.Expressions;
using HOA.Models;
using HOA.Repositories.Interfaces;

namespace HOA.Repositories
{
    public abstract class RepositoryBase<T>: IRepositoryBase<T> where T : class
    {
        protected HOADbContext _context { get; set; }
        public RepositoryBase(HOADbContext repositoryContext)
        {
            _context = repositoryContext;
        }

        public IQueryable<T> FindAll()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}
