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
    
    [HttpPost("register")]
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
    
    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDto loginDto)
    {
        var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName == loginDto.UserName);
        if (user == null)
            return Unauthorized("Invalid username");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
                return Unauthorized("Invalid password");
        }

        return Ok("Successful login.");
    }
    
    private async Task<bool> UserExists(string userName)
    {
        return await _dataContext.Users.AnyAsync(u => u.UserName == userName.ToLower());
    }
}