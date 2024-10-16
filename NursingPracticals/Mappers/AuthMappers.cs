using NursingPracticals.Contexts;
using NursingPracticals.Models.AuthVm;
using Riok.Mapperly.Abstractions;

namespace EduApp.Mappers
{
    [Mapper]
    public partial class RegisterMapper
    {
        public partial ApplicationUsers ToUser(RegisterVm registerVm);
    }
}
