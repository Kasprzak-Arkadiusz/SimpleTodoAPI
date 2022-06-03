using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Mappings;
using Application.Todo.Commands;
using Application.Todo.ViewModels;
using AutoMapper;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

namespace ApplicationTests.Todo.Commands;

public class PatchTodoItemCommandHandlerTests
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions =
        new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "PatchTestDb")
            .Options;

    private PatchTodoItemCommandHandler _commandHandler;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var context = new ApplicationDbContext(_dbContextOptions);
        await DatabaseSeeder.SeedAsync(context);
        
        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        
        var mapper = mockMapper.CreateMapper();
        _commandHandler = new PatchTodoItemCommandHandler(context, mapper);
    }
    
    [Test]
    public async Task GivingInvalidIdShouldThrowNotFoundException()
    {
        // Arrange
        var context = new ApplicationDbContext(_dbContextOptions);
        var todoId = (await context.TodoItems.ToListAsync()).Last().Id + 1;
        var jsonPatch = new JsonPatchDocument<PatchTodoItemViewModel>();
        var patchCommand = new PatchTodoItemCommand(todoId, jsonPatch);
        var cancellationToken = new CancellationToken();

        // Act
        // Assert
        Assert.ThrowsAsync<NotFoundException>(async () => await _commandHandler.Handle(patchCommand, cancellationToken));
    }
    
    [Test]
    public async Task AddOperationShouldAssignNewValue()
    {
        // Arrange
        var context = new ApplicationDbContext(_dbContextOptions);
        var todoId = (await context.TodoItems.ToListAsync()).Last().Id;
        const string newTitleValue = "New title";
        const string newDescriptionValue = "New description";
        var operations = new List<Operation<PatchTodoItemViewModel>>
        {
            new("add", "/title", null, newTitleValue),
            new("add", "/description", null, newDescriptionValue)
        };
        var jsonPatch = new JsonPatchDocument<PatchTodoItemViewModel>(operations, new DefaultContractResolver());
        var patchCommand = new PatchTodoItemCommand(todoId, jsonPatch);
        var cancellationToken = new CancellationToken();

        // Act
        var editedTodo = await _commandHandler.Handle(patchCommand, cancellationToken);

        // Assert
        Assert.NotNull(editedTodo);
        editedTodo.Title.Should().Be(newTitleValue);
        editedTodo.Description.Should().Be(newDescriptionValue);
    }
    
    [Test]
    public async Task RemoveOperationShouldAssignDefaultValueForDeadline()
    {
        // Arrange
        var context = new ApplicationDbContext(_dbContextOptions);
        var todo = (await context.TodoItems.ToListAsync()).Last();
        var operations = new List<Operation<PatchTodoItemViewModel>>
        {
            new("remove", "/deadline", null)
        };
        var jsonPatch = new JsonPatchDocument<PatchTodoItemViewModel>(operations, new DefaultContractResolver());
        var patchCommand = new PatchTodoItemCommand(todo.Id, jsonPatch);
        var cancellationToken = new CancellationToken();

        // Act
        var editedTodo = await _commandHandler.Handle(patchCommand, cancellationToken);

        // Assert
        Assert.NotNull(editedTodo);
        Assert.Null(editedTodo.Deadline);
        editedTodo.Title.Should().Be(todo.Title);
        editedTodo.Description.Should().Be(todo.Description);
    }
    
    [Test]
    [TestCase("/title")]
    [TestCase("/description")]
    public async Task RemoveOperationShouldThrowExceptionWhenAppliedToRequiredProperties(string path)
    {
        // Arrange
        var context = new ApplicationDbContext(_dbContextOptions);
        var todo = (await context.TodoItems.ToListAsync()).Last();
        var operations = new List<Operation<PatchTodoItemViewModel>>
        {
            new("remove", path, null)
        };
        var jsonPatch = new JsonPatchDocument<PatchTodoItemViewModel>(operations, new DefaultContractResolver());
        var patchCommand = new PatchTodoItemCommand(todo.Id, jsonPatch);
        var cancellationToken = new CancellationToken();

        // Act
        // Assert
        Assert.ThrowsAsync<UnprocessableRequestException>(async () => await _commandHandler.Handle(patchCommand, cancellationToken));
    }
    
    [Test]
    public async Task ReplaceOperationShouldAssignNewValue()
    {
        // Arrange
        var context = new ApplicationDbContext(_dbContextOptions);
        var todo = (await context.TodoItems.ToListAsync()).Last();
        const string newTitleValue = "Replaced title";
        const string newDescriptionValue = "Replaced description";
        var newDeadlineValue = new DateTime(2024, 12, 24);
        var operations = new List<Operation<PatchTodoItemViewModel>>
        {
            new("replace", "/title", null, newTitleValue),
            new("replace", "/description", null, newDescriptionValue),
            new("replace", "/deadline", null, newDeadlineValue)
        };
        var jsonPatch = new JsonPatchDocument<PatchTodoItemViewModel>(operations, new DefaultContractResolver());
        var patchCommand = new PatchTodoItemCommand(todo.Id, jsonPatch);
        var cancellationToken = new CancellationToken();

        // Act
        var editedTodo = await _commandHandler.Handle(patchCommand, cancellationToken);

        // Assert
        Assert.NotNull(editedTodo);
        editedTodo.Title.Should().Be(newTitleValue);
        editedTodo.Description.Should().Be(newDescriptionValue);
        editedTodo.Deadline.Should().Be(newDeadlineValue);
    }
    
    [Test]
    public async Task CopyOperationShouldCopyValue()
    {
        // Arrange
        var context = new ApplicationDbContext(_dbContextOptions);
        var todo = (await context.TodoItems.ToListAsync()).Last();
        var operations = new List<Operation<PatchTodoItemViewModel>>
        {
            new("copy", "/description", "/title")
        };
        var jsonPatch = new JsonPatchDocument<PatchTodoItemViewModel>(operations, new DefaultContractResolver());
        var patchCommand = new PatchTodoItemCommand(todo.Id, jsonPatch);
        var cancellationToken = new CancellationToken();

        // Act
        var editedTodo = await _commandHandler.Handle(patchCommand, cancellationToken);

        // Assert
        Assert.NotNull(editedTodo);
        editedTodo.Title.Should().Be(todo.Title);
        editedTodo.Deadline.Should().Be(todo.Deadline);
        editedTodo.Description.Should().Be(todo.Title);
    }
    
    [Test]
    public async Task MoveOperationShouldMoveValue()
    {
        // Arrange
        var context = new ApplicationDbContext(_dbContextOptions);
        var todo = (await context.TodoItems.ToListAsync()).Last();
        var operations = new List<Operation<PatchTodoItemViewModel>>
        {
            new("move", "/description", "/deadline")
        };
        var jsonPatch = new JsonPatchDocument<PatchTodoItemViewModel>(operations, new DefaultContractResolver());
        var patchCommand = new PatchTodoItemCommand(todo.Id, jsonPatch);
        var cancellationToken = new CancellationToken();

        // Act
        var editedTodo = await _commandHandler.Handle(patchCommand, cancellationToken);

        // Assert
        Assert.NotNull(editedTodo);
        editedTodo.Title.Should().Be(todo.Title);
        editedTodo.Description.Should().Be(todo.Deadline.Value.ToString("yyyy-MM-ddTHH:mm:sszzzz"));
        editedTodo.Deadline.Should().Be(null);
    }
}