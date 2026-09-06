using Inventory.Domain.Interfaces;

namespace Inventory.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
    IRepository<T> GetRepository<T>() where T : class;
}