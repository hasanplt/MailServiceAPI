using HasanPolatCom.Application.Features.Commands.CreateMail;
using HasanPolatCom.Application.Features.Commands.UpdateMail;
using HasanPolatCom.Application.Features.Queries.GetAllMailByReceiver;
using HasanPolatCom.Application.Features.Queries.GetAllMails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HasanPolatCom.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "Member")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMediator mediator;

        public MailController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send(CreateMailCommand mail)
        {
            var response = await mediator.Send(mail);

            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);
        }

        [HttpGet("getMailsByReceiver/{mailAddress}")]
        public async Task<IActionResult> GetMailsByReceiver(string mailAddress)
        {
            var query = new GetAllMailByReceiver(mailAddress);

            var response = await mediator.Send(query);

            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);

        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllMails()
        {
            var query = new GetAllMailsQuery();

            return Ok(await mediator.Send(query));
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(UpdateMailCommand updateMailCommand)
        {
            return Ok(await mediator.Send(updateMailCommand));
        }
    }
}
