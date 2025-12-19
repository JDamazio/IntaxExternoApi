using Microsoft.EntityFrameworkCore;
using IntaxExterno.Api.Helpers.Auth;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Infra.Data.Context;
using IntaxExterno.Infra.IoC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddInsfrastructure(builder.Configuration);
builder.Services.AddInfrastructureSwagger();
builder.Services.AddScoped<Auth>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

CreateDatabase(app);
InitializeSeedData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseInfrastructureSwagger();
}

//app.UseMiddleware<ExceptionMiddleware>();

// Only redirect to HTTPS in Development (Production uses reverse proxy)
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();

app.UseAuthorization();

app.UseCors("AllowAnyOrigin");

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    timestamp = DateTime.UtcNow,
    environment = app.Environment.EnvironmentName
})).AllowAnonymous();

app.MapControllers();

app.Run();

static void CreateDatabase(WebApplication app)
{
    var serviceScope = app.Services.CreateScope();
    var dataContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
    dataContext?.Database.Migrate();
}

static void InitializeSeedData(WebApplication app)
{
    using var serviceScope = app.Services.CreateScope();
    var seeder = serviceScope.ServiceProvider.GetService<ISeedUserAndRoleInitial>();
    seeder?.SeedRoles();
    seeder?.SeedUsers();
}
