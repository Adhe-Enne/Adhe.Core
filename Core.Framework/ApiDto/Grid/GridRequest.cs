using System.Collections.Generic;

namespace Core.Framework
{
    public class GridRequest
    {
        public Paging Paging { get; set; }

        public Sorting Sorting { get; set; }

        public string Filter { get; set; } = string.Empty;

        public IEnumerable<Filter> Filters { get; set; }
        public GridRequest()
        {
            Paging = new Paging() { PageNumber = 1, PageSize = 10 };
            Sorting = new Sorting() { Name = "Id", Order = "ASC" };
            Filter = string.Empty;
        }
    }


}
