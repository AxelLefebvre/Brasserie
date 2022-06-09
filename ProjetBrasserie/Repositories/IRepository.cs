using System.Collections.Generic;

namespace ProjetBrasserie.Repositories
{
    public interface IRepository<T> where T : class
    {
        bool Add(T entity);
        void Update(T entity);
        T Get(int id);
        void Remove(int id);
        IEnumerable<T> GetAll();
    }
}
