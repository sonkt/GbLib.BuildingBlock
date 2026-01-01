using GbLib.BuildingBlock.Domain.Entities;
using GbLib.BuildingBlock.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GbLib.BuildingBlock.Infrastructure.UnitOfWork;

/// <summary>
/// Represents a unit of work that coordinates changes to a database context.
/// This class ensures that changes are atomic and provides mechanisms for
/// transaction management such as beginning, committing, and rolling back transactions.
/// </summary>
/// <typeparam name="TDbContext">
/// The type of the database context. Must inherit from DbContext.
/// </typeparam>
public class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
{
    private readonly DbContext _dbContext;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    /// <summary>
    /// Asynchronously saves all changes made in the context to the underlying database.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous save operation. The task result contains
    /// the number of state entries written to the database.
    /// </returns>
    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }


    /// <summary>
    /// Asynchronously begins a database transaction for the current context.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation of starting a transaction.
    /// </returns>
    public async Task BeginTransactionAsync()
    {
        _transaction = await _dbContext.Database.BeginTransactionAsync();
    }

    /// <summary>
    /// Asynchronously commits the current database transaction, ensuring that all operations
    /// within the transaction are applied to the underlying database. If the commit fails,
    /// the transaction is rolled back to maintain data integrity.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation of committing the transaction.
    /// </returns>
    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            try
            {
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    /// <summary>
    /// Asynchronously rolls back the current database transaction, if one exists,
    /// and releases any associated resources.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous rollback operation.
    /// </returns>
    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public void Dispose()
    {
        if (_transaction != null)
        {
            _transaction.Dispose();
            _transaction = null;
        }

        _dbContext.Dispose();
    }
}