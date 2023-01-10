using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal;

public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DataContext(DbContextOptions<DataContext> options): base(options)
    {
    }
        
    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Phone).IsUnique();
    }
}