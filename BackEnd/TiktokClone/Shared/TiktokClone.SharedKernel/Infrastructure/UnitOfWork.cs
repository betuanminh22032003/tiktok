using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TiktokClone.SharedKernel.Application;
using TiktokClone.SharedKernel.Domain;

namespace TiktokClone.SharedKernel.Infrastructure;

/// <summary>
/// Unit of Work implementation with EF Core and domain event dispatching
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private readonly IMediator _mediator;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(DbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Persist changes first so domain event handlers observe committed state.
        var result = await _context.SaveChangesAsync(cancellationToken);

        // Dispatch domain events after successful save.
        await DispatchDomainEventsAsync(cancellationToken);

        return result;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
            
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        var entities = _context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();

        var domainEvents = entities
            .SelectMany(x => x.DomainEvents)
            .ToList();

        // Publish events first. Only clear them after successful publication so failed
        // publishers can surface the error and events can be retried if desired.
        try
        {
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }

            // Clear events after successful publish
            entities.ForEach(entity => entity.ClearDomainEvents());
        }
        catch
        {
            // Do NOT clear domain events if publishing failed.
            // Let exception bubble up so caller can decide retry/rollback behavior.
            throw;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
