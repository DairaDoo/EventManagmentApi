using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EventManagement.Database;
using EventManagmentApi.Data.Repositories;
using EventManagmentApi.Service;
using EventManagmentApi.Data.Interfaces;
using EventManagmentApi.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings.GetValue<string>("SecretKey"));

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

// Inyección para JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            
        };
});
builder.Services.AddScoped<IAuthService, AuthService>();


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

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
