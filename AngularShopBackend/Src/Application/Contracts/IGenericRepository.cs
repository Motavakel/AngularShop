using Application.Contracts.Specification;
using Domain.Entities.Base;
using System.Linq.Expressions;

namespace Application.Contracts;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(T dto, CancellationToken cancellationToken);
    void Update(T entity);
    Task Delete(T entity, CancellationToken cancellationToken);

    Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);
    IQueryable<T> Where(Expression<Func<T, bool>> expression);
    Task<bool> AnyAsync(CancellationToken cancellationToken);

    //Specification
    Task<T> GetEntityWithSpec(ISpecification<T> spec, CancellationToken cancellationToken);
    Task<IReadOnlyList<T>> ListAsyncSpec(ISpecification<T> spec, CancellationToken cancellationToken);
    Task<int> CountAsyncSpec(ISpecification<T> spec, CancellationToken cancellationToken);
    Task<IReadOnlyList<T>> ToListAsync(CancellationToken cancellationToken);
}
