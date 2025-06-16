using AutoMapper;
using MediatR;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Features.Categories.Dtos;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetCategoryByIdQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser, IMapper mapper)
    {
        _uow = uow;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _uow.Categories.GetByIdAsync(request.Id);
        if (category == null || category.UserId != _currentUser.UserId)
            throw new NotFoundException("Không tìm thấy danh mục.");

        return _mapper.Map<CategoryDto>(category);
    }
}