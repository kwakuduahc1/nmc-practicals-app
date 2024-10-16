using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NursingPracticals.Models;

namespace NursingPracticals.Contexts
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUsers>(options)
    {
        public virtual DbSet<Programs> Programs { get; set; }

        public virtual DbSet<ComponentTasks> ComponentTasks { get; set; }

        public virtual DbSet<Steps> Steps { get; set; }

        public virtual DbSet<TaskGroups> TaskGroups { get; set; }

        public virtual DbSet<MainClasses> MainClasses { get; set; }

        public virtual DbSet<Students> Students { get; set; }

        public virtual DbSet<ClassSchedules> ClassSchedules { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Programs>(x => x.HasData(
                new Programs { ProgramName = "Public Health Nursing", ProgramsID = 1 },
                new Programs { ProgramName = "Nurse Assistant Preventive", ProgramsID = 2 },
                new Programs { ProgramName = "Post-NAC/NAP Midwifery", ProgramsID = 3, }));

            builder.Entity<TaskGroups>(x => x.HasData(
                new TaskGroups { GroupName = "Public Health", Programs = [1, 2], TaskGroupsID = 1 },
                new TaskGroups { GroupName = "General Nursing", Programs = [1, 3], TaskGroupsID = 2 },
                new TaskGroups { GroupName = "Midwifery", Programs = [3], TaskGroupsID = 3 }
                ));

            foreach (var entity in builder.Model.GetEntityTypes())
            {
                // Replace table names
                entity.SetTableName(entity.GetTableName()?.ToLower());

                // Replace column names            
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.Name.ToLower());
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName()?.ToLower());
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName()?.ToLower());
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.SetDatabaseName(index.Name?.ToLower());
                }
            }

            base.OnModelCreating(builder);
        }
    }
}
