namespace Core.Framework
{
    public class Result<R, E> : Result<E>
    {
        public R Data { get; set; }
    }

    public class Result<E>
    {
        public E Error { get; private set; }

        public bool IsSuccess { get; private set; } = true;

        public bool IsFailure => !IsSuccess;

        public Result<E> SetError(E error) 
        { 
            Error = error;
            IsSuccess = false;
            return this;
        }
    }
}
