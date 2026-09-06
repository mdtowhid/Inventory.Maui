using InventorySystem.Domain.Interfaces;

namespace InventorySystem.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
    IRepository<T> GetRepository<T>() where T : class;
}