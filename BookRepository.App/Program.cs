using System.Reflection;
using BookRepository.App.Converters;
using BookRepository.App.DataAccess;
using BookRepository.App.Infrastructure;
using FluentValidation;
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
builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

//Set up Mediator
builder.Services.AddMediatR(typeof(Program));


builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddDbContext<BooksContext>();
builder.Services.AddHostedService<MigratorHostedService>();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

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