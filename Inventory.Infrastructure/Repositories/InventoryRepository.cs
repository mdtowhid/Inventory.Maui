using Dapper;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Infrastructure.Data;
using System.Data;

namespace InventorySystem.Infrastructure.Repositories;

public class InventoryRepository : GenericRepository<InventoryItem>, IInventoryRepository
{
    public InventoryRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public async Task<InventoryItem?> GetByProductIdAsync(int productId)
    {
        using var connection = _connectionFactory.CreateConnection();
        var query = "SELECT * FROM InventoryItems WHERE ProductId = @ProductId";
        return await connection.QueryFirstOrDefaultAsync<InventoryItem>(query, new { ProductId = productId });
    }

    public async Task<int> GetAvailableQuantityAsync(int productId)
    {
        using var connection = _connectionFactory.CreateConnection();
        var query = "SELECT (Quantity - ReservedQuantity) FROM InventoryItems WHERE ProductId = @ProductId";
        var result = await connection.ExecuteScalarAsync<int?>(query, new { ProductId = productId });
        return result ?? 0;
    }

    public async Task<InventoryMovement> AdjustInventoryAsync(
        int productId,
        int quantity,
        MovementType movementType,
        int? referenceId = null,
        string? referenceType = null,
        string? notes = null,
        int? userId = null)
    {
        using var connection = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@ProductId", productId);
        parameters.Add("@Quantity", quantity);
        parameters.Add("@MovementType", (int)movementType);
        parameters.Add("@ReferenceId", referenceId);
        parameters.Add("@ReferenceType", referenceType);
        parameters.Add("@Notes", notes);
        parameters.Add("@UserId", userId);
        parameters.Add("@NewQuantity", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            "sp_UpdateInventory",
            parameters,
            commandType: CommandType.StoredProcedure);

        var movement = new InventoryMovement
        {
            ProductId = productId,
            Quantity = quantity,
            MovementType = movementType,
            ReferenceId = referenceId,
            ReferenceType = referenceType,
            Notes = notes,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            NewQuantity = parameters.Get<int>("@NewQuantity")
        };

        return movement;
    }

    public async Task<IEnumerable<InventoryMovement>> GetMovementsByProductIdAsync(int productId)
    {
        using var connection = _connectionFactory.CreateConnection();
        var query = @"
            SELECT * FROM InventoryMovements 
            WHERE ProductId = @ProductId 
            ORDER BY CreatedAt DESC";
        return await connection.QueryAsync<InventoryMovement>(query, new { ProductId = productId });
    }
}