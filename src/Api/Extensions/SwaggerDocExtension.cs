using System.Reflection;
using Microsoft.OpenApi.Models;

namespace Api.Extensions;

public static class SwaggerDocExtension
{
    public static void AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Simple Todo Api",
                    Description = "An ASP.NET Web API for Simple Todo App",
                });
            
            opt.SupportNonNullableReferenceTypes();
            
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }
}