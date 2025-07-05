using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<OtpRecord> OtpRecords { get; set; }
    public DbSet<Resume> Resumes { get; set; }
    public DbSet<Company> Companies { get; set; }



    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
}