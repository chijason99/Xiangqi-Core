namespace XiangqiCore.Exceptions;
public class InvalidPieceTypeException : Exception
{
    public InvalidPieceTypeException() : base() { }

    public InvalidPieceTypeException(string message) : base(message) { }
}
