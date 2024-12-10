using System.ComponentModel.DataAnnotations;

namespace ThingsToDo.Models
{
    /// <summary>
    /// Represents an element of todolist
    /// </summary>
    public class ToDoTask
    {
        /// <summary>
        /// Identifies an element of the todolist
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Desscripts the todo element
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Start time of the todo task
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Finish time of the todo task
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime? FinishTime { get; set; }

        /// <summary>
        /// Estimeted duration of the todo task
        /// </summary>
        [DataType(DataType.Duration)]
        public TimeSpan? EstimatedDuration { get; set; }
    }
}
