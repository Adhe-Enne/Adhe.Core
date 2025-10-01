using System.Collections.Generic;

namespace Core.Framework.Contracts.Shared.Grid.Interfaces
{
    public interface IGridResponse<T>
    {
        IEnumerable<T> Data { get; set; }

        int TotalRecords { get; set; }
    }
}
