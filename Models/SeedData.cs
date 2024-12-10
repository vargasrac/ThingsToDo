using Microsoft.EntityFrameworkCore;
using ThingsToDo.Data;

namespace ThingsToDo.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ToDoTaskContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ToDoTaskContext>>()))
            {
                // Look for any movies.
                if (context.ToDoTask.Any())
                {
                    return;   // DB has been seeded
                }
                context.ToDoTask.AddRange(
                    new ToDoTask
                    { 
                        Description = "Having brakefast",
                    },
                    new ToDoTask
                    {
                        Description = "Having lunch",
                    },
                    new ToDoTask
                    {
                        Description = "Having snack",
                    },
                    new ToDoTask
                    {
                        Description = "Having dinner",
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
