using XiangqiCore.Misc;

namespace XiangqiCore.Pieces.ValidationStrategy;

/// <summary>
/// For Bishop and Advisor, they have specific positions that they can move to, so they need to implement this interface
/// </summary>
public interface IHasSpecificPositions
{
	/// <summary>
	/// Get the possible positions that the piece can move to
	/// </summary>
	/// <returns>An array of coordinates that the piece can move to</returns>
	Coordinate[] GetSpecificPositions(Side side);
}
