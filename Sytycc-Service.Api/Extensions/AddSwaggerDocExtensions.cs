
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;

namespace Sytycc_Service.Api.Extensions;

public static class SwaggerDocExtensions
{
public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
{
   services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "SYTYCC API", Version = "v1" });

        // Define the BearerAuth scheme
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter JWT with Bearer prefix into field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement 
        {
            { 
                new OpenApiSecurityScheme 
                { 
                    Reference = new OpenApiReference 
                    { 
                        Type = ReferenceType.SecurityScheme, 
                        Id = "Bearer" 
                    } 
                },
                Array.Empty<string>()
            } 
        });
    });


    return services;
}


}