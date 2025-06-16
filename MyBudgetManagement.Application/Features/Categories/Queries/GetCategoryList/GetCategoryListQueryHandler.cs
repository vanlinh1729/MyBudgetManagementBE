using AutoMapper;
using MediatR;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Features.Categories.Dtos;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Application.Features.Categories.Queries.GetCategoryList;

public class GetCategoryListQueryHandler : IRequestHandler<GetCategoryListQuery, List<CategoryDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetCategoryListQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser, IMapper mapper)
    {
        _uow = uow;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<List<CategoryDto>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
    {
        var categories = await _uow.Categories.GetCategoriesByUserIdAsync(_currentUser.UserId);
        return _mapper.Map<List<CategoryDto>>(categories);
    }
}