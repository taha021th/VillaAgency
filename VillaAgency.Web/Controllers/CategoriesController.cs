using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VillaAgency.Application.Handlers.Categories.Commands;
using VillaAgency.Application.Handlers.Categories.Queries;

namespace VillaAgency.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly IMediator _mediator;
        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            return Ok(categories);
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery() { Id=id });
            if (category==null) return NotFound();

            return Ok(category);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
        {

            var categoryId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = categoryId }, command);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryCommand command)
        {

            if (id!=command.Id) return BadRequest("ID mismatch");
            await _mediator.Send(command);
            return NoContent();

        }


        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteCategoryCommand { Id=id });
            return NoContent();
        }

    }
}
