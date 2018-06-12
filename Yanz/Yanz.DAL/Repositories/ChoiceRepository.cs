using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Yanz.DAL.EF;
using Yanz.DAL.Entities;
using Yanz.DAL.Interfaces;

namespace Yanz.DAL.Repositories
{
    public class ChoiceRepository : IRepository<Choice>
    {
        AppDbContext db;

        public ChoiceRepository(AppDbContext dbContext)
        {
            db = dbContext;
        }

        public void Add(Choice item) => db.Choices.Add(item);

        public async Task AddAsync(Choice item) => await db.Choices.AddAsync(item);

        public void Delete(Choice item) => db.Choices.Remove(item);

        public Choice Get(string id)
        {
            return db.Choices.FirstOrDefault(c => c.Id == id);
        }

        public async Task<Choice> GetAsync(string id)
        {
            return await db.Choices.FirstOrDefaultAsync(c => c.Id == id);
        }

        public void Update(Choice item) => db.Choices.Update(item);
    }
}
