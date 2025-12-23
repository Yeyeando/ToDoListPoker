using TaskManager.Domain.Entities;
using TaskManager.Domain.Ports;

namespace TaskManager.Application.Services;

public class TaskService
{
    private readonly ITaskRepository _repository;

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<TaskItem>> GetAllAsync() => _repository.GetAllAsync();
    public Task<TaskItem?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
    public Task<TaskItem> CreateAsync(TaskItem task) => _repository.CreateAsync(task);
    public Task<TaskItem?> UpdateAsync(TaskItem task) => _repository.UpdateAsync(task);
    public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
}
