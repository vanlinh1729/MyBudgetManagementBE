using MediatR;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand: IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal? Budget { get; set; }
    public string? Icon { get; set; }
    public CategoryLevel? Level { get; set; }
    public Period? Period { get; set; }
}