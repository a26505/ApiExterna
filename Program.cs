using Microsoft.EntityFrameworkCore;
using Models;
using Services;
using Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<UsersRepository>(c =>
    c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"));
builder.Services.AddHttpClient<TasksRepository>(c =>
    c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"));

builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<TasksService>();
builder.Services.AddScoped<SummaryService>();

// --- CAMBIO AQUÍ: No usar AutoDetect ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30))));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// --- BUCLE DE REINTENTOS ROBUSTO ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    bool connected = false;
    int retries = 0;

    while (!connected && retries < 15)
    {
        try
        {
            logger.LogInformation("Esperando a MySQL... Intento {0}", retries + 1);
            db.Database.EnsureCreated();
            connected = true;
            logger.LogInformation("¡Conectado con éxito!");
        }
        catch (Exception ex)
        {
            retries++;
            logger.LogWarning("MySQL no responde todavía. Esperando 5 segundos...");
            Thread.Sleep(5000);
        }
    }
}

app.Run();