
using System.Data.Entity;
using System.Linq;
using System.Data.Entity.Core;

namespace University.Models
{
    public class UniversityContext
        : DbContext

    {
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Lecturer> Lecturers { get; set; }
        public virtual DbSet<Utils.StudentRequest> Requests { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().
                HasMany(d => d.Lecturers).
                WithMany(p => p.Students).
                Map(t => t.MapLeftKey("StudentId").
                MapRightKey("Lecturer").
                ToTable("LecturerStudent"));

            modelBuilder.Entity<Student>().Property(Id => Id.Account.FirstName);
            modelBuilder.Entity<Student>().Property(Id => Id.Account.LastName);
            modelBuilder.Entity<Student>().Property(Id => Id.Account.Gender);
            modelBuilder.Entity<Student>().Property(Id => Id.Account.CreatedDate);
            modelBuilder.Entity<Student>().Property(Id => Id.Account.Email);
            modelBuilder.Entity<Student>().Property(Id => Id.Account.Password);
            modelBuilder.Entity<Student>().Property(Id => Id.Account.AccountId);



            modelBuilder.Entity<Lecturer>().Property(Id => Id.Account.FirstName);
            modelBuilder.Entity<Lecturer>().Property(Id => Id.Account.LastName);
            modelBuilder.Entity<Lecturer>().Property(Id => Id.Account.Gender);
            modelBuilder.Entity<Lecturer>().Property(Id => Id.Account.CreatedDate);
            modelBuilder.Entity<Lecturer>().Property(Id => Id.Account.Email);
            modelBuilder.Entity<Lecturer>().Property(Id => Id.Account.Password);
            modelBuilder.Entity<Lecturer>().Property(Id => Id.Account.AccountId);

            base.OnModelCreating(modelBuilder);
        }
    }
}