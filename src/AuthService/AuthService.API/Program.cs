using AuthService.API.Extensions;
using Shared.Infrastructure.Logging;
using Shared.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.AddBuilderLogging("auth");

builder.Services.AddAppContext().AddRepositories();
builder.Services.AddServices();
builder.Services.AddMediatR().AddPipelineBehavior();
builder.Services.AddJwtAuthentication();

var app = builder.Build();

// Seed initial data
//await app.Services.AddDataSeeder();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/health", () =>
{
    return "AuthService is running ...";
})
.WithName("Health")
.WithOpenApi();

app.UseExceptionHandling();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCorrelationId();

app.Run();