﻿using System.Linq.Expressions;

namespace DietManagementSystem.Application.Common;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    IQueryable<T> GetAllAsync();
    IQueryable<T> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}
