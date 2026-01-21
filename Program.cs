using Microsoft.EntityFrameworkCore;
using Models;
using Services;
using Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configuración básica
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- HTTP CLIENTS (Consumo de API Externa) ---
builder.Services.AddHttpClient<IUsersRepository, UsersRepository>(c =>
    c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"));

builder.Services.AddHttpClient<ITasksRepository, TasksRepository>(c =>
    c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"));

// --- INYECCIÓN DE DEPENDENCIAS ---
builder.Services.AddScoped<IUsersService, UserService>();
builder.Services.AddScoped<ITasksService, TasksService>();
builder.Services.AddScoped<ISummaryService, SummaryService>();

// --- BASE DE DATOS MYSQL ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30))));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

// --- LÓGICA DE REINTENTOS PARA DOCKER ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    int retries = 10;

    while (retries > 0)
    {
        try
        {
            db.Database.EnsureCreated();
            logger.LogInformation(">>> MySQL conectado correctamente.");
            break;
        }
        catch (Exception)
        {
            retries--;
            logger.LogWarning($">>> MySQL no listo. Reintentando... ({retries} intentos)");
            Thread.Sleep(5000);
        }
    }
}

app.Run();