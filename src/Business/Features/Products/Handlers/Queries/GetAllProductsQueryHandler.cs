using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Business.Features.Products.Queries;
using Business.Helpers;
using Core.Aspects.Autofac.Logger;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using Core.Utilities.Uri;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using MediatR;

namespace Business.Features.Products.Handlers.Queries
{
    /// <summary>
    /// Get paginated all products
    /// </summary>
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PagedResponse<List<ProductDto>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;

        public GetAllProductsQueryHandler(IProductRepository productRepository, IUriService uriService, IMapper mapper)
        {
            _productRepository = productRepository;
            _uriService = uriService;
            _mapper = mapper;
        }

        [LogAspect(typeof(FileLogger))]
        public async Task<PagedResponse<List<ProductDto>>> Handle(GetAllProductsQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _productRepository.GetListAsync();
            var resultMapped = _mapper.Map<List<ProductDto>>(result);
            var totalRecord = await _productRepository.GetCountAsync();

            if (request.PaginationFilter.PageNumber <= 0 || request.PaginationFilter.PageSize <= 0)
            {
                return PaginationHelper.CreatePagedResponse(resultMapped, request.PaginationFilter, totalRecord,
                    _uriService, request.Route);
            }

            resultMapped = resultMapped.Skip((request.PaginationFilter.PageNumber - 1) * request.PaginationFilter.PageSize)
                .Take(request.PaginationFilter.PageSize).ToList();
            var response = PaginationHelper.CreatePagedResponse(resultMapped, request.PaginationFilter, totalRecord,
                _uriService, request.Route);
            return response;
        }
    }
}