using System.Collections.Generic;

namespace Core.Framework 
{
    public class GridResponse<T> : Core.Abstractions.IGridResponse<T>
    {
        public IEnumerable<T> Data { get; set; }

        public int TotalRecords { get; set; }
    }
}
