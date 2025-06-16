using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Transactions.Dtos;

    public class TransactionDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public string? Image { get; set; }

        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public CategoryType CategoryType { get; set; }

        public Guid? DebtAndLoanId { get; set; }
    }
