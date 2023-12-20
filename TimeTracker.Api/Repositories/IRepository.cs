namespace TimeTracker.Api.Repositories;

public interface IRepository<T, TRequest, TResponse> 
    where T : class
    where TRequest : class
    where TResponse : class
{
    Task<TResponse?> GetByIdAsync(int id);
    IQueryable<TResponse> GetAll();
    Task<TResponse> AddAsync(TRequest request);
    Task UpdateAsync(int id, TRequest request);
    Task DeleteAsync(int id);
}