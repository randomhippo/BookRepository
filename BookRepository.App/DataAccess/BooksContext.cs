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
            //TODO: It can be considered ok for demo, but database path should not be hardcoded in a real app
            //This is required to have separate SQLLite databases for integration testing and app. App should not reset database.
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
                    
                    //Add computed column for sorting on ID
                    builder.Property(b => b.PresentedId).HasComputedColumnSql("'B' || Id", stored:false);
                });
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={_databasePath}");
    }
}
