using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Application.Contracts;

public interface IUnitOWork
{
    Task<int> SaveAsync(CancellationToken cancellationToken);
    IGenericRepository<T> Repository<T>() where T : BaseEntity;
}