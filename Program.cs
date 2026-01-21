using Microsoft.EntityFrameworkCore;
using Models;
using Services;
using Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HttpClients (API externa)
builder.Services.AddHttpClient<UsersRepository>(c =>
    c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"));

builder.Services.AddHttpClient<TasksRepository>(c =>
    c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"));

// Services
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<TasksService>();
builder.Services.AddScoped<SummaryService>();

// SQL Server
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

// Crear DB automáticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
    db.Database.EnsureCreated();
}

app.Run();
