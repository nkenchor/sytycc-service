using Microsoft.AspNetCore.Mvc;
using Sytycc_Service.Domain;
using Serilog;
using Sytycc_Service.Infrastructure;
using Sytycc_Service.Api.Extensions;
using serviceProvider = Sytycc_Service.Infrastructure.ServiceProvider;
using Sytycc_Service.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

//Check for service configuration

if (serviceProvider.CheckConfiguration(@"../sytycc-service.env") == false) return;

//Add environment variables to the global config
builder.Configuration.AddEnvironmentVariables();
//Map configuration to global class
serviceProvider.MapConfiguration(builder.Configuration);

// Suppress automatic model validation
builder.Services.Configure<ApiBehaviorOptions>(options =>{options.SuppressModelStateInvalidFilter = true;});

// Add Serilog
builder.Host.AddSerilog();

// Add services
builder.Services.AddEndpointsApiExplorer();
//Auth
builder.Services.AddJwtAuthentication(Service.TokenKey);
builder.Services.AddSwaggerDocumentation();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHeaderPropagation(options => options.Headers.Add("X-Correlation-Id"));
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddSingleton<serviceProvider>();

builder.Services.AddTransient<ICourseService,CourseService>();
builder.Services.AddTransient<ICourseRepository,CourseRepository>();
builder.Services.AddTransient<ICourseValidationService,CourseValidationService>();
builder.Services.AddTransient<IFacilitatorService,FacilitatorService>();
builder.Services.AddTransient<IFacilitatorRepository,FacilitatorRepository>();
builder.Services.AddTransient<IFacilitatorValidationService,FacilitatorValidationService>();
builder.Services.AddTransient<IParticipantService,ParticipantService>();
builder.Services.AddTransient<IParticipantRepository,ParticipantRepository>();
builder.Services.AddTransient<IParticipantValidationService,ParticipantValidationService>();
builder.Services.AddTransient<IRegistrationService,RegistrationService>();
builder.Services.AddTransient<IRegistrationRepository,RegistrationRepository>();
builder.Services.AddTransient<IRegistrationValidationService,RegistrationValidationService>();
builder.Services.AddTransient<IPaymentService,PaymentService>();
builder.Services.AddTransient<IUserRepository,UserRepository>();
builder.Services.AddTransient<IUserService,UserService>();
builder.Services.AddTransient<IUserValidationService,UserValidationService>();
builder.Services.AddTransient<IPasswordProvider,PasswordProviderService>();
builder.Services.AddTransient<IEmailService,EmailService>();

builder.Services.AddSingleton<FacilitatorSeeder>();
builder.Services.AddSingleton<CourseSeeder>();
builder.Services.AddSingleton<UserSeeder>();
builder.Services.AddSingleton<IDBProvider,DBProvider>();
builder.Services.AddSingleton<DBConnection>();




//Cors
builder.Services.AddCustomCors();




//Defining the service address
builder.WebHost.UseUrls($"{Service.Address}:{Service.Port}");
var app = builder.Build();



app.UseMiddleware<AppExceptionHandlerMiddleware>();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseHeaderPropagation();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SYTYCC V1");
    c.OAuthUsePkce();
});



app.MapControllers();
app.SeedDatabase();
app.Run();













