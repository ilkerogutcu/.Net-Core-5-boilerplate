using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.Features.Products.Queries;
using Business.Helpers;
using Core.Aspects.Autofac.Logger;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using Core.Utilities.Uri;
using DataAccess.Abstract;
using Entities.DTOs;
using Entities.Enums;
using MediatR;

namespace Business.Features.Products.Handlers.Queries
{
    /// <summary>
    /// Get paginated all products
    /// </summary>
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery,  IDataResult<IEnumerable<ProductDto>>>
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
        [SecuredOperation(Roles.Admin,Roles.Moderator,Roles.User)]
        public async Task<IDataResult<IEnumerable<ProductDto>>> Handle(GetAllProductsQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _productRepository.GetListAsync();
            if (result == null)
            {
                return new ErrorDataResult<IEnumerable<ProductDto>>(Messages.DataNotFound);
            }
            
            var resultMapped = _mapper.Map<List<ProductDto>>(result);
            var totalRecord = await _productRepository.GetCountAsync();
            return PaginationHelper.CreatePaginatedResponse(resultMapped, request.PaginationFilter, totalRecord, _uriService, request.Route);
        }
    }
}