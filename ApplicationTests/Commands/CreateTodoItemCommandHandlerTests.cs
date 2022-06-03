using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Todo.Commands;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Utils;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace ApplicationTests.Commands;

public class CreateTodoItemCommandHandlerTests
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions =
        new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "PrimeDb")
            .Options;

    private CreateTodoItemCommandHandler _commandHandler;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var context = new ApplicationDbContext(_dbContextOptions);
        await DatabaseSeeder.SeedAsync(context);

        _commandHandler = new CreateTodoItemCommandHandler(context);
    }

    [Test]
    public async Task GivingCorrectDataShouldCreateNewTodoItem()
    {
        // Arrange
        var context = new ApplicationDbContext(_dbContextOptions);
        var expectedTodo = TodoItem.Create("Do something", "Do something important");
        var createCommand = new CreateTodoItemCommand(expectedTodo.Title, expectedTodo.Description, expectedTodo.Deadline);
        var cancellationToken = new CancellationToken();

        // Act
        var createdTodo = await _commandHandler.Handle(createCommand, cancellationToken);

        // Assert
        var results = await context.TodoItems.ToListAsync(cancellationToken);
        var maxTodoId = results.Select(ti => ti.Id).MaxBy(ti => ti);

        results.Count.Should().Be(4);
        createdTodo.Id.Should().Be(maxTodoId);
        createdTodo.Title.Should().Be(expectedTodo.Title);
        createdTodo.Description.Should().Be(expectedTodo.Description);
        createdTodo.Deadline.Should().Be(expectedTodo.Deadline);
    }
}