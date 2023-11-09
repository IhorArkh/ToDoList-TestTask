using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.DTOs;
using ToDoList.Entities;

namespace ToDoList.Controllers;

public class AccountController : BaseApiController
{
    private readonly DataContext _dataContext;

    public AccountController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    [HttpPost]
    public async Task<ActionResult<User>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.UserName))
            return BadRequest("Username is taken");
        
        using var hmac = new HMACSHA512();
        
        var user = new User
        {
            UserName = registerDto.UserName,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };
        
        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync();

        return user;
    }
    
    private async Task<bool> UserExists(string userName)
    {
        return await _dataContext.Users.AnyAsync(u => u.UserName == userName.ToLower());
    }
}