using Microsoft.EntityFrameworkCore; 

namespace AutomotiveDiagnosticTool.API
{
    public class AppDBContext : DbContext 
    {
        public DbSet <User> Users { get; set; }
        public DbSet <DiagnosticData> DiagnosticData { get; set; }
        public AppDBContext (DbContextOptions<AppDBContext> options) : base(options) { }
        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users"); 
                entity.HasKey(e => e.UserId);

            });

            modelBuilder.Entity<DiagnosticData>(entity =>
            {
                entity.ToTable("DiagnosticData");
                entity.HasKey(e => e.DiagnosticDataId);

              

            });



            base.OnModelCreating(modelBuilder);

        }
        

    }
}
