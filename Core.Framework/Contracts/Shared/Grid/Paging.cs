namespace Core.Framework.Contracts.Shared.Grid
{
    public class Paging
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public Paging()
        {
            PageNumber = 1;
            PageSize = 10;
        }
        public Paging(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > 10 ? 10 : pageSize;
        }
    }
}
