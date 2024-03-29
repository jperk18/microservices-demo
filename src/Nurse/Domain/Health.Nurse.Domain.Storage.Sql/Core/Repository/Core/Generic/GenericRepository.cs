﻿using Health.Nurse.Domain.Storage.Sql.Core.Databases.NurseDb;

namespace Health.Nurse.Domain.Storage.Sql.Core.Repository.Core.Generic;

public class GenericRepository<T> : GenericQueryRepository<T>, IGenericRepository<T> where T : class
{
#pragma warning disable CS0108, CS0114
    private readonly NurseDbContext _context;
#pragma warning restore CS0108, CS0114
    public GenericRepository(NurseDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<T> Add(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }

    public async Task<IEnumerable<T>> AddRange(IEnumerable<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
        return entities;
    }

    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }
}