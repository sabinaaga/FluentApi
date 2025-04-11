using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(entity =>
            {

                entity.Property(e => e.Name).IsRequired().HasMaxLength(20);

            });

            modelBuilder.Entity<Product>(entity =>
            {

                entity.Property(e => e.Name).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Price).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(20);

            });
            modelBuilder.Entity<Book>(entity =>
            {

                entity.Property(e => e.Title).IsRequired().HasMaxLength(20);

            });
            modelBuilder.Entity<Author>(entity =>
            {

                entity.Property(e => e.FullName).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Age).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(30);

            });

        }
    }
}
