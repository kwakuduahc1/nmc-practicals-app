using NursingPracticals.Models;
using Riok.Mapperly.Abstractions;

namespace NursingPracticals.Mappers
{
    [Mapper]
    public partial class StudentsMapper
    {
        public partial Students FromAddModel(AddStudentModel model);
        public partial Students FromEditModel(EditStudentModel model);
    }

    [Mapper]
    public partial class ProgramsMapper
    {
        public partial Programs AddProgram(AddProgramModel program);

        public partial Programs EditProgram(EditProgramModel program);
    }

    [Mapper]
    public partial class ClassesMappers
    {
        public partial MainClasses AddClass(AddMainClassModel classModel);
    }

    [Mapper]
    public partial class TaskGroupMapper
    {
        public partial TaskGroups AddTask(AddTaskGroupsModel taskGroup);

        public partial TaskGroups EditTask(EditTaskGroupsModel taskGroup);
    }

    [Mapper]
    public partial class ComponentsMapper
    {
        public partial ComponentTasks AddTasks(AddComponentTasks task);
    }
    
    [Mapper]
    public partial class ClassScheduleMappers
    {
        public partial ClassSchedules AddSchedule(AddClassScheduleModel schedule);
    }
}
