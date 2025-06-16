using MediatR;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public decimal? Budget { get; set; } = 0; 
    public CategoryType Type { get; set; }
    public Period? Period { get; set; }
    public bool IsDefault { get; set; }
    public CategoryLevel Level { get; set; }
}