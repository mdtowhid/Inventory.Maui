using Dapper;
using Inventory.Domain.Entities;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Infrastructure.Data;

namespace InventorySystem.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = _connectionFactory.CreateConnection();
        var query = "SELECT * FROM Users WHERE Username = @Username AND IsDeleted = 0";
        return await connection.QueryFirstOrDefaultAsync<User>(query, new { Username = username });
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        using var connection = _connectionFactory.CreateConnection();
        var query = "SELECT * FROM Users WHERE Email = @Email AND IsDeleted = 0";
        return await connection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
    }

    public async Task<bool> ValidateCredentialsAsync(string username, string password)
    {
        using var connection = _connectionFactory.CreateConnection();
        var query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND PasswordHash = @Password AND IsDeleted = 0 AND IsActive = 1";
        var count = await connection.ExecuteScalarAsync<int>(query, new { Username = username, Password = password });
        return count > 0;
    }
}