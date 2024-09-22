namespace XiangqiCore.Attributes;


[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class BetaMethodAttribute : Attribute
{
	const string DefaultBetaMethodMessage = "This method is in beta and may not be fully tested. Use with caution.";

	public string Message { get; private set; }

	public BetaMethodAttribute() 
	{ 
		Message = DefaultBetaMethodMessage;
	}

	public BetaMethodAttribute(string message)
	{
		Message = message;
	}
}
