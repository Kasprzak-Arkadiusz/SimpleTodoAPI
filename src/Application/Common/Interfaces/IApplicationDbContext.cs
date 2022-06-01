using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<TodoItem> TodoItems { get; set; }
    Task<int> SaveChangesAsync();
}