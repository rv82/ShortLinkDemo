using Microsoft.EntityFrameworkCore;
using ShortLinkDemo.Models;

namespace ShortLinkDemo.Data
{
    public class ShortLinkContext : DbContext
    {
        public ShortLinkContext(DbContextOptions<ShortLinkContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortLink>().HasIndex(l => l.Id);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ShortLink> ShortLinks { get; set; }
    }
}
