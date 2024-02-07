using System.Collections.Generic;

namespace Core.Abstractions
{
    public interface IGridResponse<T>
    {
        IEnumerable<T> Data { get; set; }

        int TotalRecords { get; set; }
    }
}
