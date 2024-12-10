using System;
using System.Collections.Generic;
using System.Linq;
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

        #region Generated public
        // GET: api/ToDoTasks?from=2024-10-10&to=2024-10-11
        [HttpGet]
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

        // PUT: api/ToDoTasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoTask(int id, ToDoTask toDoTask)
        {
            if (id != toDoTask.Id)
            {
                return BadRequest();
            }

            _context.Entry(toDoTask).State = EntityState.Modified;

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

        // POST: api/ToDoTasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ToDoTask>> PostToDoTask(ToDoTask toDoTask)
        {
            _context.ToDoTask.Add(toDoTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToDoTask", new { id = toDoTask.Id }, toDoTask);
        }

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
        #endregion
    }
}
