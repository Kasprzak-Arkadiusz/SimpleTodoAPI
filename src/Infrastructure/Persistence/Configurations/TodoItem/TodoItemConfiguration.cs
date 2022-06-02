using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.TodoItem;

public class TodoItemConfiguration : IEntityTypeConfiguration<Domain.Entities.TodoItem>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.TodoItem> builder)
    {
        builder.HasKey(ti => ti.Id);
        builder.Property(ti => ti.Title)
            .HasMaxLength(TodoItemConstants.MaximumTitleLength);
        builder.Property(ti => ti.Description)
            .HasMaxLength(TodoItemConstants.MaximumDescriptionLength);
        builder.Property(ti => ti.Deadline)
            .IsRequired(false);
    }
}