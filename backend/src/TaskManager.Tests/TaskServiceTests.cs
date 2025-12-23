using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Ports;

namespace TaskManager.Tests
{
    // Fake repository (in-memory) para pruebas unitarias del servicio
    class FakeTaskRepository : ITaskRepository
    {
        private readonly List<TaskItem> _store;
        private int _nextId;

        public FakeTaskRepository(IEnumerable<TaskItem>? seed = null)
        {
            _store = seed?.Select(s => new TaskItem
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                IsCompleted = s.IsCompleted,
                CreatedAt = s.CreatedAt
            }).ToList() ?? new List<TaskItem>();

            _nextId = (_store.Any() ? _store.Max(x => x.Id) + 1 : 1);
        }

        public Task<IEnumerable<TaskItem>> GetAllAsync() =>
            Task.FromResult<IEnumerable<TaskItem>>(_store.Select(x => Clone(x)));

        public Task<TaskItem?> GetByIdAsync(int id) =>
            Task.FromResult<TaskItem?>(_store.FirstOrDefault(x => x.Id == id) is TaskItem t ? Clone(t) : null);

        public Task<TaskItem> CreateAsync(TaskItem task)
        {
            var copy = Clone(task);
            copy.Id = _nextId++;
            if (copy.CreatedAt == default) copy.CreatedAt = DateTime.UtcNow;
            _store.Add(copy);
            return Task.FromResult(Clone(copy));
        }

        public Task<TaskItem?> UpdateAsync(TaskItem task)
        {
            var existing = _store.FirstOrDefault(x => x.Id == task.Id);
            if (existing == null) return Task.FromResult<TaskItem?>(null);

            existing.Title = task.Title;
            existing.Description = task.Description;
            existing.IsCompleted = task.IsCompleted;
            // keep CreatedAt
            return Task.FromResult(Clone(existing));
        }

        public Task<bool> DeleteAsync(int id)
        {
            var removed = _store.RemoveAll(x => x.Id == id) > 0;
            return Task.FromResult(removed);
        }

        private static TaskItem Clone(TaskItem t) =>
            new TaskItem
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                CreatedAt = t.CreatedAt
            };
    }

    public class TaskServiceTests
    {
        private TaskService CreateServiceWithSeed(out FakeTaskRepository repo)
        {
            var seed = new[]
            {
                new TaskItem { Id = 1, Title = "Tarea 1", Description = "Desc 1", IsCompleted = false, CreatedAt = DateTime.UtcNow },
                new TaskItem { Id = 2, Title = "Tarea 2", Description = "Desc 2", IsCompleted = true, CreatedAt = DateTime.UtcNow }
            };

            repo = new FakeTaskRepository(seed);
            return new TaskService(repo);
        }

        [Fact]
        public async Task GetAll_ReturnsAllItems()
        {
            var svc = CreateServiceWithSeed(out var repo);

            var all = (await svc.GetAllAsync()).ToList();

            Assert.Equal(2, all.Count);
            Assert.Contains(all, t => t.Title == "Tarea 1");
            Assert.Contains(all, t => t.Title == "Tarea 2");
        }

        [Fact]
        public async Task GetById_ReturnsItem_WhenExists()
        {
            var svc = CreateServiceWithSeed(out var repo);

            var t = await svc.GetByIdAsync(1);

            Assert.NotNull(t);
            Assert.Equal(1, t!.Id);
            Assert.Equal("Tarea 1", t.Title);
        }

        [Fact]
        public async Task GetById_ReturnsNull_WhenNotExists()
        {
            var svc = CreateServiceWithSeed(out var repo);

            var t = await svc.GetByIdAsync(999);

            Assert.Null(t);
        }

        [Fact]
        public async Task Create_AddsNewTask_AndAssignsId()
        {
            var svc = CreateServiceWithSeed(out var repo);

            var toCreate = new TaskItem { Title = "Nueva", Description = "X", IsCompleted = false };
            var created = await svc.CreateAsync(toCreate);

            Assert.NotNull(created);
            Assert.True(created.Id > 0);
            Assert.Equal("Nueva", created.Title);

            var all = (await svc.GetAllAsync()).ToList();
            Assert.Equal(3, all.Count); // 2 seed + 1
        }

        [Fact]
        public async Task Update_ModifiesExisting_ReturnsUpdated()
        {
            var svc = CreateServiceWithSeed(out var repo);

            var existing = (await svc.GetByIdAsync(1))!;
            existing.Title = "Cambiado";
            var updated = await svc.UpdateAsync(existing);

            Assert.NotNull(updated);
            Assert.Equal("Cambiado", updated!.Title);

            var check = await svc.GetByIdAsync(1);
            Assert.Equal("Cambiado", check!.Title);
        }

        [Fact]
        public async Task Update_ReturnsNull_WhenNotFound()
        {
            var svc = CreateServiceWithSeed(out var repo);

            var non = new TaskItem { Id = 999, Title = "X", Description = "", IsCompleted = false };
            var updated = await svc.UpdateAsync(non);

            Assert.Null(updated);
        }

        [Fact]
        public async Task Delete_Removes_WhenExists()
        {
            var svc = CreateServiceWithSeed(out var repo);

            var ok = await svc.DeleteAsync(1);
            Assert.True(ok);

            var remaining = (await svc.GetAllAsync()).ToList();
            Assert.Single(remaining);
            Assert.DoesNotContain(remaining, t => t.Id == 1);
        }

        [Fact]
        public async Task Delete_ReturnsFalse_WhenNotExists()
        {
            var svc = CreateServiceWithSeed(out var repo);

            var ok = await svc.DeleteAsync(999);
            Assert.False(ok);
        }
    }
}
