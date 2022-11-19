using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SeerbitHackaton.Core.Entities
{
    public class Role : IdentityRole<long>
    {
        public bool IsActive { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? CreationTime { get; set; }
        public bool IsDefaultRole { get; set; }
    }
}
