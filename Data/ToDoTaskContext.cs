using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ThingsToDo.Models;

namespace ThingsToDo.Data
{
    public class ToDoTaskContext : DbContext
    {
        public ToDoTaskContext (DbContextOptions<ToDoTaskContext> options)
            : base(options)
        {
        }

        public DbSet<ThingsToDo.Models.ToDoTask> ToDoTask { get; set; } = default!;
    }
}
