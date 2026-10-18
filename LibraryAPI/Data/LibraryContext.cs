using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        public DbSet<Book> Books => Set<Book>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(b =>
            {
                b.Property(x => x.Title).IsRequired().HasMaxLength(100);
                b.Property(x => x.PurchasePrice).HasPrecision(18, 2);
                b.Property(x => x.InternalNotes).HasMaxLength(500);
                b.Property(x => x.StorageLocation).HasMaxLength(100);

                b.OwnsOne(x => x.Author, a =>
                {
                    a.Property(p => p.FirstName).IsRequired().HasMaxLength(50);
                    a.Property(p => p.LastName).IsRequired().HasMaxLength(50);
                });
            });
        }
    }
}
