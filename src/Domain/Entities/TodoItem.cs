using Domain.Common;

namespace Domain.Entities;

public class TodoItem : IEntity
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime? Deadline { get; private set; }

    private TodoItem(string title, string description, DateTime? deadline)
    {
        Title = title;
        Description = description;

        if (deadline.HasValue)
        {
            Deadline = deadline;
        }
    }

    public static TodoItem Create(string title, string description, DateTime? deadline = null)
    {
        return new TodoItem(title, description, deadline);
    }

    public void Update(string title, string description, DateTime? deadline = null)
    {
        if (!string.IsNullOrEmpty(title))
        {
            Title = title;
        }

        if (!string.IsNullOrEmpty(description))
        {
            Description = description;
        }

        Deadline = deadline;
    }
}