using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Yanz.Models;
using Yanz.Models.Quiz;

namespace Yanz.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<QuestionSet> QuestionSets { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Choice> Choices { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<QuestionSet>()
                .HasOne(q => q.Folder)
                .WithMany(f => f.QuestionSets)
                .OnDelete(DeleteBehavior.Cascade);
            //builder.Ignore<Item>();
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
