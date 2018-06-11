using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xunit;
using Yanz.DAL.EF;
using Yanz.DAL.Repositories;

namespace YanzTest
{
    public class FoldersControllerTest
    {
        DbContextOptions<AppDbContext> options;

        public FoldersControllerTest()
        {
            var serviceProvider = new ServiceCollection().AddEntityFrameworkSqlServer().BuildServiceProvider();
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-YanzWeb;Trusted_Connection=True;MultipleActiveResultSets=true").UseInternalServiceProvider(serviceProvider);
            options = builder.Options;
        }

        [Fact]
        public void TestFolderNull()
        {
            var unit = new EFUnitOfWork(options);
            var folders = ((FolderRepository)unit.Folders).GetAll("18ff86b3-c1da-46ec-acec-8f7d4dd8e2c1");

            Assert.Empty(folders);
            //userManager = new UserManager<ApplicationUser>();
            //FoldersController controller = new FoldersController();
        }

        [Fact]
        public void TestFoldersNullUser()
        {
            var unit = new EFUnitOfWork(options);
            var f = unit.Folders.Get(null);
            Assert.Null(f);
            //userManager = new UserManager<ApplicationUser>();
            //FoldersController controller = new FoldersController();
        }
    }
}
