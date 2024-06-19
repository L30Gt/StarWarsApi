using JediApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JediApi.Data
{
    public class JediContext(DbContextOptions<JediContext> options) : DbContext(options)
    {
        public DbSet<Jedi> Jedis { get; set; }
    }
}
