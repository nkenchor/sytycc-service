using System.Reflection;
using Sytycc_Service.Domain;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace Sytycc_Service.Api.Extensions;

public static class JwtExtension
{
     public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, string tokenKey)
    {
       services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
             options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    // Override the default behavior
                    context.HandleResponse();

                    // Create your custom response here
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    var responseObj = new AppExceptionResponse(new UnauthorizedException("Unauthorized"));
                    return context.Response.WriteAsync(JsonSerializer.Serialize(responseObj));
                }
            };
        });


        return services; // This return is crucial for chaining IServiceCollection methods.
    }
}
