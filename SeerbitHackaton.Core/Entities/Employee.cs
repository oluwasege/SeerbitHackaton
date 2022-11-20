using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Core.Entities
{
    public class Employee : FullAuditedEntity<long>
    {
        public string EmployeeNO { get; set; }
        public User User { get; set; }
        public long UserId { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public List<Payroll> Payroll { get; set; }
        public Company Company { get; set; }
        public long CompanyId { get; set; }
    }
}
