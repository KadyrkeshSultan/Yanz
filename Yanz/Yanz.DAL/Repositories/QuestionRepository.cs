using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Yanz.DAL.EF;
using Yanz.DAL.Entities;
using Yanz.DAL.Interfaces;

namespace Yanz.DAL.Repositories
{
    public class QuestionRepository : IRepository<Question>
    {
        private AppDbContext db;

        public QuestionRepository(AppDbContext dbContext)
        {
            db = dbContext;
        }

        public void Add(Question item) => db.Questions.Add(item);

        public async Task AddAsync(Question item) => await db.Questions.AddAsync(item);

        public void Delete(Question item) => db.Questions.Remove(item);

        public Question Get(string id)
        {
            return db.Questions.FirstOrDefault(f => f.Id == id);
        }

        public async Task<Question> GetAsync(string id)
        {
            return await db.Questions.FirstOrDefaultAsync(f => f.Id == id);
        }

        public void Update(Question item) => db.Questions.Update(item);
    }
}
