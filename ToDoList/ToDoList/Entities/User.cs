namespace ToDoList.Entities;

public class User
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public List<ToDo> ToDos { get; set; }
}