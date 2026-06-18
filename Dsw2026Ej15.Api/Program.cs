var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<Dsw2026Ej15.Data.IPersistence, Dsw2026Ej15.Data.PersistenceInMemory>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseMiddleware<Dsw2026Ej15.Api.ExceptionMiddleware>();
app.MapHealthChecks("/health-check");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
