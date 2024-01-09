using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimeTracker.UI.Windows.Shared.Interfaces.Repositories;

public interface IRepository<TModel>
{
    Task<(IEnumerable<TModel>, bool)> GetAllAsync();
    Task<(TModel, bool)> GetAsync(int id);
    Task<(TModel, bool)> AddAsync(TModel input);
    Task<(TModel, bool)> UpdateAsync(TModel input);
    Task<bool> DeleteAsync(int id);
}