using Microsoft.EntityFrameworkCore;

namespace ProjectsAPWebApp.Models
{
    public class ProjectAPIContext : DbContext
    {
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Worker> Workers { get; set; }
        public virtual DbSet<Company> Companys { get; set; }
        public ProjectAPIContext(DbContextOptions<ProjectAPIContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Admin)
                .WithMany(a => a.Project)
                .HasForeignKey(p => p.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Worker)
                .WithMany(w => w.Project)
                .HasForeignKey(p => p.WorkerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Worker>()
                .HasOne(w => w.Company)
                .WithMany(c => c.Workers)
                .HasForeignKey(w => w.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
