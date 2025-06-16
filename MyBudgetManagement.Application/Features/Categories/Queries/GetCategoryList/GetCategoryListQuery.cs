using MediatR;
using MyBudgetManagement.Application.Features.Categories.Dtos;

namespace MyBudgetManagement.Application.Features.Categories.Queries.GetCategoryList;

public class GetCategoryListQuery : IRequest<List<CategoryDto>> { }
