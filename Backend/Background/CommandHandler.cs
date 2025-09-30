using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Backend.Services;
using Backend.Models;

public class ConsoleCommandListener : BackgroundService
{
    private readonly TaskService _taskService;

    public ConsoleCommandListener(TaskService taskService)
    {
        _taskService = taskService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Run(() =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                string[] parts = input.Trim().Split(' ', 2);
                string command = parts[0].ToLower();
                Guid? key;

                switch (command)
                {
                    case "list":
                        foreach (var task in _taskService.Tasks.Values)
                        {
                            Console.WriteLine($"{task.Id} - {task.Description} - {(task.IsCompleted ? "Completed" : "Uncompleted")}");
                        }
                        break;

                    case "add":
                        if (parts.Length < 2)
                        {
                            Console.WriteLine("Usage: add <description>");
                            break;
                        }
                        _taskService.addTask(parts[1]);
                        Console.WriteLine($"Task added: {parts[1]}");
                        break;
                    case "toggle":
                        if (parts.Length < 2)
                        {
                            Console.WriteLine("Usage: toggle <description>");
                            break;
                        }
                        key = _taskService.Tasks.FirstOrDefault(
                            kvp => kvp.Value.Description == parts[1]
                        ).Key;
                        if (key == Guid.Empty)
                        {
                            Console.WriteLine("Task not Found");
                            break;
                        }
                        _taskService.Tasks[key.Value].IsCompleted = !_taskService.Tasks[key.Value].IsCompleted;
                        break;
                    case "delete":
                        if (parts.Length < 2)
                        {
                            Console.WriteLine("Usage: delete <description>");
                            break;
                        }
                        key = _taskService.Tasks.FirstOrDefault(
                            kvp => kvp.Value.Description == parts[1]
                        ).Key;
                        if (key == Guid.Empty)
                        {
                            Console.WriteLine("Task not Found");
                            break;
                        }
                        _taskService.Tasks.Remove(key.Value);
                        break;
                    default:
                        Console.WriteLine($"Unknown command: {input}");
                        break;
                }
            }
        }, stoppingToken);
    }
}