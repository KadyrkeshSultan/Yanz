using System.Collections.Generic;
using System.Threading.Tasks;
using Yanz.DAL.Entities;

namespace Yanz.DAL.Interfaces
{
    public interface IFolderRepository : IRepository<Folder>
    {
        IEnumerable<Folder> GetAllByUser(string userId);
        Task<IEnumerable<Folder>> GetAllByUserAsync(string userId);
    }
}
