using Microsoft.EntityFrameworkCore;

namespace CloudBruh.Trustartup.MediaSystem.Models;

public class MediaContext : DbContext
{
    public MediaContext(DbContextOptions<MediaContext> options) : base(options)
    {
        
    }

    public DbSet<Media> Media { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Media>().Property(media => media.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<Media>().Property(media => media.UpdatedAt).HasDefaultValueSql("now()");
    }
}