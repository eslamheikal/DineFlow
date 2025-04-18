using AuthService.API.Extensions;
using Shared.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAppContext().AddRepositories();
builder.Services.AddServices();
builder.Services.AddMediatR().AddPipelineBehavior();
builder.Services.AddJwtAuthentication();
builder.Services.AddCORS();

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandling();
app.UseCorrelationId();

app.Run();