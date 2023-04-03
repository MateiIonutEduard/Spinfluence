using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
#pragma warning disable

namespace Spinfluence.Data
{
    public class SpinContext : DbContext
    {
        public SpinContext(DbContextOptions<SpinContext> options) : base(options)
        { }

        public DbSet<Account> Account { get; set; }
        public DbSet<Practice> Practice { get; set; }
        public DbSet<CompanyEvent> CompanyEvent { get; set; }
        public DbSet<Company> Company { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("Account");
            modelBuilder.Entity<Practice>().ToTable("Practice");
            modelBuilder.Entity<CompanyEvent>().ToTable("CompanyEvent");
            modelBuilder.Entity<Company>().ToTable("Company");
        }
    }
}
