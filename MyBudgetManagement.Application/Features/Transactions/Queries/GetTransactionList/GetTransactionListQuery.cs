using MediatR;
using MyBudgetManagement.Application.Features.Transactions.Dtos;

namespace MyBudgetManagement.Application.Features.Transactions.Queries.GetTransactionList;

public class GetTransactionListQuery : IRequest<List<TransactionDto>>
{
    public int Year { get; set; }
    public int Month { get; set; }

    public GetTransactionListQuery(int year, int month)
    {
        Year = year;
        Month = month;
    }
}