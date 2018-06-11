using System.Threading.Tasks;

namespace Yanz.DAL.Interfaces
{
    public interface IRepository<T> where T: class
    {
        T Get(string id);
        Task<T> GetAsync(string id);
        void Add(T item);
        Task AddAsync(T item);
        void Update(T item);
        void Delete(T item);
    }
}
