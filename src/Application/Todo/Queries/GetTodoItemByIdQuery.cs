using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Todo.Queries;

public class GetTodoItemByIdQuery : IRequest<TodoItem>
{
    public int Id { get; }

    public GetTodoItemByIdQuery(int id)
    {
        Id = id;
    }
}

public class GetTodoItemByIdQueryHandler : IRequestHandler<GetTodoItemByIdQuery, TodoItem>
{
    private readonly IApplicationDbContext _context;

    public GetTodoItemByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TodoItem> Handle(GetTodoItemByIdQuery query, CancellationToken cancellationToken)
    {
        var todoItem = await _context.TodoItems.FindAsync(new object[] { query.Id }, cancellationToken: cancellationToken);

        if (todoItem is null)
        {
            throw new NotFoundException("Todo with given id cannot be found");
        }

        return todoItem;
    }
}