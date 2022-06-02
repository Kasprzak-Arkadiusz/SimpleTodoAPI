using FluentValidation;
using Infrastructure.Persistence.Configurations.TodoItem;

namespace Api.Dtos.Todo;

public class UpdateTodoItemDtoValidator : AbstractValidator<UpdateTodoItemDto>
{
    public UpdateTodoItemDtoValidator()
    {
        RuleFor(dto => dto.Id)
            .GreaterThan(0).WithMessage("Id must be a positive number");
        RuleFor(dto => dto.Title)
            .NotNull()
            .MaximumLength(TodoItemConstants.MaximumTitleLength)
            .WithMessage(nameof(CreateTodoItemDto.Title) +
                         " must be less than {MaxLength} characters. {TotalLength} characters entered.");
        RuleFor(dto => dto.Description)
            .NotNull()
            .MaximumLength(TodoItemConstants.MaximumDescriptionLength)
            .WithMessage(nameof(CreateTodoItemDto.Description) +
                         " must be less than {MaxLength} characters. {TotalLength} characters entered.");
    }
}

public class UpdateTodoItemDto
{
    public int Id { get; }
    public string Title { get; }
    public string Description { get; }
    public DateTime? Deadline { get; }

    public UpdateTodoItemDto(int id, string title, string description, DateTime? deadline)
    {
        Id = id;
        Title = title;
        Description = description;
        Deadline = deadline;
    }
}