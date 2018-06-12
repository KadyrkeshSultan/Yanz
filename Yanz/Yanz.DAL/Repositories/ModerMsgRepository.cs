using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Yanz.DAL.EF;
using Yanz.DAL.Entities;
using Yanz.DAL.Interfaces;

namespace Yanz.DAL.Repositories
{
    public class ModerMsgRepository : IRepository<ModerMsg>
    {
        AppDbContext db;

        public ModerMsgRepository(AppDbContext dbContext)
        {
            db = dbContext;
        }

        public void Add(ModerMsg item) => db.ModerMsgs.Add(item);

        public async Task AddAsync(ModerMsg item) => await db.ModerMsgs.AddAsync(item);

        public void Delete(ModerMsg item) => db.ModerMsgs.Remove(item);

        public ModerMsg Get(string id)
        {
            return db.ModerMsgs.FirstOrDefault(m => m.Id == id);
        }

        public async Task<ModerMsg> GetAsync(string id)
        {
            return await db.ModerMsgs.FirstOrDefaultAsync(m => m.Id == id);
        }

        public void Update(ModerMsg item) => db.ModerMsgs.Update(item);
    }
}
