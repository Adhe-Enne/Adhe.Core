using Core.Framework.Contracts.Shared.Grid.Interfaces;
using System.Collections.Generic;

namespace Core.Framework.Contracts.Shared.Grid
{
    public class GridResponse<T> : IGridResponse<T>
    {
        public IEnumerable<T> Data { get; set; }

        public int TotalRecords { get; set; }
    }
}
