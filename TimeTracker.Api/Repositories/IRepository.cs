namespace TimeTracker.Api.Repositories;

public interface IRepository<T, TWrite, TRead> 
    where T : class
    where TWrite : class
    where TRead : class
{
    Task<TRead?> GetByIdAsync(int id);
    IQueryable<TRead> GetAll();
    Task<TRead> AddAsync(TWrite request);
    Task UpdateAsync(int id, TWrite request);
    Task DeleteAsync(int id);
}