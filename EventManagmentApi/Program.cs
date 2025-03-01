using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EventManagement.Database;
using EventManagmentApi.Data.Repositories;
using EventManagmentApi.Service;
using EventManagmentApi.Controllers;
using EventManagmentApi.Data.Interfaces;
using EventManagmentApi.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de la conexión a la base de datos
builder.Services.AddSingleton<DatabaseInitializer>();  // Inyecta DatabaseInitializer
builder.Services.AddScoped<IEventRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new EventRepository(connectionString);
});
builder.Services.AddScoped<IEventService, EventService>();


var app = builder.Build();

// Inicializar la base de datos
using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    dbInitializer.InitializeDatabase();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configurar la API
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors();
app.Run();
