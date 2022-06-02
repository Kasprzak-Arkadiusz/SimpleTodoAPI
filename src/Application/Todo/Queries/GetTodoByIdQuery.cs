using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Todo.Queries;

public class GetTodoByIdQuery : IRequest<TodoItem>
{
    public int Id { get; }

    public GetTodoByIdQuery(int id)
    {
        Id = id;
    }
}

public class GetTodoByIdQueryHandler : IRequestHandler<GetTodoByIdQuery, TodoItem>
{
    private readonly IApplicationDbContext _context;

    public GetTodoByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TodoItem> Handle(GetTodoByIdQuery query, CancellationToken cancellationToken)
    {
        var todoItem = await _context.TodoItems.FindAsync(new object[] { query.Id }, cancellationToken: cancellationToken);

        if (todoItem is null)
        {
            throw new NotFoundException("Todo with given id cannot be found");
        }

        return todoItem;
    }
}