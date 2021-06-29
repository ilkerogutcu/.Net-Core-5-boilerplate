using System.Threading.Tasks;
using Business.Features.Authentication.Commands;
using Business.Features.Authentication.Queries;
using Core.Entities.DTOs.Authentication.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
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

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SignUpResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("sign-up/user")]
        public async Task<IActionResult> SignUpUser(SignUpUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SignInResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignInCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("get-by-id")]
        public async Task<IActionResult> GetUserById([FromBody] GetUserByIdQuery query)
        {
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("get-by-username")]
        public async Task<IActionResult> GetUserByUsername([FromBody] GetUserByUserNameQuery query)
        {
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("get-by-email")]
        public async Task<IActionResult> GetUserByEmail([FromBody] GetUserByEmailQuery query)
        {
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }
    }
}