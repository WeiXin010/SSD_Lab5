using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public AppDbContext(AppDbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
}