using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Core.Entities
{
    public class Company : FullAuditedEntity<long>
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public virtual List<CompanyAdmin> CompanyAdmins { get; set; }
        public List<Employee> Employees { get; set; }
        public string CAC { get; set; }
    }
}
