using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    
}