using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThingsToDo.Data;
using ThingsToDo.Models;

namespace ThingsToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTasksController : ControllerBase
    {
        private readonly ToDoTaskContext _context;

        public ToDoTasksController(ToDoTaskContext context)
        {
            _context = context;
        }

        #region GET
        // GET: api/ToDoTasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoTask>>> GetToDoTask()
        {
            return await _context.ToDoTask.ToListAsync();
        }

        // GET: api/ToDoTasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoTask>> GetToDoTask(int id)
        {
            var toDoTask = await _context.ToDoTask.FindAsync(id);

            if (toDoTask == null)
            {
                return NotFound();
            }

            return toDoTask;
        }

        // GET: api/ToDoTasks/filter?from=2024-12-10&to=2024-12-11
        [HttpGet("filter")]
        public ActionResult<IEnumerable<ToDoTask>> GetToDoTaskByTimestamps([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var filteredItems = _context.ToDoTask.AsQueryable();

            if (from.HasValue)
            {
                filteredItems = filteredItems.Where(item => item.TimeStamp >= from.Value);
            }

            if (to.HasValue)
            {
                filteredItems = filteredItems.Where(item => item.TimeStamp <= to.Value);
            }

            return filteredItems.ToList();
        }
        #endregion

        #region Update
        // PUT: api/ToDoTasks/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateToDoTask(int id, ToDoTask toDoTask)
        {
            var existingToDoTask = await _context.ToDoTask.FindAsync(id);

            if (existingToDoTask == null)
            {
                return NotFound("Item not found");
            }

            existingToDoTask.Description = toDoTask.Description;
            existingToDoTask.EstimatedDuration = toDoTask.EstimatedDuration;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/ToDoTasks/start
        [HttpPut("update/start/{id}")]
        public async Task<IActionResult> StartToDoTask(int id)
        {
            var existingToDoTask = await _context.ToDoTask.FindAsync(id);

            if (existingToDoTask == null)
            {
                return NotFound("Item not found");
            }

            if (existingToDoTask.StartTime != null && existingToDoTask.FinishTime == null)
            {
                return BadRequest("ERROR:The todo task can not be started while the time measurement is in progress");
            }

            existingToDoTask.FinishTime = null;
            existingToDoTask.StartTime = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //PUT: api/ToDoTasks/start
        [HttpPut("update/stop/{id}")]
        public async Task<IActionResult> StopDoTask(int id)
        {
            var existingToDoTask = await _context.ToDoTask.FindAsync(id);

            if (existingToDoTask == null)
            {
                return NotFound("Item not found");
            }

            if (existingToDoTask.StartTime == null)
            {
                return BadRequest("ERROR:The To-Do task cannot be stopped until timer is started");
            }

            if (existingToDoTask.FinishTime != null)
            {
                return BadRequest("ERROR:The To-Do task isfinished already");
            }

            existingToDoTask.FinishTime = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        #endregion

        #region ADD
        // POST: api/ToDoTasks
        [HttpPost("add")]
        public async Task<ActionResult<ToDoTask>> AddToDoTask(ToDoTask toDoTask)
        {
            if (toDoTask.StartTime != null || toDoTask.FinishTime != null)
            {
                return BadRequest("ERROR:StartTime and FinishTime of the newly created ToDo must be null");
            }

            toDoTask.TimeStamp = DateTime.Now;

            _context.ToDoTask.Add(toDoTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToDoTask", new { id = toDoTask.Id }, toDoTask);
        }

        // POST: api/ToDoTasks/start
        [HttpPost("add/start")]
        public async Task<ActionResult<ToDoTask>> AddToDoTask()
        {
            var toDoTask = new ToDoTask();

            toDoTask.StartTime = DateTime.Now;
            toDoTask.Description  = await ProvideDefaultMessages();
            toDoTask.TimeStamp = DateTime.Now;

            _context.ToDoTask.Add(toDoTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToDoTask", new { id = toDoTask.Id }, toDoTask);
        }
        #endregion

        #region DELETE
        // DELETE: api/ToDoTasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoTask(int id)
        {
            var toDoTask = await _context.ToDoTask.FindAsync(id);
            if (toDoTask == null)
            {
                return NotFound();
            }

            _context.ToDoTask.Remove(toDoTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region Private
        private bool ToDoTaskExists(int id)
        {
            return _context.ToDoTask.Any(e => e.Id == id);
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
        #endregion
    }

    public class ChuckNorrisJoke
    {
        public required string value { get; set; }
    }

}
