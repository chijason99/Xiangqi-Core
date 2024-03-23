namespace XiangqiCore.Exceptions;
public class InvalidFenException(string providedFen) : Exception($"The provided FEN {providedFen} is invalid")
{
}
