using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Inventory.Domain.Entities;
using System.Linq.Expressions;

namespace Inventory.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<int> AddAsync(T entity);
    Task<int> AddRangeAsync(IEnumerable<T> entities);
    Task<bool> UpdateAsync(T entity);
    Task<bool> UpdateRangeAsync(IEnumerable<T> entities);
    Task<bool> DeleteAsync(T entity);
    Task<bool> DeleteRangeAsync(IEnumerable<T> entities);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}

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
