using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using System.Collections.Generic;
using Backend.Services;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {

        private readonly TaskService _taskService;

        public TasksController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public IActionResult getTasksList()
        {
            Console.WriteLine("Tasks fetched");
            return Ok(_taskService.Tasks.Values);
        }

        [HttpPost]
        public IActionResult AddTask([FromBody] TaskRequest req)
        {
            Guid id = _taskService.addTask(req.Description);
            string message = $"task #{id} added successfuly\n";
            Console.WriteLine(message);
            return Ok(new BasicResponse { Message = message });
        }

        [HttpPut("{id}")]
        public IActionResult ToggleComplition(Guid id)
        {
            bool current_completed = _taskService.Tasks[id].IsCompleted;
            string new_status = !current_completed ? "Completed" : "Uncompleted";
            _taskService.Tasks[id].IsCompleted = !current_completed;
            string message = $"task #{id} toggeled to {new_status} successfuly\n";
            Console.WriteLine(message);
            return Ok(new BasicResponse { Message = message});
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteComplition(Guid id)
        {
            _taskService.Tasks.Remove(id);
            string message = $"task #{id} deleted successfuly\n";
            Console.WriteLine(message);
            return Ok(new BasicResponse { Message = message });
        }

    }
}
