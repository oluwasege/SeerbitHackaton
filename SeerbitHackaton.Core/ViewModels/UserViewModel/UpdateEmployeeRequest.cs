using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Core.ViewModels.UserViewModel
{
    public class UpdateEmployeeRequest:UpdateUserRequest
    {
        //public long EmployeeId { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
    }
    public class UpdateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PhoneNumber { get; set; }
        public string DateOfBirth { get; set; }
    }
    public class UpdateCompanyAdminRequest:UpdateUserRequest
    {
        //public long CompanyAdminId { get; set; }
    }

    public class UpdateSuperAdminRequest : UpdateUserRequest
    {
        //public long SuperAdminId { get; set; }
    }

}
