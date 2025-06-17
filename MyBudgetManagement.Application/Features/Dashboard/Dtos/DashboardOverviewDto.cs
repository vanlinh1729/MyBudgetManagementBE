namespace MyBudgetManagement.Application.Features.Dashboard.Dtos;

public class DashboardOverviewDto
{
    public decimal Balance { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public int TotalTransactions { get; set; }
}