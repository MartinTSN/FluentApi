namespace FluentApi.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model : DbContext
    {
        public Model()
            : base("name=ProjectManagementModel")
        {
        }

        public virtual DbSet<ContactInfo> ContactInfos { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOptional(e => e.ContactInfo)
                .WithRequired(e => e.Employee);

            modelBuilder.Entity<Team>()
                .HasOptional(team => team.Employees)
                .WithRequired();

            modelBuilder.Entity<Project>()
                .HasOptional(project => project.Teams)
                .WithRequired();
        }
    }
}
