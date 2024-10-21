using NursingPracticals.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NursingPracticals.Models
{
    public class AddProgramModel
    {
        [StringLength(75)]
        [Required]
        public required string ProgramName { get; set; }
    }

    public class EditProgramModel
    {
        public short ProgramsID { get; set; }

        [StringLength(75)]
        [Required]
        public required string ProgramName { get; set; }
    }

    public class AddMainClassModel
    {
        [Required]
        [StringLength(15, MinimumLength = 3)]
        public required string ClassName { get; set; }

        [Required]
        public short ProgramsID { get; set; }
    }

    public class ViewClassModel
    {
        public int MainClassesID { get; set; }

        public required string ClassName { get; set; }

        public int ProgramsID { get; set; }
    }


    public class EditMainClassModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MainClassesID { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3)]
        public required string ClassName { get; set; }

        [Required]
        public short ProgramsID { get; set; }
    }

    public class AddStudentModel
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public required string IndexNumber { get; set; }

        [Required]
        public required int MainClassesID { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 3)]
        public required string FullName { get; set; }
    }

    public class EditStudentModel
    {
        [Required]
        public int StudentID { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public required string IndexNumber { get; set; }

        [Required]
        public required int MainClassesID { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 3)]
        public required string FullName { get; set; }
    }

    public class ProgramListModel
    {
        public short ProgramsID { get; set; }
        public required string ProgramName { get; set; }
    }

    public class MainClassListModel
    {
        public int MainClassesID { get; set; }
        public required string ClassName { get; set; }
        public short ProgramsID { get; set; }
    }

    public class AddTaskGroupsModel
    {
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public required string GroupName { get; set; }

        [Required]
        public required int[] Programs { get; set; }
    }

    public class EditTaskGroupsModel
    {
        [Required]
        public int TaskGroupsID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public required string GroupName { get; set; }

        [Required]
        public required int[] Programs { get; set; }
    }

    public class AddComponentTasks
    {

        [Required]
        [StringLength(200)]
        public required string ComponentTask { get; set; }

        [Required]
        public int TaskGroupsID { get; set; }

        public required ICollection<AddStepsModel> Steps { get; set; }
    }

    public class AddStepsModel
    {
        [StringLength(150)]
        [Required]
        public required string StepName { get; set; }

        //[Required]
        //public int ComponentTasksID { get; set; }
    }

    public class AddFullStepsModel
    {
        [StringLength(150)]
        [Required]
        public required string StepName { get; set; }

        [Required]
        public short ComponentTasksID { get; set; }
    }

    public class EditComponentTaskModel
    {

        [Required]
        public short ComponentTasksID { get; set; }

        [Required]
        [StringLength(200)]
        public required string ComponentTask { get; set; }

        [Required]
        public int TaskGroupsID { get; set; }
    }

    public class EditStepsModel
    {
        [Required]
        public int StepsID { get; set; }

        [StringLength(150)]
        [Required]
        public required string StepName { get; set; }

        [Required]
        public int ComponentTasksID { get; set; }
    }

    public class AddClassScheduleModel
    {
        [Required]
        public int TaskGroupsID { get; set; }

        [Required]
        [StringLength(75)]
        public required string ScheduleName { get; set; }

        [Required]
        public int MainClassesID { get; set; }

        [Required]
        public required DateTime ExamDate { get; set; }
    }

    public class EditClassSchedule
    {
        [Required]
        public int ClassSchedulesID { get; set; }

        [Required]
        public int MainClassesID { get; set; }

        [Required]
        public int TaskGroupsID { get; set; }

        [Required]
        [StringLength(75)]
        public required string ScheduleName { get; set; }

        [Required]
        public required DateTime ExamDate { get; set; }
    }

    public record ClassScheduleVm(int ClassSchedulesID, string ScheduleName, DateTime ExamDate, int MainClassesID, string ClassName, bool IsActive);

    public record TaskGroupsVm(int TaskGroupsID, string GroupName);
}
