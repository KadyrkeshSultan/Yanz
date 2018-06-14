using Yanz.DAL.EF;
using Yanz.DAL.Entities;

namespace Yanz.DAL.Repositories
{
    public class ModerMsgRepository : Repository<ModerMsg>
    {
        public ModerMsgRepository(AppDbContext dbContext)
            :base(dbContext)
        {

        }
    }
}
