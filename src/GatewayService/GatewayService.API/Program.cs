using GatewayService.API.Extensions;
using Microsoft.IdentityModel.Tokens;
using Shared.Infrastructure.Logging;
using Shared.Infrastructure.Middlewares;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Host.AddBuilderLogging("gateway");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddJwtAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapReverseProxy();

app.UseExceptionHandling();


app.UseCorrelationId();

app.Use((context, next) =>
{
    var traceId = Activity.Current?.TraceId;
    // Push to Serilog context
    //using (Serilog.Context.LogContext.PushProperty("CorrelationId", traceId.ToString()))
    //{
    //    next.Invoke(context);
    //}

    return next(context);
});


app.Run();