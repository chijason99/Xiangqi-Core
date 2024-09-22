namespace XiangqiCore.Exceptions;

public class ParesMoveRecordException : Exception
{
	public ParesMoveRecordException(string message) : base(message)
	{
	}

	public ParesMoveRecordException(string message, Exception innerException) : base(message, innerException)
	{
	}

	public ParesMoveRecordException()
	{
	}
}
