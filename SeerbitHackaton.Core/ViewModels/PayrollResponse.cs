using SeerbitHackaton.Core.Extensions;
using SeerbitHackaton.Core.ViewModels.UserViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Core.ViewModels
{
    public class PayrollResponse
    {
        public long Id { get; set; }
        public decimal AmountToBePaid { get; set; }
        public decimal AmountOfHours { get; set; }
        public string EmployeeName { get; set; }
        public decimal AmountPerHour { get; set; }
        public bool HasBeenPaid { get; set; }
        public static implicit operator PayrollResponse(Payroll model)
        {
            return model == null
                ? null
                : new PayrollResponse()
                {
                    Id = model.Id,
                    AmountOfHours = model.AmountOfHours,
                    EmployeeName = model.Employee.User.FullName,
                    AmountToBePaid = model.AmountToBePaid,
                    AmountPerHour = model.AmountPerHour,
                    HasBeenPaid = model.HasBeenPaid
                };
        }
    }
}
