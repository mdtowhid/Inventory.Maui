using Dapper;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Infrastructure.Data;

namespace InventorySystem.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public async Task<Product?> GetByCodeAsync(string code)
    {
        using var connection = _connectionFactory.CreateConnection();
        var query = "SELECT * FROM Products WHERE Code = @Code AND IsDeleted = 0";
        return await connection.QueryFirstOrDefaultAsync<Product>(query, new { Code = code });
    }

    public async Task<Product?> GetByBarcodeAsync(string barcode)
    {
        using var connection = _connectionFactory.CreateConnection();
        var query = "SELECT * FROM Products WHERE Barcode = @Barcode AND IsDeleted = 0";
        return await connection.QueryFirstOrDefaultAsync<Product>(query, new { Barcode = barcode });
    }

    public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold)
    {
        using var connection = _connectionFactory.CreateConnection();
        var query = @"
            SELECT p.* 
            FROM Products p
            INNER JOIN InventoryItems i ON p.Id = i.ProductId
            WHERE i.Quantity <= @Threshold 
              AND p.IsDeleted = 0
              AND p.IsActive = 1
            ORDER BY i.Quantity ASC";
        return await connection.QueryAsync<Product>(query, new { Threshold = threshold });
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
    {
        using var connection = _connectionFactory.CreateConnection();
        var query = @"
            SELECT * FROM Products 
            WHERE IsDeleted = 0 
              AND IsActive = 1
              AND (Name LIKE @SearchTerm 
                   OR Code LIKE @SearchTerm 
                   OR Barcode LIKE @SearchTerm
                   OR Description LIKE @SearchTerm)
            ORDER BY Name";
        return await connection.QueryAsync<Product>(query, new { SearchTerm = $"%{searchTerm}%" });
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        using var connection = _connectionFactory.CreateConnection();
        var query = "SELECT * FROM Products WHERE CategoryId = @CategoryId AND IsDeleted = 0 AND IsActive = 1";
        return await connection.QueryAsync<Product>(query, new { CategoryId = categoryId });
    }
}