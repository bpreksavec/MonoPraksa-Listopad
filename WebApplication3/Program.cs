using WebApplication3.Repositories;
using WebApplication3.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<WorkoutRepository>();
builder.Services.AddScoped<WorkoutService>();
builder.Services.AddTransient<WorkoutValidator>();
builder.Services.AddSingleton<WorkoutStatistics>();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
