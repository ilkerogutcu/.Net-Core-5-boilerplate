using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.Features.Products.Queries;
using Business.Helpers;
using Core.Aspects.Autofac.Exception;
using Core.Aspects.Autofac.Logger;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.MessageBrokers.RabbitMq;
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
        private readonly IRabbitMqProducer _producer;

        public GetAllProductsQueryHandler(IProductRepository productRepository, IUriService uriService, IMapper mapper, IRabbitMqProducer producer)
        {
            _productRepository = productRepository;
            _uriService = uriService;
            _mapper = mapper;
            _producer = producer;
        }

        [LogAspect(typeof(FileLogger))]
        [ExceptionLogAspect(typeof(FileLogger))]
     //   [SecuredOperation(Roles.Admin,Roles.Moderator,Roles.User)]
        public async Task<IDataResult<IEnumerable<ProductDto>>> Handle(GetAllProductsQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _productRepository.GetListAsync();
            if (result is null)
            {
                return new ErrorDataResult<IEnumerable<ProductDto>>(Messages.DataNotFound);
            }
            
            var resultMapped = _mapper.Map<List<ProductDto>>(result);
            await _producer.Publish(new ProducerModel
            {
                Model = resultMapped[0],
                QueueName = "product-listed"
            });
            return PaginationHelper.CreatePaginatedResponse(resultMapped, request.PaginationFilter, result.Count, _uriService, request.Route);
        }
    }
}