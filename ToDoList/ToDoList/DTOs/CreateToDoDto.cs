using System.ComponentModel.DataAnnotations;

namespace ToDoList.DTOs;

public class CreateToDoDto
{
    [Required]
    public string ToDoName { get; set; }
}