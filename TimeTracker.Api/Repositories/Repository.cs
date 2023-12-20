using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Api.Context;

namespace TimeTracker.Api.Repositories;

public class Repository<T, TRequest, TResponse> : IRepository<T, TRequest, TResponse> 
    where T : class
    where TRequest : class
    where TResponse : class
{
    protected readonly TimeTrackerContext Context;
    protected readonly IMapper Mapper;

    public Repository(TimeTrackerContext context, IMapper mapper)
    {
        Context = context;
        Mapper = mapper;
    }

    public virtual async Task<TResponse?> GetByIdAsync(int id)
    {
        var entity = await Context.Set<T>().FindAsync(id);
        return Mapper.Map<TResponse>(entity);
    }

    public virtual async Task<List<TResponse>> GetAllAsync()
    {
        var entities = await Context.Set<T>().ToListAsync();
        return Mapper.Map<List<TResponse>>(entities);
    }

    public virtual async Task<TResponse> AddAsync(TRequest request)
    {
        var entity = Mapper.Map<T>(request);
        
        Context.Set<T>().Add(entity);
        await Context.SaveChangesAsync();
        
        var response = Mapper.Map<TResponse>(entity);
        return response;
    }

    public virtual async Task UpdateAsync(int id, TRequest request)
    {
        var entity = await Context.Set<T>().FindAsync(id);
        if(entity == null) return;
        
        Context.Entry(entity).CurrentValues.SetValues(request);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await Context.Set<T>().FindAsync(id);
        if (entity == null) return;
        
        Context.Set<T>().Remove(entity);
        await Context.SaveChangesAsync();
    }
}