using FluentValidation;
using Infrastructure.Persistence.Configurations.TodoItem;

namespace Api.Dtos.Todo;

public class CreateTodoItemDtoValidator : AbstractValidator<CreateTodoItemDto>
{
    public CreateTodoItemDtoValidator()
    {
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

public class CreateTodoItemDto
{
    public string Title { get; }
    public string Description { get; }
    public DateTime? Deadline { get; }

    public CreateTodoItemDto(string title, string description, DateTime? deadline)
    {
        Title = title;
        Description = description;
        Deadline = deadline;
    }
}