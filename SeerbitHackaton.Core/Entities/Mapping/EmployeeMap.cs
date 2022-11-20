using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Core.Entities.Mapping
{
    public class EmployeeMap
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            SetupEmployee(builder);
        }

        private void SetupEmployee(EntityTypeBuilder<Employee> builder)
        {
            var employee = new Employee
            {
                AccountNumber = "0000000000",
                BankName = "Polaris Bank Limited",
                EmployeeNO = "Seer3089",
                UserId = 4,
                CreationTime = Clock.Now,
                Id = 1,
                CompanyId = 1

            };

            var employee2 = new Employee
            {
                AccountNumber = "0000000000",
                BankName = "Polaris Bank Limited",
                EmployeeNO = "Seer3060",
                UserId = 3,
                CreationTime = Clock.Now,
                Id = 1,
                CompanyId = 1

            };
            var employees=new List<Employee>() { employee,employee2};
        }
    }
}
