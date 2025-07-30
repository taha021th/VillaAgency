// استفاده از فضاهای نام مورد نیاز



// تعریف فضای نام برای کنترلرها
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VillaAgency.Application.Handlers.Properties.Commands;
using VillaAgency.Application.Handlers.Properties.Queries;

namespace VillaAgency.Web.Controllers
{
    [Route("api/[controller]")] // تعیین مسیر پایه برای این کنترلر به صورت "api/properties"
    [ApiController] // مشخص می‌کند که این کلاس یک کنترلر API است
    public class PropertiesController : ControllerBase
    {
        // فیلد فقط-خواندنی برای نگهداری نمونه‌ای از Mediator
        private readonly IMediator _mediator;

        // سازنده برای تزریق وابستگی Mediator
        public PropertiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // ارسال کوئری GetAllPropertiesQuery به MediatR برای پردازش
            var properties = await _mediator.Send(new GetAllPropertiesQuery());
            // بازگرداندن نتیجه با کد وضعیت 200 OK
            return Ok(properties);
        }

        [HttpGet("{id:guid}")] // این متد به درخواست‌های GET به مسیر "api/properties/{id}" پاسخ می‌دهد و id را از نوع guid می‌پذیرد
        public async Task<IActionResult> GetById(Guid id)
        {
            // ارسال کوئری GetPropertyByIdQuery با id دریافت شده به MediatR
            var property = await _mediator.Send(new GetPropertyByIdQuery(id));
            // اگر ملکی پیدا نشد
            if (property == null)
            {
                // بازگرداندن کد وضعیت 404 Not Found
                return NotFound();
            }
            // بازگرداندن ملک پیدا شده با کد وضعیت 200 OK
            return Ok(property);
        }
        [Authorize(Roles = "Admin,Agent")]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePropertyCommand createPropertyCommand)
        {
            try
            {
                await _mediator.Send(createPropertyCommand);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }
        [Authorize(Roles = "Admin,Agent")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdatePropertyCommand updatePropertyCommand)
        {
            if (id!=updatePropertyCommand.Id) return BadRequest();

            try
            {
                await _mediator.Send(updatePropertyCommand);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        [Authorize(Roles = "Admin,Agent")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _mediator.Send(new DeletePropertyCommand(id));
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] GetPaginatedFilteredPropertiesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);

        }


    }
}
