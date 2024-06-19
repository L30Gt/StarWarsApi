using JediApi.Data;
using JediApi.Models;
using JediApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JediApi.Tests.Repositories
{
    public class JediRepositoryTests : IDisposable
    {
        private readonly JediContext _context;
        private readonly JediRepository _repository;

        public JediRepositoryTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<JediContext>();
            builder.UseInMemoryDatabase("TestDb")
                   .UseInternalServiceProvider(serviceProvider);

            _context = new JediContext(builder.Options);
            _repository = new JediRepository(_context);

            // Seed data for testing
            _context.Jedis.AddRange(
                new Jedi { Id = 1, Name = "Luke Skywalker", Strength = 100, Version = 1 },
                new Jedi { Id = 2, Name = "Han Solo", Strength = 80, Version = 1 }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task TestFindAll()
        {
            var jedis = await _repository.GetAllAsync();
            Assert.Equal(2, jedis.Count);
        }

        [Fact]
        public async Task TestFindByIdSuccess()
        {
            var jedi = await _repository.GetByIdAsync(2);

            Assert.NotNull(jedi);
            Assert.Equal(2, jedi.Id);
            Assert.Equal("Han Solo", jedi.Name);
            Assert.Equal(1, jedi.Version);
        }

        [Fact]
        public async Task TestAddJedi()
        {
            var newJedi = new Jedi { Name = "Yoda", Strength = 900 };
            var addedJedi = await _repository.AddAsync(newJedi);

            Assert.Equal("Yoda", addedJedi.Name);
            Assert.Equal(900, addedJedi.Strength);
            Assert.Equal(1, addedJedi.Version); // Version should be set to 1
        }

        [Fact]
        public async Task TestUpdateJedi()
        {
            var jedi = await _repository.GetByIdAsync(2);
            jedi.Name = "Han Solo Updated";
            jedi.Strength = 85;

            var updated = await _repository.UpdateAsync(jedi);
            Assert.True(updated);

            var updatedJedi = await _repository.GetByIdAsync(2);
            Assert.Equal("Han Solo Updated", updatedJedi.Name);
            Assert.Equal(85, updatedJedi.Strength);
        }

        [Fact]
        public async Task TestDeleteJedi()
        {
            var success = await _repository.DeleteAsync(1);
            Assert.True(success);

            var deletedJedi = await _repository.GetByIdAsync(1);
            Assert.Null(deletedJedi);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
