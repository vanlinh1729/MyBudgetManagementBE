using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Features.Transactions.Dtos;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Application.Features.Transactions.Queries.GetTransactionList;

public class GetTransactionListQueryHandler : IRequestHandler<GetTransactionListQuery, List<TransactionDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetTransactionListQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser, IMapper mapper)
    {
        _uow = uow;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<List<TransactionDto>> Handle(GetTransactionListQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var transactions = await _uow.Transactions
            .Query()
            .Where(t =>
                t.UserBalance.UserId == userId &&
                t.Date.Year == request.Year &&
                t.Date.Month == request.Month)
            .Include(t => t.Category)
            .OrderByDescending(t => t.Date)
            .ProjectTo<TransactionDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return transactions;
    }
}