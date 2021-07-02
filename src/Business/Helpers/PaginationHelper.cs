using System;
using System.Collections.Generic;
using Core.Utilities.Pagination;
using Core.Utilities.Results;
using Core.Utilities.Uri;

namespace Business.Helpers
{
    public static class PaginationHelper
    {
        public static PagedResponse<List<T>> CreatePagedResponse<T>(List<T> pagedData, PaginationFilter validFilter,
            int totalRecords, IUriService uriService, string route)
        {
            var roundedTotalPages = 0;
            var response = new PagedResponse<List<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            var totalPages = totalRecords / (double) validFilter.PageSize;
            if (validFilter.PageNumber <= 0 || validFilter.PageSize <= 0)
            {
                roundedTotalPages = 1;
                validFilter.PageNumber = 1;
                validFilter.PageSize = 1;
            }
            else
            {
                roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            }

            response.NextPage = validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber + 1, validFilter.PageSize), route)
                : null;
            response.PreviousPage = validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber - 1, validFilter.PageSize), route)
                : null;
            response.FirstPage = uriService.GetPageUri(new PaginationFilter(1, validFilter.PageSize), route);
            response.LastPage =
                uriService.GetPageUri(new PaginationFilter(roundedTotalPages, validFilter.PageSize), route);
            response.TotalPages = roundedTotalPages;
            response.TotalRecords = totalRecords;

            return response;
        }
    }
}