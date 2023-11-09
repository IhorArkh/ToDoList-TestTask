namespace ToDoList.Entities;

public class ToDo
{
    public int Id { get; set; }

    public string ToDoName { get; set; }

    public bool IsCompleted { get; set; }

    public int UserId { get; set; }

    public User User { get; set; }
}