using Dapper;
using Inventory.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Inventory.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnectionFactory _connectionFactory;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;
    private bool _disposed;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IDbConnection Connection
    {
        get
        {
            _connection ??= _connectionFactory.CreateConnection();
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            return _connection;
        }
    }

    public async Task BeginTransactionAsync()
    {
        if (_connection == null)
        {
            _connection = await _connectionFactory.CreateConnectionAsync();
        }
        _transaction = _connection.BeginTransaction();
    }

    public async Task CommitAsync()
    {
        if (_transaction != null)
        {
            await ((SqlTransaction)_transaction).CommitAsync();
            _transaction = null;
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await ((SqlTransaction)_transaction).RollbackAsync();
            _transaction = null;
        }
    }

    public IRepository<T> GetRepository<T>() where T : class
    {
        if (!_repositories.ContainsKey(typeof(T)))
        {
            var repository = new GenericRepository<T>(_connectionFactory);
            _repositories.Add(typeof(T), repository);
        }

        return (IRepository<T>)_repositories[typeof(T)];
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _transaction?.Dispose();
            _connection?.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}