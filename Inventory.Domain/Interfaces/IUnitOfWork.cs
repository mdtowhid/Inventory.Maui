using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Inventory.Domain.Interfaces;
using InventorySystem.Domain.Entities;

namespace Inventory.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
    IRepository<T> GetRepository<T>() where T : class;
}

// Specific repositories
public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetByCodeAsync(string code);
    Task<Product?> GetByBarcodeAsync(string barcode);
    Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold);
    Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
}

public interface IInventoryRepository : IRepository<InventoryItem>
{
    Task<InventoryItem?> GetByProductIdAsync(int productId);
    Task<int> GetAvailableQuantityAsync(int productId);
    Task<InventoryMovement> AdjustInventoryAsync(
        int productId,
        int quantity,
        MovementType movementType,
        int? referenceId = null,
        string? referenceType = null,
        string? notes = null,
        int? userId = null);
    Task<IEnumerable<InventoryMovement>> GetMovementsByProductIdAsync(int productId);
}

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByOrderNumberAsync(string orderNumber);
    Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
    Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerName);
    Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);
}

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ValidateCredentialsAsync(string username, string password);
}