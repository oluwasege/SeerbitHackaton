using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Core.Utils
{
    public class JwtSettings
    {
        public string Site { get; set; }
        public string Audience { get; set; }
        public string DurationInMinutes { get; set; }
        public string SecretKey { get; set; }
    }
}
