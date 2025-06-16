using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Features.Transactions.Dtos;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Application.Features.Transactions.Queries.GetTransactionById;

public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, TransactionDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetTransactionByIdQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser, IMapper mapper)
    {
        _uow = uow;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<TransactionDto> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var transaction = await _uow.Transactions
            .Query()
            .Include(t => t.Category)
            .Include(t => t.UserBalance)
            .FirstOrDefaultAsync(t => t.Id == request.Id && t.UserBalance.UserId == userId, cancellationToken);

        if (transaction == null)
            throw new NotFoundException("Không tìm thấy giao dịch.");

        return _mapper.Map<TransactionDto>(transaction);
    }
}