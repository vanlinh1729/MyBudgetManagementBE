using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBudgetManagement.Application.Features.Dashboard.Queries.GetDashboardOverview;
using MyBudgetManagement.Application.Features.Dashboard.Queries.GetDashboardSummaryByCategory;

namespace MyBudgetManagement.API.Controllers.Dashboard;

[Authorize]
[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("overview")]
    public async Task<IActionResult> GetOverview()
    {
        var result = await _mediator.Send(new GetDashboardOverviewQuery());
        return Ok(result);
    }

    [HttpGet("summary-by-category")]
    public async Task<IActionResult> GetSummaryByCategory()
    {
        var result = await _mediator.Send(new GetDashboardSummaryByCategoryQuery());
        return Ok(result);
    }
}