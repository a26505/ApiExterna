using Microsoft.EntityFrameworkCore; 

namespace Models; 

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) {}
    public DbSet<Log> Logs { get; set; }
}

public class Log
{
    public int Id { get; set; }
    public string Endpoint { get; set; } = "";
    public DateTime CalledAt { get; set; } = DateTime.UtcNow;
    public string Message { get; set; } = "";
}