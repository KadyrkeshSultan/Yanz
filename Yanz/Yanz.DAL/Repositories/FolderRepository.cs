using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yanz.DAL.EF;
using Yanz.DAL.Entities;
using Yanz.DAL.Interfaces;

namespace Yanz.DAL.Repositories
{
    public class FolderRepository : IRepository<Folder>
    {
        private AppDbContext db;

        public FolderRepository(AppDbContext dbContext)
        {
            db = dbContext;
        }

        public void Add(Folder item) => db.Folders.Add(item);

        public async Task AddAsync(Folder item) => await db.Folders.AddAsync(item);

        public void Delete(Folder item) => db.Folders.Remove(item);

        public Folder Get(string id) => db.Folders.FirstOrDefault(f => f.Id == id);

        public async Task<Folder> GetAsync(string id)
        {
            return await db.Folders.FirstOrDefaultAsync(f => f.Id == id);
        }

        public IEnumerable<Folder> GetAll(string userId)
        {
            return db.Folders.Where(f => f.AppUserId == userId).ToList();
        }

        public async Task<IEnumerable<Folder>> GetAllAsync(string userId)
        {
            return await db.Folders.Where(f => f.AppUserId == userId).ToListAsync();
        }

        public void Update(Folder item) => db.Folders.Update(item);
    }
}
