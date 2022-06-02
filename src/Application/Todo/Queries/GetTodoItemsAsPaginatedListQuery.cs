using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.Todo.Queries;

public class GetTodoItemsAsPaginatedListQuery : IRequest<PaginatedList<TodoItem>>
{
    public int PageNumber { get; }
    public int PageSize { get; }

    public GetTodoItemsAsPaginatedListQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class GetTodoItemsAsPaginatedListQueryHandler
    : IRequestHandler<GetTodoItemsAsPaginatedListQuery, PaginatedList<TodoItem>>
{
    private readonly IApplicationDbContext _context;

    public GetTodoItemsAsPaginatedListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<TodoItem>> Handle(GetTodoItemsAsPaginatedListQuery query,
        CancellationToken cancellationToken)
    {
        var paginatedResult = await PaginatedList<TodoItem>
                .CreateAsync(_context.TodoItems.AsQueryable(), query.PageNumber, query.PageSize);

        return paginatedResult;
    }
}