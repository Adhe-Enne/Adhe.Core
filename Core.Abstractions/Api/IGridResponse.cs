using System.Collections.Generic;

namespace Core.Contracts
{
    public interface IGridResponse<T>
    {
        IEnumerable<T> Data { get; set; }

        int TotalRecords { get; set; }
    }
}
