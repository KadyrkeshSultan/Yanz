using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Yanz.DAL.EF;
using Yanz.DAL.Entities;
using Yanz.DAL.Interfaces;

namespace Yanz.DAL.Repositories
{
    public class QuestionRepository : Repository<Question>
    {
        public QuestionRepository(AppDbContext dbContext)
            :base(dbContext)
        {

        }
    }
}
