using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VillaAgency.Application.Handlers.ContactMessages.Commands;

namespace VillaAgency.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactController(IMediator mediator)
        {
            _mediator=mediator;
        }
        [HttpPost]
        public async Task<IActionResult> SubmitContactForm([FromBody] CreateContactMessageCommand command)
        {
            await _mediator.Send(command);
            return Ok(new {message="پیام شما با موفقیت ارسال شد." });
        }
    }
}
