using Microsoft.AspNetCore.Mvc;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Features.Transactions.Commands.CreateTransaction;
using MyBudgetManagement.Application.Wrappers;

namespace MyBudgetManagement.API.Controllers
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
    }
}