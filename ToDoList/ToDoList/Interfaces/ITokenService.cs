using ToDoList.Entities;

namespace ToDoList.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}