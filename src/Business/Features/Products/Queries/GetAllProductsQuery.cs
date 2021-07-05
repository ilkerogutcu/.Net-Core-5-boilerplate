using System.Collections.Generic;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.DTOs;
using MediatR;

namespace Business.Features.Products.Queries
{
    public class GetAllProductsQuery : IRequest<IDataResult<IEnumerable<ProductDto>>>
    {
        public PaginationFilter PaginationFilter { get; init; }
        public string Route { get; init; }
    }
}