using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Core.Entities
{
    public class Payroll : FullAuditedEntity<long>
    {
        public decimal AmountToBePaid { get; set; }
        public decimal AmountOfHours { get; set; }
        public Employee Employee { get; set; }
        public long EmployeeId { get; set; }
        public decimal AmountPerHour { get; set; }
        public long CompanyId { get; set; }
        public bool HasBeenPaid { get; set; }
    }
}
