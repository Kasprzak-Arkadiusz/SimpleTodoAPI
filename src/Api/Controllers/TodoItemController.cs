using Api.Dtos.Todo;
using Application.Todo.Commands;
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoItem>> GetTodoItemById(int id)
    {
        var todoItem = await Mediator.Send(new GetTodoItemByIdQuery(id));
        return Ok(todoItem);
    }

    /// <summary>
    /// Create todoItem 
    /// </summary>
    /// <returns>Created todoItem</returns>
    /// <response code="201">Successfully created todoItem</response>
    /// <response code="400">Validation or logic error</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TodoItem>> CreateTodoItem([FromBody] CreateTodoItemDto dto)
    {
        var todoItem = await Mediator.Send(new CreateTodoItemCommand(dto.Title, dto.Description, dto.Deadline));
        return CreatedAtRoute(null, todoItem);
    }

    /// <summary>
    /// Update todoItem 
    /// </summary>
    /// <returns>Updated todoItem</returns>
    /// <response code="200">Successfully updated todoItem</response>
    /// <response code="400">Validation or logic error</response>
    /// <response code="404">TodoItem not found </response>
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoItem>> UpdateTodoItem([FromBody] UpdateTodoItemDto dto)
    {
        var todoItem = await Mediator.Send(new UpdateTodoItemCommand(dto.Id, dto.Title, dto.Description, dto.Deadline));
        return Ok(todoItem);
    }
}