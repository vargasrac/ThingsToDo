using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ThingsToDo.BLService;
using ThingsToDo.Data;
using ThingsToDo.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ToDoTaskContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ToDoTaskContext") ?? throw new InvalidOperationException("Connection string 'ToDoTaskContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IBLService, BLService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi("document");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

app.MapOpenApi();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
