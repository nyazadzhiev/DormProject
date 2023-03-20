using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DormProject.DormOffice.Data
{
    public class DormDbContext : DbContext
    {
        public virtual DbSet<Student> Students { get; set; }

        public DormDbContext(DbContextOptions<DormDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            StudentModelBuilder(modelBuilder);
            FamilyMemberModelBuilder(modelBuilder);
            SpecialityModelBuilder(modelBuilder);
            RoomModelBuilder(modelBuilder);
        }

        private void RoomModelBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().HasKey(s => s.Id);
            modelBuilder.Entity<Room>().HasMany(s => s.Students).WithOne(r => r.Room).HasForeignKey(s => s.RoomId);
        }

        private void SpecialityModelBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Speciality>().HasKey(s => s.Id);
            modelBuilder.Entity<Speciality>().Property(s => s.Name).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Speciality>().Property(s => s.ShortName).IsRequired().HasMaxLength(3);
            modelBuilder.Entity<Speciality>().HasMany(s => s.Students).WithOne(s => s.Speciality).HasForeignKey(s => s.SpecialityId);
        }

        private void FamilyMemberModelBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FamilyMember>().HasKey(s => s.Id);
            modelBuilder.Entity<FamilyMember>().Property(s => s.FirstName).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<FamilyMember>().Property(s => s.LastName).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<FamilyMember>().HasOne(s => s.Student).WithMany(f => f.FamilyMembers).HasForeignKey(f => f.StudentId);
        }

        private static void StudentModelBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasKey(s => s.Id);
            modelBuilder.Entity<Student>().Property(s => s.FirstName).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Student>().Property(s => s.LastName).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Student>().Property(s => s.Grade).IsRequired();
            modelBuilder.Entity<Student>().Property(s => s.Adress).HasDefaultValue(null);
            modelBuilder.Entity<Student>().Property(s => s.FacultyNumber).HasDefaultValue(null);
            modelBuilder.Entity<Student>().Property(s => s.PhoneNumber).HasDefaultValue(null);
            modelBuilder.Entity<Student>().HasMany(s => s.FamilyMembers).WithOne(f => f.Student).HasForeignKey(f => f.StudentId);
            modelBuilder.Entity<Student>().HasOne(s => s.Speciality).WithMany(s => s.Students).HasForeignKey(s => s.SpecialityId);
            modelBuilder.Entity<Student>().HasOne(s => s.Room).WithMany(r => r.Students).HasForeignKey(s => s.RoomId);
        }
    }
}
