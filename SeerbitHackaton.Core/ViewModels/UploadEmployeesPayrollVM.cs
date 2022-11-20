using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Core.ViewModels
{
    public class UploadEmployeesPayrollVM
    {
        public IFormFile File { get; set; }
        public long CompanyId { get; set; }
    }

}
