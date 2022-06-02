using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Todo.ViewModels;

public class PatchTodoItemViewModel : IMapFrom<TodoItem>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? Deadline { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<TodoItem, PatchTodoItemViewModel>();
        profile.CreateMap<PatchTodoItemViewModel, TodoItem>();
    }
}