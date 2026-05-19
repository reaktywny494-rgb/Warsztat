using Microsoft.EntityFrameworkCore;
using Warsztat.Models;

namespace Warsztat.Data
{
    public class WarsztatContext : DbContext
    {
        public WarsztatContext(DbContextOptions<WarsztatContext> options)
            : base(options)
        {
        }
        public DbSet<Mebel> Meble { get; set; } = default!;
        public DbSet<Narzedzie> Narzedzia { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Narzedzie>()
                .HasOne(n => n.Mebel)
                .WithMany(m => m.Narzedzia)
                .HasForeignKey(n => n.Mebel_ID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
