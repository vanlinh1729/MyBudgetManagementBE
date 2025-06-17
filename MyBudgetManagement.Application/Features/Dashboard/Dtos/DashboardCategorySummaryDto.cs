using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Dashboard.Dtos;

public class DashboardCategorySummaryDto
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string Icon { get; set; }
    public CategoryType Type { get; set; }
    public decimal TotalAmount { get; set; }
}