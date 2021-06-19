using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StarterProject.Business.Features.Authentication.Commands;
using Microsoft.Extensions.DependencyInjection;
namespace StarterProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("sign-up/user")]
        public async Task<IActionResult> SignUpUser(SignUpUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}