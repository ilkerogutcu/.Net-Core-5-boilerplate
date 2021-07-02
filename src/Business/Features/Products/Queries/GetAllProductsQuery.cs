using System.Collections.Generic;
using Core.Utilities.Pagination;
using Core.Utilities.Results;
using Entities.DTOs;
using MediatR;

namespace Business.Features.Products.Queries
{
    public class GetAllProductsQuery : IRequest<PagedResponse<List<ProductDto>>>
    {
        public PaginationFilter PaginationFilter { get; init; }
        public string Route { get; init; }
    }
}