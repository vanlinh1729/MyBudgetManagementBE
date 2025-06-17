using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Features.Dashboard.Dtos;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Application.Features.Dashboard.Queries.GetDashboardSummaryByCategory;

public class GetDashboardSummaryByCategoryQueryHandler : IRequestHandler<GetDashboardSummaryByCategoryQuery, List<DashboardCategorySummaryDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetDashboardSummaryByCategoryQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<List<DashboardCategorySummaryDto>> Handle(GetDashboardSummaryByCategoryQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        var now = DateTime.UtcNow;

        var transactions = await _uow.Transactions.Query()
            .Where(t => t.UserBalance.UserId == userId && t.Date.Month == now.Month && t.Date.Year == now.Year)
            .Include(t => t.Category)
            .ToListAsync(cancellationToken);

        var result = transactions
            .GroupBy(t => new { t.CategoryId, t.Category.Name, t.Category.Type, t.Category.Icon })
            .Select(g => new DashboardCategorySummaryDto
            {
                CategoryId = g.Key.CategoryId,
                CategoryName = g.Key.Name,
                Icon = g.Key.Icon,
                Type = g.Key.Type,
                TotalAmount = g.Sum(t => t.Amount)
            })
            .ToList();

        return result;
    }
}