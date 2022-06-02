using Api.Extensions;
using Api.Middlewares;
using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Utils;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var infrastructureSettings = new InfrastructureSettings();
configuration.Bind(nameof(InfrastructureSettings), infrastructureSettings);
builder.Services.AddInfrastructure(infrastructureSettings);

builder.Services.AddApplication();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddSwaggerDocumentation();

builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ErrorHandlingMiddleware>();

// app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

if (infrastructureSettings.SeedWithCustomData)
{
    using var scope = app.Services.CreateScope();
    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DatabaseSeeder.SeedAsync(dataContext);
}

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dataContext.Database.Migrate();
}

app.Run();