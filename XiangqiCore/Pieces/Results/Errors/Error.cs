namespace XiangqiCore.Results.Errors;
public sealed record Error(string errorType, string errorMessage)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}
