using SeerbitHackaton.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Core.ViewModels.UserViewModel
{
    public class CompanyAdminResponse
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string UserStatus { get; set; }
        public string CompanyName { get; set; }

        public static implicit operator CompanyAdminResponse(CompanyAdmin model)
        {
            return model == null
                ? null
                : new CompanyAdminResponse()
                {
                    Id = model.Id,
                    DateOfBirth = model.User.DateOfBirth,
                    FirstName = model.User.FirstName,
                    Gender = model.User.Gender.GetDescription(),
                    LastName = model.User.LastName,
                    UserStatus = model.User.UserStatus.GetDescription(),
                    CompanyName = model.Company.Name,
                };
        }
    }
}
