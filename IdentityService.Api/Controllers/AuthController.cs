using HasanPolatCom.Application.Features.Commands.CreateUser;
using HasanPolatCom.Application.Features.Queries.GetUserWithToken;
using HasanPolatCom.Application.Features.Queries.ValidUserAndCreateToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace HasanPolatCom.WebApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly HttpContext httpContext;

        public AuthController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            this.mediator = mediator;
            httpContext = httpContextAccessor.HttpContext;
        }

        [HttpPost]
        public async Task<IActionResult> GetToken(ValidUserAndCreateTokenQuery queryValid)
        {
            
            var response = await mediator.Send(queryValid);

            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);
        }

        [Authorize(Policy = "Member")]
        [HttpGet]
        public async Task<IActionResult> GetUserWithToken()
        {

            GetUserWithTokenQuery query = new GetUserWithTokenQuery();

            var response = await mediator.Send(query);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserCommand command)
        {
            var response = await mediator.Send(command);

            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);
        }

        

    }
}
