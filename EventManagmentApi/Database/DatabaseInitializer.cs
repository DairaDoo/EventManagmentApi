using System;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace EventManagement.Database
{
    public class DatabaseInitializer
    {
        private readonly string _masterConnectionString;
        private readonly string _databaseConnectionString;
        private readonly string _databaseName;

        public DatabaseInitializer(IConfiguration configuration)
        {
            var dbSettings = configuration.GetConnectionString("DefaultConnection");
            var builder = new NpgsqlConnectionStringBuilder(dbSettings);

            _databaseName = builder.Database;
            _databaseConnectionString = dbSettings;
            _masterConnectionString = $"Host={builder.Host};Port={builder.Port};Username={builder.Username};Password={builder.Password};Database=postgres";
        }

        public void InitializeDatabase()
        {
            CreateDatabaseIfNotExists();
            CreateTablesIfNotExists();
        }

        private void CreateDatabaseIfNotExists()
        {
            using var connection = new NpgsqlConnection(_masterConnectionString);
            connection.Open();

            // Verificar si la base de datos ya e   xiste
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

        private void CreateTablesIfNotExists()
        {
            using var connection = new NpgsqlConnection(_databaseConnectionString);
            connection.Open();

            var createTablesQuery = @"
                CREATE TABLE IF NOT EXISTS Events (
                    Id SERIAL PRIMARY KEY,
                    Name VARCHAR(255) NOT NULL,
                    Description TEXT,
                    Location VARCHAR(255),
                    Date TIMESTAMP NOT NULL,
                    Price DECIMAL(10,2) NOT NULL,
                    ImageUrl TEXT NULL
                );

                CREATE TABLE IF NOT EXISTS Users (
                    Id SERIAL PRIMARY KEY,
                    FirstName VARCHAR(100) NOT NULL,
                    LastName VARCHAR(100) NOT NULL,
                    Username VARCHAR(100) UNIQUE NOT NULL,
                    Email VARCHAR(255) UNIQUE NOT NULL,
                    PasswordHash TEXT NOT NULL,
                    Role VARCHAR(50) NOT NULL DEFAULT 'User'
                );

                CREATE TABLE IF NOT EXISTS Registrations (
                    Id SERIAL PRIMARY KEY,
                    UserId INT REFERENCES Users(Id) ON DELETE CASCADE,
                    EventId INT REFERENCES Events(Id) ON DELETE CASCADE,
                    RegistrationDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                );
            ";

            connection.Execute(createTablesQuery);
            Console.WriteLine("✅ Tables created/verified successfully.");
        }

    }
}
