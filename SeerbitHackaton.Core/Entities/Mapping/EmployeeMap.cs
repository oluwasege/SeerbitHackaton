using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Core.Entities.Mapping
{
    public class EmployeeMap:IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable(nameof(Employee));
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
                CompanyId = 1,
                 BankAccountNumber=""

            };

            var employee2 = new Employee
            {
                AccountNumber = "0000000000",
                BankName = "Polaris Bank Limited",
                EmployeeNO = "Seer3060",
                UserId = 3,
                CreationTime = Clock.Now,
                Id = 2,
                CompanyId = 1,
                BankAccountNumber=""

            };
            var employees=new List<Employee>() { employee,employee2};
            builder.HasData(employees);
        }
    }
}
