using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Todo.Queries;

public class GetAllTodoItemsQuery : IRequest<IEnumerable<TodoItem>> { }

public class GetAllTodoItemsQueryHandler : IRequestHandler<GetAllTodoItemsQuery, IEnumerable<TodoItem>>
{
    private readonly IApplicationDbContext _context;

    public GetAllTodoItemsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoItem>> Handle(GetAllTodoItemsQuery query, CancellationToken cancellationToken)
    {
        var todoItems = await _context.TodoItems.ToListAsync(cancellationToken);
        return todoItems;
    }
}