namespace EventManagmentApi.Helpers
{
    public class RegistrationSqlQueries
    {
        public const string GetAllRegistrations = @"
        SELECT r.Id, r.UserId, r.EventId, r.RegistrationDate, r.Status
        FROM Registrations r
        ORDER BY r.RegistrationDate DESC;
    ";

        public const string GetRegistrationById = @"
        SELECT r.Id, r.UserId, r.EventId, r.RegistrationDate, r.Status
        FROM Registrations r
        WHERE r.Id = @Id;
    ";

        public const string GetRegistrationsByUserId = @"
        SELECT r.Id, r.UserId, r.EventId, r.RegistrationDate, r.Status
        FROM Registrations r
        WHERE r.UserId = @UserId;
    ";

        public const string GetRegistrationsByEventId = @"
        SELECT r.Id, r.UserId, r.EventId, r.RegistrationDate, r.Status
        FROM Registrations r
        WHERE r.EventId = @EventId;
    ";

        public const string CreateRegistration = @"
        INSERT INTO Registrations (UserId, EventId, Status)
        VALUES (@UserId, @EventId, @Status)
        RETURNING Id;
    ";

        public const string DeleteRegistration = @"
        DELETE FROM Registrations
        WHERE Id = @Id;
    ";

        public const string UpdateRegistrationStatus = @"
        UPDATE Registrations
        SET Status = @Status
        WHERE Id = @Id;
    ";
    }


}
