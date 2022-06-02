using Application.Common.Interfaces;
using Domain.Entities;

namespace Infrastructure.Persistence.Utils;

public static class DatabaseSeeder
{

    public static async Task SeedAsync(IApplicationDbContext context)
    {
        if (context.TodoItems.Any())
        {
            return;
        }

        await SeedTodoItemsAsync(context);
        await context.SaveChangesAsync();
    }

    private static async Task SeedTodoItemsAsync(IApplicationDbContext context)
    {
        var today = DateTime.Today;
        
        var todoItems = new List<TodoItem>
        {
            TodoItem.Create("Homework", "Practice coding", today.AddDays(7)),
            TodoItem.Create("Shopping", "Buy vegetables at the grocery store", today.AddDays(2)),
            TodoItem.Create("Jira", "Complete tasks in Jira", today.AddDays(5))
        };

        await context.TodoItems.AddRangeAsync(todoItems);
    }
}