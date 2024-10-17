using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NursingPracticals.Models
{
    public class Programs
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProgramsID { get; set; }

        [StringLength(75)]
        [Required]
        public required string ProgramName { get; set; }

        public IEnumerable<ComponentTasks>? ComponentTasks { get; set; }

        public IEnumerable<MainClasses>? MainClasses { get; set; }
    }

    public class TaskGroups
    {
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskGroupsID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public required string GroupName { get; set; }

        [Required]
        public required int[] Programs { get; set; }

        public virtual ICollection<ComponentTasks>? ComponentTasks { get; set; }
    }

    public class ComponentTasks
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short ComponentTasksID { get; set; }

        [Required]
        [StringLength(200)]
        public required string ComponentTask { get; set; }

        [Range(1, 10)]
        [DefaultValue(5)]
        public byte Difficulty { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [Required]
        public int TaskGroupsID { get; set; }

        public virtual TaskGroups? TaskGroups { get; set; }

        public virtual ICollection<Steps>? Steps { get; set; }
    }

    public class Steps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int StepsID { get; set; }

        [StringLength(150)]
        [Required]
        public required string StepName { get; set; }

        [Required]
        public int ComponentTasksID { get; set; }

        public virtual ComponentTasks? ComponentTasks { get; set; }
    }

    public class MainClasses
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MainClassesID { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3)]
        public required string ClassName { get; set; }

        [Required]
        public int ProgramsID { get; set; }

        [DefaultValue(false)]
        public bool ClassStatus { get; set; }

        public virtual Programs? Programs { get; set; }

        public ICollection<ClassSchedules>? ClassSchedules { get; set; }
    }


    public class Students
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentID { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public required string IndexNumber { get; set; }

        [Required]
        public required int MainClassesID { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 3)]
        public required string FullName { get; set; }

        [StringLength(200)]
        public string? ImageUrl { get; set; }

        [DefaultValue(false)]
        public bool IsActive{ get; set; }

        public virtual ICollection<Results>? Results { get; set; }
    }

    public class ClassSchedules
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClassSchedulesID { get; set; }

        [Required]
        public required string ComponentTasksID { get; set; }

        [Required]
        [StringLength(75)]
        public required string ScheduleName { get; set; }

        [Required]
        public required DateTime ExamDate { get; set; }
    }

    public class Results
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResultsID { get; set; }

        [Required]
        public int StudentsID { get; set; }

        [Required]
        public int ClassSchedulesID { get; set; }
    }

    public class Exams
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExamsID { get; set; }

        [Required]
        public int ResultsID { get; set; }

        [Required]
        public int StepsID { get; set; }

        [Required]
        public double Score { get; set; }
    }
}
