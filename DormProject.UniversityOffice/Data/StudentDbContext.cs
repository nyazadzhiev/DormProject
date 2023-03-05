using Microsoft.EntityFrameworkCore;

namespace DormProject.UniversityOffice.Data
{
    public class StudentDbContext : DbContext
    {
        public virtual DbSet<Student> Students { get; set; }

        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasKey(s => s.Id);
            modelBuilder.Entity<Student>().Property(s => s.FirstName).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Student>().Property(s => s.LastName).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Student>().Property(s => s.Grade).IsRequired();
        }
    }
}
