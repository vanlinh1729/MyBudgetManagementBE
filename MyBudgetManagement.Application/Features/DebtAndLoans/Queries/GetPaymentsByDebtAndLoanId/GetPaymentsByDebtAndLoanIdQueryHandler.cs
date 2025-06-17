using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Features.Transactions.Dtos;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Application.Features.DebtAndLoans.Queries.GetPaymentsByDebtAndLoanId;

public class
    GetPaymentsByDebtAndLoanIdQueryHandler : IRequestHandler<GetPaymentsByDebtAndLoanIdQuery, List<TransactionDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetPaymentsByDebtAndLoanIdQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser, IMapper mapper)
    {
        _uow = uow;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<List<TransactionDto>> Handle(GetPaymentsByDebtAndLoanIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var transactions = await _uow.Transactions.Query()
            .Where(t => t.DebtAndLoanId == request.DebtAndLoanId && t.CreatedBy == userId)
            .OrderByDescending(t => t.Date)
            .ProjectTo<TransactionDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return transactions;
    }
}