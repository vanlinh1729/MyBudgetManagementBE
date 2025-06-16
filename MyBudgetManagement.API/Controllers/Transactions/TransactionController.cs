using Microsoft.AspNetCore.Mvc;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MyBudgetManagement.Application.Features.Transactions.Commands.CreateTransaction;
using MyBudgetManagement.Application.Features.Transactions.Commands.DeleteTransaction;
using MyBudgetManagement.Application.Features.Transactions.Commands.UpdateTransaction;
using MyBudgetManagement.Application.Features.Transactions.Queries.GetTransactionById;
using MyBudgetManagement.Application.Features.Transactions.Queries.GetTransactionList;
using MyBudgetManagement.Application.Wrappers;

namespace MyBudgetManagement.API.Controllers.Transactions
{
    [Authorize]
    [ApiController]
    [Route("api/transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly ISender _mediator;

        public TransactionsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransactionCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { id });
        }
        
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] int year, [FromQuery] int month)
        {
            var result = await _mediator.Send(new GetTransactionListQuery(year, month));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetTransactionByIdQuery(id));
            return Ok(result);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateTransactionCommand command)
        {
            if (id != command.Id) return BadRequest("Id không khớp.");
            await _mediator.Send(command);
            return Ok("Cập nhật Transaction thành công!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteTransactionCommand(id));
            return NoContent();
        }

    }
}