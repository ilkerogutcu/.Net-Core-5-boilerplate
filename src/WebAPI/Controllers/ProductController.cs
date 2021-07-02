using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Features.Products.Queries;
using Core.Utilities.Pagination;
using Core.Utilities.Results;
using Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<List<Product>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllProducts([FromQuery] PaginationFilter paginationFilter)
        {
            var result = await _mediator.Send(new GetAllProductsQuery
            {
                PaginationFilter = paginationFilter,
                Route = Request.Path.Value
            });
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
    }
}