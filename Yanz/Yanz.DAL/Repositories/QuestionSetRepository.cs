using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Yanz.DAL.EF;
using Yanz.DAL.Entities;
using Yanz.DAL.Interfaces;

namespace Yanz.DAL.Repositories
{
    public class QuestionSetRepository : Repository<QuestionSet>
    {
        public QuestionSetRepository(AppDbContext dbContext)
            :base(dbContext)
        {

        }
    }
}
