namespace Core.Contracts.Exceptions
{
    public interface IBusinessException
    {
        EnumBusinessErrorCode ErrorCode { get; }
        string? Reason { get; }
        string Message { get; }
    }
}
