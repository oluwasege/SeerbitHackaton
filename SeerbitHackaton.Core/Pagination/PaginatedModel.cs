using SeerbitHackaton.Core.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Pagination
{
    public class PaginatedModel<T> : BasePagedList<T> where T : class
    {
        public PaginatedModel(IEnumerable<T> items, int pageNumber, int pageSize, int totalCount) : base(pageNumber, pageSize, totalCount)
        {
            Items = items;
        }

        public override IEnumerable<T> Items { get; }
    }
}
