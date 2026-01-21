using Microsoft.EntityFrameworkCore;
using Models;
using Services;
using Repositories;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURACIÓN DE CONTROLADORES Y SWAGGER ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- 2. REGISTRO DE REPOSITORIOS (Interfaces + HttpClient) ---
// Registramos la interfaz y la clase, configurando la URL de la API externa
builder.Services.AddHttpClient<IUsersRepository, UsersRepository>(c =>
    c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"));

builder.Services.AddHttpClient<ITasksRepository, TasksRepository>(c =>
    c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"));

// --- 3. REGISTRO DE SERVICIOS (Inversión de Dependencias) ---
// Ahora los controladores pedirán la Interfaz y recibirán estas clases
builder.Services.AddScoped<IUsersService, UserService>();
builder.Services.AddScoped<ITasksService, TasksService>();
builder.Services.AddScoped<ISummaryService, SummaryService>();

// --- 4. CONFIGURACIÓN DE BASE DE DATOS (MySQL Pomelo) ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseMySql(
        connectionString, 
        new MySqlServerVersion(new Version(8, 0, 30)) // Versión fija para evitar el error de AutoDetect
    ));

var app = builder.Build();

// --- 5. MIDDLEWARE ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

// --- 6. INICIALIZACIÓN ROBUSTA (Solución al Timeout de MySQL) ---
// Intentamos conectar a la DB varias veces antes de que la App falle
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var db = services.GetRequiredService<ApiDbContext>();

    int retries = 10;
    while (retries > 0)
    {
        try
        {
            logger.LogInformation("Verificando conexión con MySQL...");
            db.Database.EnsureCreated(); // Crea la base de datos y tablas si no existen
            logger.LogInformation("¡Base de datos conectada con éxito!");
            break;
        }
        catch (Exception ex)
        {
            retries--;
            logger.LogWarning($"MySQL aún no está listo. Reintentando... ({retries} intentos restantes)");
            Thread.Sleep(5000); // Espera 5 segundos entre reintentos
        }
    }
}

app.Run();