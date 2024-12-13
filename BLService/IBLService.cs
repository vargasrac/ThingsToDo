using Microsoft.AspNetCore.Mvc;
using ThingsToDo.Models;

namespace ThingsToDo.BLService
{
    public interface IBLService
    {
        
        string GetHello();
        Task<List<ToDoTask>> GetToDoTaskAll();
        Task<ToDoTask?> GetToDoTaskById(int id);
        Task<List<ToDoTask>> GetToDoTaskByPage(int pageNumber, int pageSize);
        Task<List<ToDoTask>> GetToDoTaskByTimestamps(DateTime? from, DateTime? to);
        Task<ToDoTask?> UpdateToDoTask(int id, ToDoTask toDoTask);
        Task<string?> UpdateToDoTaskStart(int id);
        Task<string?> UpdateToDoTaskStop(int id);
        Task<string?> AddToDoTask(ToDoTask toDoTask);
        Task AddToDoTaskStart();
        Task DeleteToDoTask(int id);
        Task<bool> ToDoTaskExists(int id);
    }
}
