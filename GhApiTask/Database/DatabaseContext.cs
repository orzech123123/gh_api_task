using GhApiTask.Database.Records;
using Microsoft.EntityFrameworkCore;

namespace GhApiTask.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"DataSource=github.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommitRecord>().Property(c => c.Commiter).IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<CommitRecord> Commits { get; set; }
    }
}
