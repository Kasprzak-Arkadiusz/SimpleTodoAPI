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
    /// Get all todoItems 
    /// </summary>
    /// <returns>All todoItems</returns>
    /// <response code="200">Successfully returned todoItems</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetAllTodoItems()
    {
        var todoItems = await Mediator.Send(new GetAllTodoItemsQuery());
        return Ok(todoItems);
    }
    /// <summary>
    /// Get todoItem 
    /// </summary>
    /// <param name="id">TodoItem id</param> 
    /// <returns>TodoItem with given id</returns>
    /// <response code="200">Successfully returned todoItem</response>
    /// <response code="404">TodoItem not found </response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<TodoItem>> GetTodoItemById(int id)
    {
        var todoItem = await Mediator.Send(new GetTodoByIdQuery(id));
        return Ok(todoItem);
    }
    
}