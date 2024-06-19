using JediApi.Models;

namespace JediApi.Repositories
{
    public interface IJediRepository
    {
        Task<Jedi> GetByIdAsync(int id);
        Task<List<Jedi>> GetAllAsync();
        Task<Jedi> AddAsync(Jedi jedi);
        Task<bool> UpdateAsync(Jedi jedi);
        Task<bool> DeleteAsync(int id);
    }
}
