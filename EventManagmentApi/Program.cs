using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EventManagement.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();  // Add the MVC Controllers for Web API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// Add database initialization
var configuration = builder.Configuration;
var databaseInitializer = new DatabaseInitializer(configuration);
databaseInitializer.InitializeDatabase();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline
app.MapControllers();  // Map API controllers to routes

app.Run();  // Run the web application
