namespace Core.Utilities.Results
{
    public class PagedResponse<T> : IDataResult<T> where T :  new()
    {
        public bool Success { get; }
        public string Message { get; }
        public T Data { get; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public System.Uri FirstPage { get; set; }
        public System.Uri LastPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public System.Uri NextPage { get; set; }
        public System.Uri PreviousPage { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber<=0?1:pageNumber;
            PageSize = pageSize<=0?1:pageSize;
            Data = data;
            Message = "List paged.";
            Success = true;
        }
    }
}