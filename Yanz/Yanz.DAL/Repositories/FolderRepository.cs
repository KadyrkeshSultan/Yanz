﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yanz.DAL.EF;
using Yanz.DAL.Entities;
using Yanz.DAL.Interfaces;

namespace Yanz.DAL.Repositories
{
    public class FolderRepository : Repository<Folder>, IFolderRepository
    {
        public FolderRepository(AppDbContext dbContext)
            :base(dbContext)
        {
        }

        public IEnumerable<Folder> GetAllByUser(string userId)
        {
            return db.Folders.Where(f => f.AppUserId == userId).ToList();
        }

        public async Task<IEnumerable<Folder>> GetAllByUserAsync(string userId)
        {
            return await db.Folders.Where(f => f.AppUserId == userId).ToListAsync();
        }
    }
}
