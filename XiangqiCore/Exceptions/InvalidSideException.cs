namespace XiangqiCore.Exceptions;
public class InvalidSideException : Exception
{
    public InvalidSideException() : base() { }

    public InvalidSideException(string message) : base(message) { }

    public InvalidSideException(Side invalidSide) : base($"Invalid Side {Enum.GetName(invalidSide)}") { }

    public InvalidSideException(string message, Exception innerException) : base(message, innerException) { }
}
