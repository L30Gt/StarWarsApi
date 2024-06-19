using JediApi.Data;
using JediApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JediApi.Repositories
{
    public class JediRepository(JediContext context) : IJediRepository
    {
        public async Task<Jedi> GetByIdAsync(int id)
        {
            return await context.Jedis.FindAsync(id);
        }

        public async Task<List<Jedi>> GetAllAsync()
        {
            return await context.Jedis.ToListAsync();
        }

        public async Task<Jedi> AddAsync(Jedi jedi)
        {
            jedi.Version = 1;
            context.Jedis.Add(jedi);
            await context.SaveChangesAsync();
            return jedi;
        }

        public async Task<bool> UpdateAsync(Jedi jedi)
        {
            context.Entry(jedi).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JediExists(jedi.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var jedi = await context.Jedis.FindAsync(id);
            if (jedi == null)
            {
                return false;
            }

            context.Jedis.Remove(jedi);
            await context.SaveChangesAsync();
            return true;
        }

        private bool JediExists(int id)
        {
            return context.Jedis.Any(e => e.Id == id);
        }
    }
}
