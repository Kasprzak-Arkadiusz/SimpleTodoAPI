using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Todo.Queries;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Utils;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace ApplicationTests.Todo.Queries;

public class GetTodoItemAsPaginatedListQueryTests
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions =
        new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "GetAsPaginatedListTestDb")
            .Options;

    private GetTodoItemsAsPaginatedListQueryHandler _queryHandler;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var context = new ApplicationDbContext(_dbContextOptions);
        await DatabaseSeeder.SeedAsync(context);

        _queryHandler = new GetTodoItemsAsPaginatedListQueryHandler(context);
    }

    [Test]
    public async Task GettingTodoItemsAsPaginatedListShouldReturnPaginatedList()
    {
        // Arrange
        var context = new ApplicationDbContext(_dbContextOptions);
        const int pageNumber = 2;
        const int pageSize = 1;
        var todoItems =
            await PaginatedList<TodoItem>.CreateAsync(context.TodoItems.AsQueryable(), pageNumber, pageSize);
        var getAllQuery = new GetTodoItemsAsPaginatedListQuery(pageNumber, pageSize);
        var cancellationToken = new CancellationToken();

        // Act
        var todoItemsPaginatedList = await _queryHandler.Handle(getAllQuery, cancellationToken);

        // Assert
        todoItemsPaginatedList.Should().BeEquivalentTo(todoItems);
    }
}