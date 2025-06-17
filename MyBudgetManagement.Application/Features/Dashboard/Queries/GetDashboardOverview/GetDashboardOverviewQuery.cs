using MediatR;
using MyBudgetManagement.Application.Features.Dashboard.Dtos;

namespace MyBudgetManagement.Application.Features.Dashboard.Queries.GetDashboardOverview;

public class GetDashboardOverviewQuery: IRequest<DashboardOverviewDto> { }