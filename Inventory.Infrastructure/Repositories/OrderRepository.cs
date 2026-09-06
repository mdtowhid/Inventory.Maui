using Dapper;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Infrastructure.Data;

namespace InventorySystem.Infrastructure.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public async Task<Order?> GetByOrderNumberAsync(string orderNumber)
    {
        using var connection = _connectionFactory.CreateConnection();
        var query = "SELECT * FROM Orders WHERE OrderNumber = @OrderNumber AND IsDeleted = 0";
        return await connection.QueryFirstOrDefaultAsync<Order>(query, new { OrderNumber = orderNumber });
    }

    public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
    {
        using var connection = _connectionFactory.CreateConnection();
        var query = "SELECT * FROM Orders WHERE Status = @Status AND IsDeleted = 0 ORDER BY CreatedAt DESC";
        return await connection.QueryAsync<Order>(query, new { Status = (int)status });
    }

    public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerName)
    {
        using var connection = _connectionFactory.CreateConnection();
        var query = @"
            SELECT * FROM Orders 
            WHERE CustomerName LIKE @CustomerName 
              AND IsDeleted = 0 
            ORDER BY CreatedAt DESC";
        return await connection.QueryAsync<Order>(query, new { CustomerName = $"%{customerName}%" });
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
    {
        using var connection = _connectionFactory.CreateConnection();
        var query = "UPDATE Orders SET Status = @Status, UpdatedAt = GETDATE() WHERE Id = @Id AND IsDeleted = 0";
        var rowsAffected = await connection.ExecuteAsync(query, new { Id = orderId, Status = (int)newStatus });
        return rowsAffected > 0;
    }
}