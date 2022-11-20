using SeerbitHackaton.Core.Enums;
using SeerbitHackaton.Core.Extensions;

namespace SeerbitHackaton.Core.ViewModels
{
    public class UserVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long Id { get; set; }
        //public string AccountName { get; set; }
        public string State { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public UserType UserType { get; set; }
        public bool IsCompanyAdmin { get; set; }
        public bool IsSuperAdmin { get; set; }
        public bool IsEmployee { get; set; }
        public string UserStatus { get; set; }
        public IList<string> Roles { get; set; }

        public static implicit operator UserVM(User model)
        {
            return model == null
                ? null
                : new UserVM()
                {
                    Id = model.Id,
                    DateOfBirth = model.DateOfBirth,
                    FirstName = model.FirstName,
                    Gender = model.Gender.GetDescription(),
                    IsEmployee = model.IsEmployee,
                    IsCompanyAdmin = model.IsCompanyAdmin,
                    IsSuperAdmin = model.IsSuperAdmin,
                    LastName = model.LastName,
                    //State = model.State,
                    UserType = model.UserType,
                    UserStatus = model.UserStatus.GetDescription()
                };
        }
    }


}