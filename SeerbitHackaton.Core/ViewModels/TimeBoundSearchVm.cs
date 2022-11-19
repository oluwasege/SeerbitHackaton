global using SeerbitHackaton.Core.Enums;

namespace SeerbitHackaton.Core.ViewModels
{
    public class TimeBoundSearchVm
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public DateRangeQueryType TimeRange { get; set; }
    }
}
