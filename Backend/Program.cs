using Backend.Services;
var builder = WebApplication.CreateBuilder(args);

// Add controller services
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddSingleton<TaskService>();
builder.Services.AddHostedService<ConsoleCommandListener>();
var app = builder.Build();

// Middleware
app.UseHttpsRedirection();
app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();