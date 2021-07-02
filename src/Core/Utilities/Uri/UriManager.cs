using Core.Utilities.Pagination;
using Microsoft.AspNetCore.WebUtilities;

namespace Core.Utilities.Uri
{
    public class UriManager : IUriService
    {
        private readonly string _baseUri;

        public UriManager(string baseUri)
        {
            _baseUri = baseUri;
        }

        public System.Uri GetPageUri(PaginationFilter filter, string route)
        {
            var endpointUri = new System.Uri(string.Concat(_baseUri, route));
            var modifiedUri =
                QueryHelpers.AddQueryString(endpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
            return new System.Uri(modifiedUri);
        }
    }
}