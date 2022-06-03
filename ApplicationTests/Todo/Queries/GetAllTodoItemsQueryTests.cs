using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Todo.Queries;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Utils;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace ApplicationTests.Todo.Queries;

public class GetAllTodoItemsQueryTests
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions =
        new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "GetAllTestDb")
            .Options;
    
    private GetAllTodoItemsQueryHandler _queryHandler;
    
    [OneTimeSetUp]
    public async Task Setup()
    {
        var context = new ApplicationDbContext(_dbContextOptions);
        await DatabaseSeeder.SeedAsync(context);

        _queryHandler = new GetAllTodoItemsQueryHandler(context);
    }
    
    [Test]
    public async Task GettingAllTodoItemsShouldReturnAllTodoItems()
    {
        // Arrange
        var context = new ApplicationDbContext(_dbContextOptions);
        var getAllQuery = new GetAllTodoItemsQuery();
        var cancellationToken = new CancellationToken();

        // Act
        var todoItems = await _queryHandler.Handle(getAllQuery, cancellationToken);

        // Assert
        var todoItemsList = todoItems.ToList();
        todoItemsList.Count.Should().Be(3);
        todoItemsList.Should().BeEquivalentTo(await context.TodoItems.ToListAsync(cancellationToken));
    }
}