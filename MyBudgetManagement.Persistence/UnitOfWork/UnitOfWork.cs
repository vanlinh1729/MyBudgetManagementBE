// using MyBudgetManagement.Application.Interfaces;
// using MyBudgetManagement.Domain.Interfaces;
// using MyBudgetManagement.Persistance.Repositories;
//
// namespace MyBudgetManagement.Persistence.UnitOfWork;
//
// public class UnitOfWork : IUnitOfWork
// {
//     private readonly IApplicationDbContext _context;
//
//     public UnitOfWork(IApplicationDbContext context)
//     {
//         _context = context;
//         Users = new UserRepositoryAsync(_context);
//         UserBalances = new UserBalanceRepositoryAsync(_context);
//         Tokens = new TokenRepositoryAsync(_context);
//     }
//     
//     public IUserRepositoryAsync Users { get; }
//     public IUserBalanceRepositoryAsync UserBalances { get; }
//     public ITokenRepositoryAsync Tokens { get; }
//     public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
//     {
//         return await _context.SaveChangesAsync(cancellationToken);
//     }
//     
//     public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
//     {
//         return await _context.SaveChangesAsync(cancellationToken) > 0;
//     }
//     
//     public void Dispose()
//     {
//         _context.Dispose();
//     }
// }