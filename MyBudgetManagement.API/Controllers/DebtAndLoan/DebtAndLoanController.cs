using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBudgetManagement.Application.Features.DebtAndLoans.Commands.CreateDebtAndLoan;
using MyBudgetManagement.Application.Features.DebtAndLoans.Commands.DeleteDebtAndLoan;
using MyBudgetManagement.Application.Features.DebtAndLoans.Commands.PayDebtAndLoan;
using MyBudgetManagement.Application.Features.DebtAndLoans.Commands.UpdateDebtAndLoan;
using MyBudgetManagement.Application.Features.DebtAndLoans.Queries.GetDebtAndLoanById;
using MyBudgetManagement.Application.Features.DebtAndLoans.Queries.GetDebtAndLoanList;
using MyBudgetManagement.Application.Features.DebtAndLoans.Queries.GetDebtAndLoanOverview;
using MyBudgetManagement.Application.Features.DebtAndLoans.Queries.GetDebtAndLoanSummary;
using MyBudgetManagement.Application.Features.DebtAndLoans.Queries.GetPaymentsByDebtAndLoanId;

namespace MyBudgetManagement.API.Controllers.DebtAndLoans;

[Authorize]
[ApiController]
[Route("api/debt-and-loans")]
public class DebtAndLoanController : ControllerBase
{
    private readonly IMediator _mediator;

    public DebtAndLoanController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateDebtAndLoanCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateDebtAndLoanCommand command)
    {
        if (id != command.Id) return BadRequest("ID không khớp.");
        await _mediator.Send(command);
        return Ok("Update DebtAndLoan thành công!");
    }
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await _mediator.Send(new GetDebtAndLoanListQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetDebtAndLoanByIdQuery { Id = id });
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteDebtAndLoanCommand { Id = id });
        return Ok("Delete DebtAndLoan thành công!");
    }

    [HttpPost("{id}/pay")]
    public async Task<IActionResult> PayDebtAndLoan(Guid id, [FromBody] PayDebtAndLoanCommand command)
    {
        if (id != command.DebtAndLoanId)
            return BadRequest("ID không khớp.");

        await _mediator.Send(command);
        return Ok("PayDebtAndLoan thành công!");
    }
    [HttpGet("{id}/payments")]
    public async Task<IActionResult> GetPayments(Guid id)
    {
        var result = await _mediator.Send(new GetPaymentsByDebtAndLoanIdQuery(id));
        return Ok(result);
    }

    [HttpGet("{id}/summary")]
    public async Task<IActionResult> GetSummary(Guid id)
    {
        var result = await _mediator.Send(new GetDebtAndLoanSummaryQuery(id));
        return Ok(result);
    }
    [HttpGet("overview")]
    public async Task<IActionResult> GetOverview()
    {
        var result = await _mediator.Send(new GetDebtAndLoanOverviewQuery());
        return Ok(result);
    }


}