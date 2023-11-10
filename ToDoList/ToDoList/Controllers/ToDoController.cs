using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.DTOs;
using ToDoList.Entities;

namespace ToDoList.Controllers;

public class ToDoController : BaseApiController
{
    private readonly DataContext _dataContext;

    public ToDoController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToDoDto>>> GetUserToDos()
    {
        var userId = User.FindFirst("UserId")?.Value;
        if (userId == default)
            return StatusCode(StatusCodes.Status500InternalServerError);

        var todoItems = await _dataContext.ToDos
            .Where(t => t.UserId == int.Parse(userId))
            .ToListAsync();

        var toDosToReturn = new List<ToDoDto>();
        foreach (var item in todoItems)
        {
            toDosToReturn.Add(new ToDoDto
            {
                Id = item.Id,
                ToDoName = item.ToDoName,
                IsCompleted = item.IsCompleted
            });
        }

        return Ok(toDosToReturn);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateToDoItem(CreateToDoDto createToDoDto)
    {
        var userId = User.FindFirst("UserId")?.Value;
        if (userId == default)
            return StatusCode(StatusCodes.Status500InternalServerError);

        var toDoItem = new ToDo
        {
            ToDoName = createToDoDto.ToDoName,
            IsCompleted = false,
            UserId = int.Parse(userId)
        };

        _dataContext.ToDos.Add(toDoItem);
        await _dataContext.SaveChangesAsync();

        return Ok();
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> CompleteToDo(int id)
    {
        var userId = User.FindFirst("UserId")?.Value;
        if (userId == default)
            return StatusCode(StatusCodes.Status500InternalServerError);

        var existingToDo = await _dataContext.ToDos
            .Where(t => t.UserId == int.Parse(userId) && t.Id == id)
            .FirstOrDefaultAsync();

        if (existingToDo == default)
            return NotFound();

        existingToDo.IsCompleted = true;
        await _dataContext.SaveChangesAsync();

        return NoContent();
    }
}