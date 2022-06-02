using Application.Todo.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Controller for todos management
/// </summary>
public class TodoItemController : BaseController
{
    public TodoItemController(IMediator mediator) : base(mediator) { }
    
    /// <summary>
    /// Get all todos 
    /// </summary>
    /// <returns>All todos</returns>
    /// <response code="200">Successfully returned todos</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetAllTodoItems()
    {
        var todoItems = await Mediator.Send(new GetAllTodoItemsQuery());
        return Ok(todoItems);
    }
}