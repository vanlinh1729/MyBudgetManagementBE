using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Features.Dashboard.Dtos;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Dashboard.Queries.GetDashboardOverview;


public class GetDashboardOverviewQueryHandler : IRequestHandler<GetDashboardOverviewQuery, DashboardOverviewDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetDashboardOverviewQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<DashboardOverviewDto> Handle(GetDashboardOverviewQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        var now = DateTime.UtcNow;

        var userBalance = await _uow.UserBalances.GetUserBalanceByUserIdAsync(userId)
                          ?? throw new NotFoundException("Không tìm thấy số dư.");

        var transactions = await _uow.Transactions.Query()
            .Where(t => t.UserBalance.UserId == userId && t.Date.Month == now.Month && t.Date.Year == now.Year)
            .Include(t => t.Category)
            .ToListAsync(cancellationToken);

        var totalIncome = transactions
            .Where(t => t.Category.Type == CategoryType.Income)
            .Sum(t => t.Amount);

        var totalExpense = transactions
            .Where(t => t.Category.Type == CategoryType.Expense)
            .Sum(t => t.Amount);

        return new DashboardOverviewDto
        {
            Balance = userBalance.Balance,
            TotalIncome = totalIncome,
            TotalExpense = totalExpense,
            TotalTransactions = transactions.Count
        };
    }
}
