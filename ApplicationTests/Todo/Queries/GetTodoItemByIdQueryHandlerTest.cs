using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Todo.Queries;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Utils;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace ApplicationTests.Todo.Queries;

public class GetTodoItemByIdQueryHandlerTest
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions =
        new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "GetByIdTestDb")
            .Options;
    
    private GetTodoItemByIdQueryHandler _queryHandler;
    
    [OneTimeSetUp]
    public async Task Setup()
    {
        var context = new ApplicationDbContext(_dbContextOptions);
        await DatabaseSeeder.SeedAsync(context);

        _queryHandler = new GetTodoItemByIdQueryHandler(context);
    }
    
    [Test]
    public async Task GettingTodoItemByIdShouldReturnTodoItemWithSpecificId()
    {
        // Arrange
        var context = new ApplicationDbContext(_dbContextOptions);
        var todoItem = (await context.TodoItems.ToListAsync()).Last();
        var getByIdQuery = new GetTodoItemByIdQuery(todoItem.Id);
        var cancellationToken = new CancellationToken();

        // Act
        var receivedTodoItem = await _queryHandler.Handle(getByIdQuery, cancellationToken);

        // Assert
        receivedTodoItem.Should().BeEquivalentTo(todoItem);
    }
    
    [Test]
    public async Task GettingByInvalidIdShouldThrowNotFoundException()
    {
        // Arrange
        var context = new ApplicationDbContext(_dbContextOptions);
        var todoId = (await context.TodoItems.ToListAsync()).Last().Id + 1;
        var deleteCommand = new GetTodoItemByIdQuery(todoId);
        var cancellationToken = new CancellationToken();

        // Act
        // Assert
        Assert.ThrowsAsync<NotFoundException>(async () => await _queryHandler.Handle(deleteCommand, cancellationToken));
    }
}