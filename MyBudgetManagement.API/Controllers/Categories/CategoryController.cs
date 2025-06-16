using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBudgetManagement.Application.Features.Categories.Commands.CreateCategory;

namespace MyBudgetManagement.API.Controllers.Categories;

[Authorize]
[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ISender _mediator;

    public CategoryController(ISender mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { id });
    }
    
}