using System.Text.Json;
using BookRepository.App.DataAccess;
using BookRepository.App.Domain;

namespace BookRepository.App.Infrastructure
{
    /// <summary>
    /// Class used to set up initial data
    /// </summary>
    public class MigratorHostedService : IHostedService
    {

        private readonly IServiceProvider _serviceProvider;
        public MigratorHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Create a new scope to retrieve scoped services
            using (var scope = _serviceProvider.CreateScope())
            {
                // Get the DbContext instance
                var booksContext = scope.ServiceProvider.GetRequiredService<BooksContext>();

                //Do the migration asynchronously
                
                await booksContext.Database.EnsureDeletedAsync(cancellationToken);
                await booksContext.Database.EnsureCreatedAsync(cancellationToken);

                var assignmentDataPath = Path.Combine(Environment.CurrentDirectory, "AssignmentData.json");
                var initialDataString = await File.ReadAllTextAsync(assignmentDataPath, cancellationToken);
                using (var stream = File.OpenRead(assignmentDataPath))
                {
                    var initialEntities = await JsonSerializer.DeserializeAsync<IEnumerable<Book>>(stream, cancellationToken: cancellationToken) ?? Array.Empty<Book>();

                    await booksContext.AddRangeAsync(initialEntities, cancellationToken);
                    await booksContext.SaveChangesAsync(cancellationToken: cancellationToken);
                }


            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

}
