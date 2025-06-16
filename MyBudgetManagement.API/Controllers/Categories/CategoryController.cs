using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBudgetManagement.Application.Features.Categories.Commands.CreateCategory;
using MyBudgetManagement.Application.Features.Categories.Commands.UpdateCategory;
using MyBudgetManagement.Application.Features.Categories.Commands.DeleteCategory;
using MyBudgetManagement.Application.Features.Categories.Queries.GetCategoryList;
using MyBudgetManagement.Application.Features.Categories.Queries.GetCategoryById;

namespace MyBudgetManagement.API.Controllers.Categories;

[Authorize]
[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateCategoryCommand command)
    {
        if (id != command.Id) return BadRequest("ID không khớp.");
        await _mediator.Send(command);
        return Ok("Cập nhật Category thành công!");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteCategoryCommand { Id = id });
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetCategoryByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await _mediator.Send(new GetCategoryListQuery());
        return Ok(result);
    }
}