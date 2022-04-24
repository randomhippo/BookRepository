using BookRepository.App;
using BookRepository.App.Converters;
using BookRepository.App.DataAccess;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter("yyyy-MM-dd"));
    options.JsonSerializerOptions.Converters.Add(new DecimalFormatConverter("N2"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Set up Mediator
builder.Services.AddMediatR(typeof(Program));

builder.Services.AddDbContext<BooksContext>();

builder.Services.AddHostedService<MigratorHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


// Needed to have class accessible for test, since internal by default
public partial class Program { }