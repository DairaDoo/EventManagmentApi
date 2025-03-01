namespace EventManagmentApi.Helpers
{
    public class UserSqlQueries // Queries asociadas con los Users
    {
        // Querie para Obtener todos los usuarios
        public const string GetAllUsers = "SELECT * FROM Users;";

        // Querie para Obtener un usuario en específico por su id
        public const string GetUserById = "SELECT * FROM Users WHERE Id = @Id;";

        // Querie para Obtener usuario por su username
        public const string GetUserByUsername = "SELECT * FROM Users WHERE Username = @Username;";

        // Querie para Crear un nuevo usuario
        public const string CreateUser = @"
        INSERT INTO Users (FirstName, LastName, Username, Email, PasswordHash, Role)
        VALUES (@FirstName, @LastName, @Username, @Email, @PasswordHash, @Role)
        RETURNING Id;";

        // Querie para Actualizar un usuario
        public const string UpdateUser = @"
        UPDATE Users
        SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Role = @Role
        WHERE Id = @Id;";

        // Querie para eliminar un usuario
        public const string DeleteUser = "DELETE FROM Users WHERE Id = @Id";
    }
}
