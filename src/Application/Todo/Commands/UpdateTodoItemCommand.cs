using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Todo.Commands;

public class UpdateTodoItemCommand : IRequest<TodoItem>
{
    public int Id { get; }
    public string Title { get; }
    public string Description { get; }
    public DateTime? Deadline { get; }

    public UpdateTodoItemCommand(int id, string title, string description, DateTime? deadline)
    {
        Id = id;
        Title = title;
        Description = description;
        Deadline = deadline;
    }
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand, TodoItem>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TodoItem> Handle(UpdateTodoItemCommand command, CancellationToken cancellationToken)
    {
        var updatedTodoItem = await _context.TodoItems.FirstOrDefaultAsync(ti => ti.Id == command.Id, cancellationToken);

        if (updatedTodoItem == default)
        {
            throw new NotFoundException("Todo with given id cannot be found");
        }
        
        updatedTodoItem.Update(command.Title, command.Description, command.Deadline);
        await _context.SaveChangesAsync();

        return updatedTodoItem;
    }
}