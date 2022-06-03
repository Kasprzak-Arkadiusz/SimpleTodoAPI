using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Todo.Commands;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Utils;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace ApplicationTests.Todo.Commands;

public class DeleteTodoItemCommandHandlerTests
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions =
        new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "DeleteTestDb")
            .Options;

    private DeleteTodoItemCommandHandler _commandHandler;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var context = new ApplicationDbContext(_dbContextOptions);
        await DatabaseSeeder.SeedAsync(context);

        _commandHandler = new DeleteTodoItemCommandHandler(context);
    }

    [Test]
    public async Task GivingCorrectIdShouldDeleteCorrectTodoItem()
    {
        // Arrange
        var context = new ApplicationDbContext(_dbContextOptions);
        var todoId = (await context.TodoItems.ToListAsync()).Last().Id;
        var deleteCommand = new DeleteTodoItemCommand(todoId);
        var cancellationToken = new CancellationToken();

        // Act
        await _commandHandler.Handle(deleteCommand, cancellationToken);

        // Assert
        var results = await context.TodoItems.ToListAsync(cancellationToken);
        results.Count.Should().Be(2);

        var wasTodoFound = results.Any(ti => ti.Id == todoId);
        wasTodoFound.Should().Be(false);
    }
    
    [Test]
    public async Task GivingInvalidIdShouldThrowNotFoundException()
    {
        // Arrange
        var context = new ApplicationDbContext(_dbContextOptions);
        var todoId = (await context.TodoItems.ToListAsync()).Last().Id + 1;
        var deleteCommand = new DeleteTodoItemCommand(todoId);
        var cancellationToken = new CancellationToken();

        // Act
        // Assert
        Assert.ThrowsAsync<NotFoundException>(async () => await _commandHandler.Handle(deleteCommand, cancellationToken));
    }
}