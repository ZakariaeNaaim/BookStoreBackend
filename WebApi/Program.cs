using Serilog;
using WebApi.Extensions;

// Configure Serilog
SerilogExtensions.ConfigureSerilog();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Configure Services
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddHttpContextAccessor(); // Required for IUserContextService

// Add configurations using extension methods
builder.Services.AddCorsConfiguration();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddIdentityConfiguration();
builder.Services.AddAuthenticationConfiguration(builder.Configuration);
builder.Services.AddSessionConfiguration();
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure middleware pipeline
app.ConfigureMiddlewarePipeline(builder.Configuration);

app.Run();
