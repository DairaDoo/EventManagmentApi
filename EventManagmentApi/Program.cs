using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EventManagement.Database;
using EventManagmentApi.Data.Repositories;
using EventManagmentApi.Service;
using EventManagmentApi.Data.Interfaces;
using EventManagmentApi.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar CORS (Permitir cualquier origen, método y cabecera)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configuración de la conexión a la base de datos
builder.Services.AddSingleton<DatabaseInitializer>();  // Inyecta DatabaseInitializer

// Inyección de dependencias para Event
builder.Services.AddScoped<IEventRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new EventRepository(connectionString);
});
builder.Services.AddScoped<IEventService, EventService>();

// Inyección de dependencias para User
builder.Services.AddScoped<IUserRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new UserRepository(connectionString);
});
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Inicializar la base de datos
using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    dbInitializer.InitializeDatabase();
}

// Configuración del middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Management API v1");
        options.RoutePrefix = string.Empty; // Para acceder desde la raíz en producción
    });
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowAll"); // Aplicar la política de CORS

app.UseAuthorization();
app.MapControllers();
app.Run();
