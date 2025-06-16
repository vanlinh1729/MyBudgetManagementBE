using MediatR;

namespace MyBudgetManagement.Application.Features.Transactions.Commands.DeleteTransaction;

public class DeleteTransactionCommand : IRequest
{
    public Guid Id { get; set; }

    public DeleteTransactionCommand(Guid id)
    {
        Id = id;
    }
}