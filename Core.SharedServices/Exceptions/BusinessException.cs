using Core.Contracts.Exceptions;
namespace Core.SharedServices.Exceptions
{
    public class BusinessException : Exception, IBusinessException
    {
        public EnumBusinessErrorCode ErrorCode { get; }
        public string? Reason { get; }

        public BusinessException(string message, EnumBusinessErrorCode errorCode, string? reason = null)
            : base(message)
        {
            ErrorCode = errorCode;
            Reason = reason;
        }
    }
}
