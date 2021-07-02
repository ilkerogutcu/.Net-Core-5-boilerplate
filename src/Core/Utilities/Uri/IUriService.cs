using Core.Utilities.Pagination;

namespace Core.Utilities.Uri
{
    public interface IUriService
    {
        System.Uri GetPageUri(PaginationFilter filter, string route);
    }
}