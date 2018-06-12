using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Yanz.DAL.EF;
using Yanz.DAL.Entities;
using Yanz.DAL.Interfaces;

namespace Yanz.DAL.Repositories
{
    public class QuestionSetRepository : IRepository<QuestionSet>
    {
        AppDbContext db;

        public QuestionSetRepository(AppDbContext dbContext)
        {
            db = dbContext;
        }

        public void Add(QuestionSet item) => db.QuestionSets.Add(item);

        public async Task AddAsync(QuestionSet item) => await db.QuestionSets.AddAsync(item);

        public void Delete(QuestionSet item) => db.QuestionSets.Remove(item);

        public QuestionSet Get(string id)
        {
            return db.QuestionSets.FirstOrDefault(q => q.Id == id);
        }

        public async Task<QuestionSet> GetAsync(string id)
        {
            return await db.QuestionSets.FirstOrDefaultAsync(q => q.Id == id);
        }

        public void Update(QuestionSet item) => db.QuestionSets.Update(item);
    }
}
