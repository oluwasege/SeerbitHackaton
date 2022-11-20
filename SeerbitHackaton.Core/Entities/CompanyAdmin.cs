using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Core.Entities
{
    public class CompanyAdmin : FullAuditedEntity<long>
    {
        public Company Company { get; set; }
        public long CompanyId { get; set; }
        public User User { get; set; }
        public long UserId { get; set; }
    }
}
