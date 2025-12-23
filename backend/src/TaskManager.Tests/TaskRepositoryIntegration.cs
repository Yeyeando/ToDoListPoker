using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

using TaskManager.Infrastructure.Data;
using TaskManager.Infrastructure.Repositories;
using TaskManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Tests
{
    public class TaskRepositoryIntegrationTests : IDisposable
    {
        private readonly TaskDbContext _context;
        private readonly TaskRepository _repo;

        public TaskRepositoryIntegrationTests()
        {
            // DB InMemory con nombre único por test-run
            var opts = new DbContextOptionsBuilder<TaskDbContext>()
                .UseInMemoryDatabase($"TaskRepoTests_{Guid.NewGuid()}")
                .Options;

            _context = new TaskDbContext(opts);
            _repo = new TaskRepository(_context);
        }

        [Fact]
        public async Task CreateAndGetById_Works()
        {
            var t = new TaskItem { Title = "Repo create", Description = "desc" };

            var created = await _repo.CreateAsync(t);
            Assert.True(created.Id > 0);

            var fetched = await _repo.GetByIdAsync(created.Id);
            Assert.NotNull(fetched);
            Assert.Equal("Repo create", fetched!.Title);
        }

        [Fact]
        public async Task GetAll_ReturnsInsertedItems()
        {
            await _repo.CreateAsync(new TaskItem { Title = "A" });
            await _repo.CreateAsync(new TaskItem { Title = "B" });

            var all = (await _repo.GetAllAsync()).ToList();
            Assert.True(all.Count >= 2);
            Assert.Contains(all, x => x.Title == "A");
            Assert.Contains(all, x => x.Title == "B");
        }

        [Fact]
        public async Task Update_ChangesEntity()
        {
            var created = await _repo.CreateAsync(new TaskItem { Title = "Before" });
            created.Title = "After";

            var updated = await _repo.UpdateAsync(created);
            Assert.NotNull(updated);
            Assert.Equal("After", updated!.Title);

            var fetched = await _repo.GetByIdAsync(created.Id);
            Assert.Equal("After", fetched!.Title);
        }

        [Fact]
        public async Task Delete_RemovesEntity()
        {
            var created = await _repo.CreateAsync(new TaskItem { Title = "ToDelete" });
            var ok = await _repo.DeleteAsync(created.Id);
            Assert.True(ok);

            var fetched = await _repo.GetByIdAsync(created.Id);
            Assert.Null(fetched);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
