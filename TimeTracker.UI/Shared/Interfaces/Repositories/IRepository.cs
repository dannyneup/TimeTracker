using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimeTracker.UI.Windows.Shared.Interfaces.Repositories;

public interface IRepository<TRequest, TResponse>
{
    Task<(IEnumerable<TResponse>, bool)> GetAllAsync();
    Task<(TResponse, bool)> GetAsync(int id);
    Task<(TResponse, bool)> AddAsync(TRequest request);
    Task<(TResponse, bool)> UpdateAsync(int id, TRequest request);
    Task<bool> DeleteAsync(int id);
}