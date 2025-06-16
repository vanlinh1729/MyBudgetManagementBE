using MediatR;
using MyBudgetManagement.Application.Features.Transactions.Dtos;

namespace MyBudgetManagement.Application.Features.Transactions.Queries.GetTransactionById;

public class GetTransactionByIdQuery: IRequest<TransactionDto>
{
    public Guid Id { get; set; }

    public GetTransactionByIdQuery(Guid id)
    {
        Id = id;
    }
}