global using System.Reflection;
using SeerbitHackaton.Core.Entities;

namespace SeerbitHackaton.Core.DataAccess.EfCore.Context
{
    /// <Note>
    /// DbSet properties are being used by generic repository
    /// </Note>
    public class ApplicationDbContext : BaseDbContext
    {
        public DbSet<State> States { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.UseOpenIddict();
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
