# Event Management API

## 📌 Propósito del Proyecto

El proyecto **Event Management API** tiene como objetivo proporcionar una plataforma robusta para gestionar eventos, donde los usuarios puedan registrarse, ver eventos disponibles y realizar pagos en línea. El backend de esta plataforma está desarrollado con **ASP.NET Core** utilizando **Dapper** como ORM para interactuar con la base de datos **PostgreSQL**.

### Características Principales:
- 🗓️ **Gestión de Eventos**: Crear, ver y administrar eventos.
- 🔐 **Autenticación**: Usando **JWT** para asegurar las rutas de la API.
- 💳 **Pagos**: Integración con **Stripe** para procesar pagos.
- 📍 **Ubicación de Eventos**: Usando **Google Maps** para mostrar las ubicaciones de los eventos.
- 🖼️ **Imágenes de Eventos**: Potencialmente utilizando **Cloudinary** para almacenar y servir imágenes de eventos.

---

## ⚙️ Tecnologías Utilizadas

### 1. **ASP.NET Core**
Un framework de alto rendimiento para la creación de aplicaciones web y APIs. Nos ayuda a estructurar el backend de manera eficiente y manejar solicitudes HTTP.

### 2. **Dapper (Micro ORM)**
Un ORM ligero y rápido que nos permite interactuar con la base de datos utilizando consultas SQL directas, lo que nos da control total sobre las operaciones de la base de datos.

### 3. **PostgreSQL**
Un sistema de bases de datos relacional que utilizamos para almacenar información sobre eventos, usuarios y registros de eventos.

### 4. **JWT (JSON Web Token)**
Método de autenticación que permite proteger las rutas de la API, asegurando que solo los usuarios autenticados puedan acceder a ciertas funcionalidades.

### 5. **Stripe**
Una plataforma de pagos en línea que facilita el procesamiento de pagos dentro de la API, permitiendo a los usuarios pagar por sus registros a eventos.

### 6. **Google Maps**
Se integrará para mostrar las ubicaciones de los eventos en un mapa, facilitando que los usuarios encuentren la dirección del evento.

### 7. **Cloudinary (para Imágenes)**
Plataforma recomendada para almacenar y servir imágenes asociadas a los eventos. Cloudinary facilita la carga, almacenamiento y entrega de imágenes de manera eficiente.

---

## 🗂️ Estructura del Proyecto

### 1. **Controllers**
Los controladores gestionan las solicitudes HTTP y responden con la información adecuada. Cada controlador tiene una responsabilidad específica, como manejar eventos, usuarios o registros.

#### Ejemplo de Controlador: `TestingController`

```csharp
using Microsoft.AspNetCore.Mvc;

namespace EventManagmentApi.Controllers
{
    public class TestingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("✅ API is working!");
        }
    }
}
```
Este controlador tiene una ruta simple que responde con un mensaje para verificar que la API está funcionando correctamente.

2. Services
Los servicios encapsulan la lógica de negocio. En este proyecto, se utilizan para manejar operaciones como registrar usuarios, crear eventos y procesar pagos.

3. DatabaseInitializer
Esta clase es responsable de crear la base de datos y las tablas necesarias al iniciar la aplicación.

Crear la Base de Datos
```csharp
private void CreateDatabaseIfNotExists()
{
    using var connection = new NpgsqlConnection(_masterConnectionString);
    connection.Open();

    var checkDbQuery = "SELECT 1 FROM pg_database WHERE datname = @DbName";
    var dbExists = connection.ExecuteScalar<int?>(checkDbQuery, new { DbName = _databaseName });

    if (dbExists == null)
    {
        Console.WriteLine($"🔹 Creando la base de datos: {_databaseName}...");
        connection.Execute($"CREATE DATABASE \"{_databaseName}\"");
        Console.WriteLine("✅ Base de datos creada correctamente.");
    }
    else
    {
        Console.WriteLine("⚡ La base de datos ya existe. No se necesita crearla.");
    }
}
```

4. Uso de Interfaces y Modelos
Las interfaces permiten que el código esté desacoplado y sea más fácil de mantener. Los modelos representan las entidades de negocio como Eventos, Usuarios y Registros, asegurando que la estructura de la base de datos y el código estén alineados.

Comandos SQL para Crear la Base de Datos y Tablas
sql
Copy code
-- Crear base de datos
```sql
CREATE DATABASE eventdb;
```
-- Usar la base de datos creada
\c eventdb;

-- Crear la tabla de Eventos
```sql
CREATE TABLE IF NOT EXISTS Events (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Description TEXT,
    Location VARCHAR(255),
    Date TIMESTAMP NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    ImageUrl TEXT NULL
);

-- Crear la tabla de Usuarios
CREATE TABLE IF NOT EXISTS Users (
    Id SERIAL PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Username VARCHAR(100) UNIQUE NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    PasswordHash TEXT NOT NULL,
    Role VARCHAR(50) NOT NULL DEFAULT 'User'
);

-- Crear la tabla de Registros
CREATE TABLE IF NOT EXISTS Registrations (
    Id SERIAL PRIMARY KEY,
    UserId INT REFERENCES Users(Id) ON DELETE CASCADE,
    EventId INT REFERENCES Events(Id) ON DELETE CASCADE,
    RegistrationDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

Este script SQL crea la base de datos eventdb y las tablas necesarias para el sistema: Events, Users, y Registrations.

Consultas SQL en Dapper
Las consultas SQL se gestionan en archivos separados para mantener el código limpio y modular. Por ejemplo, para obtener todos los eventos registrados en la base de datos:

sql
Copy code
-- Ejemplo de consulta en Dapper
````sql
SELECT * FROM Events;
```
🛠️ Instrucciones para Ejecutar el Proyecto
Instalar Dependencias: Antes de ejecutar la aplicación, asegúrate de que todas las dependencias estén instaladas:
```console
dotnet restore
```

Ejecutar el Proyecto: Para ejecutar la aplicación, utiliza el siguiente comando:
```console
dotnet run
```
Acceder a la API:

Swagger UI: La interfaz interactiva de Swagger estará disponible en https://localhost:{port}/swagger para probar las rutas de la API.
API Endpoints: Puedes realizar solicitudes HTTP a las rutas de los controladores para obtener o enviar información (e.g., ver eventos, registrar usuarios).

🚀 Próximos Pasos y Mejoras
Integración de Stripe: Se implementará Stripe para manejar pagos de usuarios al registrarse en eventos. Esto incluirá la creación de un método de pago seguro en la API.
Integración de Google Maps: Usaremos la API de Google Maps para obtener y mostrar ubicaciones de eventos, facilitando la geolocalización.
Almacenamiento de Imágenes: Utilizaremos Cloudinary para almacenar y servir imágenes relacionadas con los eventos, como banners o imágenes descriptivas.

🎯 Conclusión
Este proyecto proporciona una base sólida para una plataforma de gestión de eventos, utilizando prácticas modernas como la separación de responsabilidades, el uso de Dapper como ORM para SQL, y una arquitectura basada en servicios y controladores. A medida que se añaden características como Stripe y Google Maps, la plataforma evolucionará para ser una solución completa para la gestión de eventos.
