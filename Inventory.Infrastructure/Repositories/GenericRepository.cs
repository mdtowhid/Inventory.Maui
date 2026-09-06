using Dapper;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Infrastructure.Data;
using System.Data;
using System.Linq.Expressions;

namespace InventorySystem.Infrastructure.Repositories;

public class GenericRepository<T> : IRepository<T> where T : class
{
    protected readonly IDbConnectionFactory _connectionFactory;
    protected readonly string _tableName;
    protected readonly string _keyColumnName;

    public GenericRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        _tableName = GetTableName();
        _keyColumnName = GetKeyColumnName();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        var query = $"SELECT * FROM {_tableName} WHERE {_keyColumnName} = @Id AND IsDeleted = 0";
        return await connection.QueryFirstOrDefaultAsync<T>(query, new { Id = id });
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        var query = $"SELECT * FROM {_tableName} WHERE IsDeleted = 0";
        return await connection.QueryAsync<T>(query);
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        // Simplified - for complex queries, use Dapper directly in derived repositories
        using var connection = _connectionFactory.CreateConnection();
        var query = $"SELECT * FROM {_tableName} WHERE IsDeleted = 0";
        return await connection.QueryAsync<T>(query);
    }

    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        var results = await FindAsync(predicate);
        return results.FirstOrDefault();
    }

    public virtual async Task<int> AddAsync(T entity)
    {
        using var connection = _connectionFactory.CreateConnection();
        var properties = GetProperties(entity);
        var columns = string.Join(", ", properties.Select(p => p.Name));
        var values = string.Join(", ", properties.Select(p => $"@{p.Name}"));
        var query = $"INSERT INTO {_tableName} ({columns}) VALUES ({values}); SELECT SCOPE_IDENTITY()";
        return await connection.ExecuteScalarAsync<int>(query, entity);
    }

    public virtual async Task<int> AddRangeAsync(IEnumerable<T> entities)
    {
        using var connection = _connectionFactory.CreateConnection();
        var count = 0;
        foreach (var entity in entities)
        {
            count += await AddAsync(entity);
        }
        return count;
    }

    public virtual async Task<bool> UpdateAsync(T entity)
    {
        using var connection = _connectionFactory.CreateConnection();
        var properties = GetProperties(entity).Where(p => !p.Name.Equals(_keyColumnName, StringComparison.OrdinalIgnoreCase));
        var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));
        var query = $"UPDATE {_tableName} SET {setClause} WHERE {_keyColumnName} = @{_keyColumnName} AND IsDeleted = 0";
        var rowsAffected = await connection.ExecuteAsync(query, entity);
        return rowsAffected > 0;
    }

    public virtual async Task<bool> UpdateRangeAsync(IEnumerable<T> entities)
    {
        var allUpdated = true;
        foreach (var entity in entities)
        {
            if (!await UpdateAsync(entity))
            {
                allUpdated = false;
            }
        }
        return allUpdated;
    }

    public virtual async Task<bool> DeleteAsync(T entity)
    {
        using var connection = _connectionFactory.CreateConnection();
        var query = $"UPDATE {_tableName} SET IsDeleted = 1 WHERE {_keyColumnName} = @{_keyColumnName}";
        var rowsAffected = await connection.ExecuteAsync(query, entity);
        return rowsAffected > 0;
    }

    public virtual async Task<bool> DeleteRangeAsync(IEnumerable<T> entities)
    {
        var allDeleted = true;
        foreach (var entity in entities)
        {
            if (!await DeleteAsync(entity))
            {
                allDeleted = false;
            }
        }
        return allDeleted;
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        using var connection = _connectionFactory.CreateConnection();
        var query = $"SELECT COUNT(*) FROM {_tableName} WHERE IsDeleted = 0";
        return await connection.ExecuteScalarAsync<int>(query);
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        var count = await CountAsync(predicate);
        return count > 0;
    }

    #region Helper Methods
    protected virtual string GetTableName()
    {
        var type = typeof(T);
        var tableAttribute = type.GetCustomAttributes(
            typeof(System.ComponentModel.DataAnnotations.Schema.TableAttribute), false)
            .FirstOrDefault() as System.ComponentModel.DataAnnotations.Schema.TableAttribute;

        return tableAttribute?.Name ?? type.Name + "s";
    }

    protected virtual string GetKeyColumnName()
    {
        var type = typeof(T);
        var keyProperty = type.GetProperties()
            .FirstOrDefault(p => p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase));

        return keyProperty?.Name ?? "Id";
    }

    protected virtual IEnumerable<System.Reflection.PropertyInfo> GetProperties(T entity)
    {
        return typeof(T).GetProperties()
            .Where(p => p.CanRead &&
                       !p.Name.Equals("IsDeleted", StringComparison.OrdinalIgnoreCase) &&
                       !p.Name.StartsWith("_") &&
                       p.Name != "CreatedAt" &&
                       p.Name != "UpdatedAt");
    }
    #endregion
}