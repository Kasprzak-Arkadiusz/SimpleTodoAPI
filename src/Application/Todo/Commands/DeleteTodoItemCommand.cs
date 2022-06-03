using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Todo.Commands;

public class DeleteTodoItemCommand : IRequest<Unit>
{
    public int Id { get; }

    public DeleteTodoItemCommand(int id)
    {
        Id = id;
    }
}

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteTodoItemCommand command, CancellationToken cancellationToken)
    {
        var todoItemToDelete = await _context.TodoItems.FirstOrDefaultAsync(ti => ti.Id == command.Id, cancellationToken);

        if (todoItemToDelete == default)
        {
            throw new NotFoundException("Todo with given id cannot be found");
        }

        _context.TodoItems.Remove(todoItemToDelete);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}