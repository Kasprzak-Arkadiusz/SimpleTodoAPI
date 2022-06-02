using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Todo.Commands;

public class CreateTodoItemCommand : IRequest<TodoItem>
{
    public string Title { get; }
    public string Description { get; }
    public DateTime? Deadline { get; }

    public CreateTodoItemCommand(string title, string description, DateTime? deadline)
    {
        Title = title;
        Description = description;
        Deadline = deadline;
    }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, TodoItem>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TodoItem> Handle(CreateTodoItemCommand command, CancellationToken cancellationToken)
    {
        var todoItem = TodoItem.Create(command.Title, command.Description, command.Deadline);

        await _context.TodoItems.AddAsync(todoItem, cancellationToken);
        await _context.SaveChangesAsync();
        
        return todoItem;
    }
}