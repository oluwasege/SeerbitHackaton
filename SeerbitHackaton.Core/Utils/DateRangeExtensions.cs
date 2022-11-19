global using SeerbitHackaton.Core.ViewModels;
namespace SeerbitHackaton.Core.Utils
{
    public static class DateRangeExtensions
    {
        public static TimeBoundSearchVm SetDateRange(this TimeBoundSearchVm model)
        {
            if (model == null)
                return model;

            switch (model.TimeRange)
            {
                case Enums.DateRangeQueryType.Daily:
                    {
                        if (model.FromDate == null)
                            model.FromDate = DateTime.Now.Date.AddDays(-7);
                        break;
                    }
                case Enums.DateRangeQueryType.Weekly:
                    {
                        if (model.FromDate == null)
                            model.FromDate = DateTime.Now.Date.AddDays(-7);
                        break;
                    }
                case Enums.DateRangeQueryType.Monthly:
                    {
                        if (model.FromDate == null)
                            model.FromDate = DateTime.Now.Date.AddMonths(-1);
                        break;
                    }
                case Enums.DateRangeQueryType.Yearly:
                    {
                        if (model.FromDate == null)
                            model.FromDate = DateTime.Now.Date.AddMonths(-12);
                        break;
                    }
            }

            return model;
        }
    }
}
