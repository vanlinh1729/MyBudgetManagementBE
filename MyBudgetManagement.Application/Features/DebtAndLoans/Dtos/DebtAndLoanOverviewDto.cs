namespace MyBudgetManagement.Application.Features.DebtAndLoans.Dtos;

public class DebtAndLoanOverviewDto
{
    public int DebtCount { get; set; }
    public int LoanCount { get; set; }

    public decimal TotalDebtAmount { get; set; }
    public decimal TotalLoanAmount { get; set; }

    public decimal TotalDebtPaid { get; set; }
    public decimal TotalLoanCollected { get; set; }

    public decimal TotalDebtRemaining => TotalDebtAmount - TotalDebtPaid;
    public decimal TotalLoanRemaining => TotalLoanAmount - TotalLoanCollected;
}
