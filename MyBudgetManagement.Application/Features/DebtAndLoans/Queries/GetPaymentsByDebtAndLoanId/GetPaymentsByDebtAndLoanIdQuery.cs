using MediatR;
using MyBudgetManagement.Application.Features.Transactions.Dtos;

namespace MyBudgetManagement.Application.Features.DebtAndLoans.Queries.GetPaymentsByDebtAndLoanId;

public class GetPaymentsByDebtAndLoanIdQuery: IRequest<List<TransactionDto>>
{
    public Guid DebtAndLoanId { get; set; }

    public GetPaymentsByDebtAndLoanIdQuery(Guid debtAndLoanId)
    {
        DebtAndLoanId = debtAndLoanId;
    }
}
