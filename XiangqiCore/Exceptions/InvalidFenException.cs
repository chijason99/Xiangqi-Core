namespace XiangqiCore.Exceptions;
public class InvalidFenException : Exception
{
    public InvalidFenException() : base() { }

    public InvalidFenException(string? providedFen) : base($"The provided FEN {providedFen} is invalid")
    {
    }

    public InvalidFenException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
