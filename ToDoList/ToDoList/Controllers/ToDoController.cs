using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
}