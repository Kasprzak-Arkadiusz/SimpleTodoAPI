using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Validators;
using Application.Todo.ViewModels;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace Application.Todo.Commands;

public class PatchTodoItemCommand : IRequest<TodoItem>
{
    public int Id { get; }
    public JsonPatchDocument<PatchTodoItemViewModel> JsonPatch { get; }

    public PatchTodoItemCommand(int id, JsonPatchDocument<PatchTodoItemViewModel> jsonPatch)
    {
        Id = id;
        JsonPatch = jsonPatch;
    }
}

public class PatchTodoItemCommandHandler : IRequestHandler<PatchTodoItemCommand, TodoItem>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PatchTodoItemCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TodoItem> Handle(PatchTodoItemCommand command, CancellationToken cancellationToken)
    {
        var todoItem = await _context.TodoItems.FirstOrDefaultAsync(ti => ti.Id == command.Id, cancellationToken);

        if (todoItem == default)
        {
            throw new NotFoundException("Todo with given id cannot be found");
        }

        var todoItemVm = _mapper.Map<PatchTodoItemViewModel>(todoItem);

        command.JsonPatch.ApplyTo(todoItemVm);

        _mapper.Map(todoItemVm, todoItem);

        ValidateEntityState(todoItem);

        _context.TodoItems.Update(todoItem);
        await _context.SaveChangesAsync();

        return todoItem;
    }

    private static void ValidateEntityState(TodoItem todoItem)
    {
        try
        {
            var titleValidator = new StringValidator(todoItem.Title, nameof(todoItem.Title));
            titleValidator
                .NotEmpty()
                .MaxLength(TodoItemConstants.MaximumTitleLength);

            var descriptionValidator = new StringValidator(todoItem.Description, nameof(todoItem.Description));
            descriptionValidator
                .NotEmpty()
                .MaxLength(TodoItemConstants.MaximumDescriptionLength);
        }
        catch (BadRequestException e)
        {
            throw new UnprocessableRequestException(e.Message);
        }
    }
}