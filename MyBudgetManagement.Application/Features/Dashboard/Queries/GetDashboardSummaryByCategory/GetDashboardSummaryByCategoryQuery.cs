using MediatR;
using MyBudgetManagement.Application.Features.Dashboard.Dtos;

namespace MyBudgetManagement.Application.Features.Dashboard.Queries.GetDashboardSummaryByCategory;

public class GetDashboardSummaryByCategoryQuery : IRequest<List<DashboardCategorySummaryDto>> { }
