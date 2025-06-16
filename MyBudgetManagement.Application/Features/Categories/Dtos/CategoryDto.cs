using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Categories.Dtos;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal? Budget { get; set; }
    public string? Icon { get; set; }
    public CategoryType Type { get; set; }
    public CategoryLevel? Level { get; set; }
    public Period? Period { get; set; }
    public bool IsDefault { get; set; }
}