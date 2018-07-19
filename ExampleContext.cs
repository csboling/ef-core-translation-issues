namespace Issues.OpsAfterProjection
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;

    public class ExampleDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
            builder
                .UseSqlite("Data Source=example.db")
                .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var library = modelBuilder.Entity<Library>();
            library
                .HasMany(l => l.Books)
                .WithOne(b => b.Library);
            library
                .HasOne(l => l.City)
                .WithOne(c => c.Library)
                .HasForeignKey<City>(c => c.LibraryId);
            var city = modelBuilder.Entity<City>();
            city
                .HasOne(c => c.Library)
                .WithOne(l => l.City)
                .HasForeignKey<City>(c => c.LibraryId);
            var book = modelBuilder.Entity<Book>();
            book
                .HasOne(b => b.Library)
                .WithMany(l => l.Books);
            base.OnModelCreating(modelBuilder);
        }
    }
}
