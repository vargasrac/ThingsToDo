using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ThingsToDo.BLService;
using ThingsToDo.Data;
using ThingsToDo.Models;

namespace ThingsToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTasksController : ControllerBase
    {
        [Required]
        private readonly IBLService _blService;

        public ToDoTasksController(IBLService bLService)
        {
            _blService = bLService;
        }

        #region GET
        // GET: api/ToDoTasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoTask>>> GetToDoTask()
        {
            return await _blService.GetToDoTaskAll();
        }

        // GET: api/ToDoTasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoTask>> GetToDoTask(int id)
        {
            ToDoTask? toDoTasks = await _blService.GetToDoTaskById(id);

            if (toDoTasks == null)
            {
                return NotFound();
            }

            return toDoTasks;
        }

        // GET: api/ToDoTasks/page?pageNumber=3&pageSize=4
        [HttpGet("page")]
        public async Task<ActionResult<IEnumerable<ToDoTask>>> GetToDoTasksByPage(int pageNumber = 1, int pageSize = 10)
        {
            var toDoTasks = await _blService.GetToDoTaskByPage(pageNumber, pageSize);

            return toDoTasks;
        }

        // GET: api/ToDoTasks/filter?from=2024-12-10&to=2024-12-11
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ToDoTask>>> GetToDoTaskByTimestamps([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            return await _blService.GetToDoTaskByTimestamps(from, to);
        }
        #endregion

        #region Update
        // PUT: api/ToDoTasks/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateToDoTask(int id, ToDoTask toDoTask)
        {
            if (!await _blService.ToDoTaskExists(id))
            {
                return NotFound("item not found");
            }

            await _blService.UpdateToDoTask(id, toDoTask);

            return NoContent();
        }

        // PUT: api/ToDoTasks/start
        [HttpPut("update/start/{id}")]
        public async Task<IActionResult> UpdateToDoTaskStart(int id)
        {
            if (!await _blService.ToDoTaskExists(id))
            {
                return NotFound("item not found");
            }

            string? errMsg = await _blService.UpdateToDoTaskStart(id);

            if (errMsg != null)
            {
                return BadRequest(errMsg);
            }



            return NoContent();
        }

        //PUT: api/ToDoTasks/start
        [HttpPut("update/stop/{id}")]
        public async Task<IActionResult> StopDoTask(int id)
        {
            if (!await _blService.ToDoTaskExists(id))
            {
                return NotFound("item not found");
            }

            string? errMsg = await _blService.UpdateToDoTaskStop(id);

            if (errMsg != null)
            {
                return BadRequest(errMsg);
            }

            return NoContent();
        }
        #endregion

        #region ADD
        // POST: api/ToDoTasks
        [HttpPost("add")]
        public async Task<ActionResult<ToDoTask>> AddToDoTask(ToDoTask toDoTask)
        {
            string? errMsg = await _blService.AddToDoTask(toDoTask);

            if (errMsg != null)
            {
                return BadRequest(errMsg);
            }

            return CreatedAtAction("Created", null);
        }

        // POST: api/ToDoTasks/start
        [HttpPost("add/start")]
        public async Task<ActionResult<ToDoTask>> AddToDoTaskStart()
        {
            await _blService.AddToDoTaskStart();

            return CreatedAtAction("GetToDoTask created", null);
        }
        #endregion

        #region DELETE
        // DELETE: api/ToDoTasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoTask(int id)
        {
            if (!await _blService.ToDoTaskExists(id))
            {
                return NotFound("item not found");
            }

            await _blService.DeleteToDoTask(id);

            return NoContent();
        }
        #endregion
    }
}
