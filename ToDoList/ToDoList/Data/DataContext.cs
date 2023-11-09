﻿using Microsoft.EntityFrameworkCore;
using ToDoList.Entities;

namespace ToDoList.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> User { get; set; }

    public DbSet<ToDo> ToDos { get; set; }
}