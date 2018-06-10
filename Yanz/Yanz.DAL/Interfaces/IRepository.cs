using System.Collections.Generic;

namespace Yanz.DAL.Interfaces
{
    public interface IRepository<T> where T: class
    {
        IEnumerable<T> GetAll();
        T Get(string id);
        void Add(T item);
        void Update(T item);
        void Delete(T item);
    }
}
