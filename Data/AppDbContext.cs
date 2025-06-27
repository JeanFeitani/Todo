namespace Todo.Data;

using Microsoft.EntityFrameworkCore;
using Todo.Models;

public class AppDbContext : DbContext
{
    public DbSet<TodoModel> Todos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=app.db;Cache=Shared");
        }

    }

}