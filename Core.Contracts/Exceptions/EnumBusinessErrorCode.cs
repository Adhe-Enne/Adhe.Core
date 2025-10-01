namespace Core.Contracts.Exceptions
{
    public enum EnumBusinessErrorCode
    {
        None= 0,
        Internal,
        UserEmailExists,
        UserNotFound,
        UserLockedOut,
        InvalidPassword,
        Unauthorized,
        UserAlreadyExists,
        InvalidRole,
        InvalidToken,
        UnrecognizedRole,
        EntityNotFound,
        BoardNotFound,
        ColumnNotFound,
        TaskNotFound,
        ValidationError,
        Forbidden,
        Conflict,
        InternalError,
        BoardHasActiveTasks // <-- Agregado para la validación de tareas activas en el tablero
        // Agrega aquí más códigos según tus necesidades

    }
}
