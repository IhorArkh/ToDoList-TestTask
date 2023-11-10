using Microsoft.OpenApi.Models;

namespace ToDoList.Extensions;

public static class SwaggerServiceExtensions
{
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("jwt_auth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Name = "Authorization",
                Description = "Type 'Bearer ' and then your token",
            });
    
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "jwt_auth"
                        }
                    },
                    new string[] {}
                }
            });
        });

        return services;
    }
}