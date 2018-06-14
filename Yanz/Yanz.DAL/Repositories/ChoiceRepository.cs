using Yanz.DAL.EF;
using Yanz.DAL.Entities;

namespace Yanz.DAL.Repositories
{
    public class ChoiceRepository : Repository<Choice>
    {
        public ChoiceRepository(AppDbContext dbContext)
            :base(dbContext)
        {
        }
        
    }
}
