using Api.Dtos.Todo;
using Application.Common.Models;
using Application.Todo.Commands;
using Application.Todo.Queries;
using Application.Todo.ViewModels;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// API Controller responsible for todoItems management
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
    /// Get todoItems as paginated list 
    /// </summary>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Number of todoItems on a page</param>
    /// <returns>TodoItems in the form of a paged list</returns>
    /// <response code="200">Successfully returned todoItems</response>
    /// <response code="400">Bad query parameters</response>
    [HttpGet("paged")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedList<TodoItem>>> GetTodoItemsAsPaginatedList(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
        {
            return BadRequest("PageNumber must be a positive number");
        }

        if (pageSize < 1)
        {
            return BadRequest("PageSize must be a positive number");
        }

        var paginatedResult = await Mediator.Send(new GetTodoItemsAsPaginatedListQuery(pageNumber, pageSize));
        return Ok(paginatedResult);
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
    /// <param name="id">TodoItem id</param> 
    /// <returns>Updated todoItem</returns>
    /// <response code="200">Successfully updated todoItem</response>
    /// <response code="400">Validation or logic error</response>
    /// <response code="404">TodoItem not found </response>
    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoItem>> UpdateTodoItem( int id, [FromBody] JsonPatchDocument<PatchTodoItemViewModel> jsonPatch)
    {
        var todoItem = await Mediator.Send(new PatchTodoItemCommand(id, jsonPatch));
        return Ok(todoItem);
    }

    /// <summary>
    /// Delete todoItem 
    /// </summary>
    /// <response code="200">Successfully deleted todoItem</response>
    /// <response code="404">TodoItem not found </response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteTodoItem(int id)
    {
        await Mediator.Send(new DeleteTodoItemCommand(id));
        return Ok();
    }
}