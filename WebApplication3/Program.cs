using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Repositories;
using WebApplication3.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddScoped<WorkoutRepository>();
builder.Services.AddScoped<WorkoutService>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();