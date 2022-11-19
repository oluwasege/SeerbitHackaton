

namespace SeerbitHackaton.Core.ViewModels
{
    public class QueryModel : PagedRequestModel
    {
        public string Keyword { get; set; }
        public string Filter { get; set; }
    }

    public class PagedRequestModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "PageIndex must be greater than 0")]
        public int PageIndex { get; set; } = PaginationConsts.PageIndex;
        [Range(1, int.MaxValue, ErrorMessage = "PageSize must be greater than 0")]
        public int PageSize { get; set; } = PaginationConsts.PageSize;
    }
}
