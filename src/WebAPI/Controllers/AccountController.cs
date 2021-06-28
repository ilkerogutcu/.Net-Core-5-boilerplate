using System.Threading.Tasks;
using Business.Features.Authentication.Commands;
using MediatR;
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

        [HttpPost("sign-up/user")]
        public async Task<IActionResult> SignUpUser(SignUpUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignInCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}