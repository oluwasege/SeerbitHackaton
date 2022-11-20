using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Core.ViewModels.UserViewModel
{
    public class RegisterUserVM
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        //[Required]
        //public string State { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Role { get; set; }
    }

    public class CreateEmployeeRequest : RegisterUserVM
    {
        public string EmployeeNO { get; set; }
        public long UserId { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public long CompanyId { get; set; }
    }

    public class CreateCompanyAdminRequest : RegisterUserVM
    {
        public long CompanyId { get; set; }
        public long UserId { get; set; }
    }
    public class CreateCompanyRequest : CreateCompanyAdminRequest
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string CAC { get; set; }
    }

}
