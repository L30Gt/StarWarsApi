using JediApi.Models;
using JediApi.Repositories;
using JediApi.Services;
using Moq;

namespace JediApi.Tests.Services
{
    public class JediServiceTests
    {
        // não mexer
        private readonly JediService _service;
        private readonly Mock<IJediRepository> _repositoryMock;

        public JediServiceTests()
        {
            // não mexer
            _repositoryMock = new Mock<IJediRepository>();
            _service = new JediService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetById_Success()
        {
            int jediId = 1;
            Jedi jedi = new Jedi { Id = 1, Name = "AAA", Strength = 100, Version = 1};

            _repositoryMock.Setup(r => r.GetByIdAsync(jediId)).ReturnsAsync(jedi);

            var result = await _service.GetByIdAsync(jediId);
            
            Assert.Equal(jedi, result);
        }

        [Fact]
        public async Task GetById_NotFound()
        {
            int jediId = 1;

            _repositoryMock.Setup(r => r.GetByIdAsync(jediId)).ReturnsAsync((Jedi)null);

            var result = await _service.GetByIdAsync(jediId);
            Assert.Null(result);

            //await Assert.ThrowsAsync<Exception>(() => _service.GetByIdAsync(jediId));
        }

        [Fact]
        public async Task GetAll()
        {
            List<Jedi> jedis = new List<Jedi>()
            {
                new Jedi { Id = 1, Name = "AAA", Strength = 100, Version = 1 },
                new Jedi { Id = 2, Name = "CCC", Strength = 75, Version = 1 }
            };

            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(jedis);
            var result = await _service.GetAllAsync();

            Assert.Equal(result, jedis);
            Assert.Equal(result.Count, jedis.Count);

        }

        [Fact]
        public async Task AddAsync()
        {
            Jedi newJedi = new Jedi { Id = 1, Name = "Yoda", Strength = 900, Version = 1};
            Jedi jedi = new Jedi { Id = newJedi.Id ,Name = newJedi.Name, Strength = newJedi.Strength, Version = newJedi.Version};

            _repositoryMock.Setup(r => r.AddAsync(jedi));
            await _service.AddAsync(newJedi);

            Assert.Equal(newJedi.Name, jedi.Name);
            Assert.Equal(newJedi.Strength, jedi.Strength);

        }

        [Fact]
        public async Task UpdateAsync()
        {
            Jedi existingJedi = new Jedi { Id = 1, Name = "AAA", Strength = 100 };
            Jedi updatedJedi = new Jedi { Id = 1, Name = "Yoda", Strength = 300 };

            _repositoryMock.Setup(r => r.UpdateAsync(existingJedi)).ReturnsAsync(true);

            var result = await _service.UpdateAsync(updatedJedi);

            Assert.Equal(updatedJedi.Name, existingJedi.Name);
            Assert.Equal(updatedJedi.Strength, existingJedi.Strength);

        }

        [Fact]
        public async Task DeleteAsync()
        {
            int jediId = 1;
            _repositoryMock.Setup(repo => repo.DeleteAsync(jediId)).ReturnsAsync(true);

            var result = await _service.DeleteAsync(jediId);

            Assert.True(result);

            _repositoryMock.Verify(repo => repo.DeleteAsync(jediId), Times.Once);

        }

    }
}
