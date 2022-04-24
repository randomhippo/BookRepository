using BookRepository.App.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookRepository.App.DataAccess
{
    public class BooksContext : DbContext
    {
        public DbSet<Book> Books => Set<Book>();

        private readonly string _databasePath;

        public BooksContext() :base()
        {
            var userAppFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _databasePath = Path.Join(userAppFolder, "books.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("NOCASE");
            modelBuilder.Entity<Book>(
                builder =>
                {
                   
                    //Specify type as double since SqlLite does not support decimal
                    //See https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
                    builder.Property(b => b.Price).HasConversion<double>();
                    builder.Property(b => b.Published).HasColumnType("datetime");
                });
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={_databasePath}");
    }
}
