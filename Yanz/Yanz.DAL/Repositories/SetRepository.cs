using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Yanz.DAL.EF;
using Yanz.DAL.Entities;
using Yanz.DAL.Interfaces;

namespace Yanz.DAL.Repositories
{
    public class SetRepository : IRepository<Set>
    {
        AppDbContext db;

        public SetRepository(AppDbContext dbContext)
        {
            db = dbContext;
        }

        public void Add(Set item) => db.Sets.Add(item);

        public async Task AddAsync(Set item) => await db.Sets.AddAsync(item);

        public void Delete(Set item) => db.Sets.Remove(item);

        public Set Get(string id) => db.Sets.FirstOrDefault(s => s.Id == id);

        public async Task<Set> GetAsync(string id) => await db.Sets.FirstOrDefaultAsync(s => s.Id == id);

        public void Update(Set item) => db.Sets.Update(item);
    }
}
