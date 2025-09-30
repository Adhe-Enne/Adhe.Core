namespace Core.Contracts.Exceptions
{
    public enum EnumBusinessErrorCode
    {
        None= 0,
        Internal,
        UserEmailExists,
        UserNotFound,
        InvalidPassword,
        Unauthorized,
        UserAlreadyExists,
        InvalidRole,
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
