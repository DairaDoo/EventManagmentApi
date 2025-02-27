namespace EventManagmentApi.Helpers
{
    public class SqlQueries // comandos SQL para los Eventos
    {
        public const string GetAllEvents = @"SELECT * FROM Events"; // obtener todos los eventos

        public const string GetEventById = @"SELECT * FROM Events WHERE Id = @Id"; // obtener evento por Id

        public const string CreateEvent = @"INSERT INTO Events (Name, Description, Location, Date, Price,
        ImageUrl) VALUES (@Name, @Description, @Location, @Date, @Price, @ImageUrl)
        RETURNING Id"; // Crear Evento

        public const string UpdateEvent = @"
        UPDATE Events 
        SET Name = @Name, Description = @Description, Location = @Location, Date = @Date,
        Price = @Price, ImageUrl = @ImageUrl WHERE Id = @Id"; // Actualizar un Evento

        public const string DeleteEvent = @"DELETE FROM Events WHERE Id = @Id"; // Eliminar un Evento por ID.
    }
}
