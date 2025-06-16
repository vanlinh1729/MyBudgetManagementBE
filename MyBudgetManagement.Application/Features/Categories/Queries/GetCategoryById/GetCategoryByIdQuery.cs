using MediatR;
using MyBudgetManagement.Application.Features.Categories.Dtos;

namespace MyBudgetManagement.Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQuery : IRequest<CategoryDto>
{
    public Guid Id { get; set; }
}