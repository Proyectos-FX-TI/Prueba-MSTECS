using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using mstecs_back.Entities;
using mstecs_back.Seeders;

namespace mstecs_back.Helpers
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DevConnection"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            UsuarioSeeder.Seed(modelBuilder);
            RolSeeder.Seed(modelBuilder);
        }

        public DbSet<UsuarioEntitie> Usuarios { get; set; }
        public DbSet<RolEntitie> Roles { get; set; }
    }
}