using SeerbitHackaton.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Core.ViewModels.UserViewModel
{
    public class EmployeeResponse
    {
        public string EmployeeNO { get; set; }
        public long Id { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string UserStatus { get; set; }
        public string CompanyName { get; set; }

        public static implicit operator EmployeeResponse(Employee model)
        {
            return model == null
                ? null
                : new EmployeeResponse()
                {
                    Id = model.Id,
                    DateOfBirth = model.User.DateOfBirth,
                    FirstName = model.User.FirstName,
                    Gender = model.User.Gender.GetDescription(),
                    LastName = model.User.LastName,
                    UserStatus = model.User.UserStatus.GetDescription(),
                    AccountNumber = model.AccountNumber,
                    BankAccountNumber = model.BankAccountNumber,
                    BankName = model.BankName,
                    EmployeeNO = model.EmployeeNO,
                     CompanyName= model.Company.Name,
                };
        }
    }
}
