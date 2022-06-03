using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Models;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Utils;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace ApplicationTests.Models;

public class PaginatedListTests
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions =
        new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "PaginatedListTestDb")
            .Options;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var context = new ApplicationDbContext(_dbContextOptions);
        await DatabaseSeeder.SeedAsync(context);
    }

    [Test]
    [TestCase(3, 3, 1)]
    [TestCase(4, 3, 1)]
    public async Task GivingValidDataShouldCreateValidPaginatedList(int pageNumber, int correctPageNumber,
        int pageSize)
    {
        // Arrange
        var context = new ApplicationDbContext(_dbContextOptions);
        var todoItemsQueryable = context.TodoItems.AsQueryable();
        var todoItemsList = await todoItemsQueryable.ToListAsync();

        // Act
        var paginatedList = await PaginatedList<TodoItem>.CreateAsync(todoItemsQueryable, pageNumber, pageSize);

        // Assert
        paginatedList.Items.Should().BeEquivalentTo(new List<TodoItem> { todoItemsList.Last() });
        paginatedList.PageNumber.Should().Be(correctPageNumber);
        var numberOfTodoItems = todoItemsList.Count;
        paginatedList.TotalPages.Should().Be(numberOfTodoItems);
        paginatedList.TotalCount.Should().Be((int)Math.Ceiling(numberOfTodoItems / (double)pageSize));
        paginatedList.HasPreviousPage.Should().Be(true);
        paginatedList.HasNextPage.Should().Be(false);
    }
}