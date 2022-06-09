using ProjetBrasserie.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjetBrasserie.Repositories
{
    public class BrasserieRepository<T> : IRepository<T>
        where T : class
    {

        private readonly BrasserieDbContext _context;
        public BrasserieRepository(BrasserieDbContext context)
        {
            _context = context;
        }

        public bool Add(T entity)
        {
            _context.Set<T>().Add(entity);
            var test = _context.SaveChanges();
            return test > 0;
        }

        public T Get(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public void Remove(int id)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity == null) return;

            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.SaveChanges();
        }
    }
}
