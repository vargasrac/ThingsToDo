using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Text.Json;
using ThingsToDo.Data;
using ThingsToDo.Models;

namespace ThingsToDo.BLService
{
    public class BLService : IBLService
    {
        private readonly ToDoTaskContext _context;

        public BLService(ToDoTaskContext context)
        {
            _context = context;
        }

        public async Task<string?> AddToDoTask(ToDoTask toDoTask)
        {
            if (toDoTask.StartTime != null || toDoTask.FinishTime != null)
            {
                return "ERROR:StartTime and FinishTime of the newly created ToDo must be null";
            }

            toDoTask.TimeStamp = DateTime.Now;

            _context.ToDoTask.Add(toDoTask);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }

        public async Task AddToDoTaskStart()
        {
            var toDoTask = new ToDoTask();

            toDoTask.StartTime = DateTime.Now;
            toDoTask.Description = await ProvideDefaultMessages();
            toDoTask.TimeStamp = DateTime.Now;

            _context.ToDoTask.Add(toDoTask);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteToDoTask(int id)
        {
            var toDoTask = await _context.ToDoTask.FindAsync(id);
  
            _context.ToDoTask.Remove(toDoTask);
            await _context.SaveChangesAsync();
        }

        public string GetHello()
        {
            throw new NotImplementedException();
        }

        public async Task<List<ToDoTask>> GetToDoTaskAll()
        {
            return await _context.ToDoTask.ToListAsync();
        }

        public async Task<ToDoTask?> GetToDoTaskById(int id)
        {
            return await _context.ToDoTask.FindAsync(id);
        }

        public async Task<List<ToDoTask>> GetToDoTaskByPage(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            return await _context.ToDoTask
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        }

        public async Task<List<ToDoTask>> GetToDoTaskByTimestamps(DateTime? from, DateTime? to)
        {
            IQueryable<ToDoTask> filteredItems = _context.ToDoTask.AsQueryable();

            if (from.HasValue)
            {
                filteredItems = filteredItems.Where(item => item.TimeStamp >= from.Value);
            }

            if (to.HasValue)
            {
                filteredItems = filteredItems.Where(item => item.TimeStamp <= to.Value);
            }

            return await filteredItems.ToListAsync();
        }

        public async Task<ToDoTask?> UpdateToDoTask(int id, ToDoTask toDoTask)
        {
            var existingToDoTask = await _context.ToDoTask.FindAsync(id);

            existingToDoTask.Description = toDoTask.Description;
            existingToDoTask.EstimatedDuration = toDoTask.EstimatedDuration;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return existingToDoTask;
        }

        public async Task<string?>  UpdateToDoTaskStart(int id)
        {
            var existingToDoTask = await _context.ToDoTask.FindAsync(id);

            if (existingToDoTask.StartTime != null && existingToDoTask.FinishTime == null)
            {
                return "ERROR:The todo task can not be started while the time measurement is in progress";
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }

        public async Task<string?> UpdateToDoTaskStop(int id)
        {
            var existingToDoTask = await _context.ToDoTask.FindAsync(id);

            if (existingToDoTask.StartTime == null)
            {
                return "ERROR:The To-Do task cannot be stopped until timer is started";
            }

            if (existingToDoTask.FinishTime != null)
            {
                return "ERROR:The To-Do task isfinished already";
            }

            return null;
        }

        async Task<bool> IBLService.ToDoTaskExists(int id)
        {
            return await _context.ToDoTask.AnyAsync(e => e.Id == id);
        }

        static async Task<string> ProvideDefaultMessages()
        {
            using HttpClient client = new();
            {
                var json = await client.GetStringAsync(
                    "https://api.chucknorris.io/jokes/random");

                if (json != null)
                {
                    ChuckNorrisJoke? joke = JsonSerializer.Deserialize<ChuckNorrisJoke>(json);
                    return joke?.value ?? string.Empty;
                }
                return string.Empty;
            }
        }
    }
    public class ChuckNorrisJoke
    {
        public required string value { get; set; }
    }
}
