using Microsoft.EntityFrameworkCore;

namespace ru.snowprelicator.populate_database
{

    public class ApplicationContext : DbContext
    {
        private string dbConnectionString;

        public ApplicationContext(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(dbConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Horse> HorseTable { get; set; }
    }
}
